using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace FunctionalityMatrixApp.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductsData productsData;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHtmlHelper htmlHelper;

        public EditModel(IProductsData productsData, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHtmlHelper htmlHelper)
        {
            this.productsData = productsData;
            this.configuration = configuration;
            this.htmlHelper = htmlHelper;
            this.webHostEnvironment = webHostEnvironment;
            this.htmlHelper = htmlHelper;
            ProductTypes = htmlHelper.GetEnumSelectList<ProductType>();
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public int SelectedParentId { get; set; }

        [BindProperty]
        public List<int> AttachmentsIdsToDelete { get; set; }

        [BindProperty]
        public List<int> PicturesIdsToDelete { get; set; }

        [BindProperty]
        public IFormFile[] PicturesUpload { get; set; }

        [BindProperty]
        public IFormFile[] AttachmentsUpload { get; set; }

        public IEnumerable<SelectListItem> ProductTypes { get; set; }
        public IEnumerable<SelectListItem> AvailableParents { get; set; }

        public IActionResult OnGet(int? productId)
        {
            GetAvailableParentsAsSelectListItem();

            if (productId.HasValue)
            {
                Product = productsData.GetById(productId.Value);
            }
            else
            {
                Product = new Product();
            }

            if(Product == null)
            {
                return RedirectToPage("NotFound");
            }
            else
            {
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            GetAvailableParentsAsSelectListItem();
            var selectedParent = productsData.GetById(SelectedParentId);
            if(selectedParent != null)
            {
                Product.ParentId = selectedParent.Id;
            };
            
            await PicturesUploadToServer();
            await AttachmentsUploadToServer();

            var deletedPictures = productsData.RemovePictures(PicturesIdsToDelete);
            var deletedAttachments = productsData.RemoveAttachments(AttachmentsIdsToDelete);

            PicturesDeleteFromServer(deletedPictures);
            AttachmentsDeleteFromServer(deletedAttachments);


            if (Product.Id > 0)
            {
                productsData.Update(Product);
                productsData.Commit();
                return RedirectToPage("Details", new { productId = Product.Id});
            }
            else
            {
                productsData.Add(Product);
                productsData.Commit();
                return RedirectToPage("List");
            }
        }

        private async Task PicturesUploadToServer()
        {
            var pathString = configuration.GetValue<string>("UploadPaths:Pictures");
            var pathLocations = pathString.Split("/");

            foreach (var uploadFile in PicturesUpload)
            {
                var uniqueName = GetUniqueFileName(uploadFile.FileName);
                var path = Path.Combine(webHostEnvironment.WebRootPath, pathLocations[0], pathLocations[1], pathLocations[2], pathLocations[3], uniqueName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fileStream);
                }

                var picture = new Picture() { Name = uniqueName };
                Product.Pictures.Add(picture);
            }
        }

        private async Task AttachmentsUploadToServer()
        {
            var pathString = configuration.GetValue<string>("UploadPaths:Attachments");
            var pathLocations = pathString.Split("/");

            foreach (var uploadFile in AttachmentsUpload)
            {
                var uniqueName = GetUniqueFileName(uploadFile.FileName);
                var path = Path.Combine(webHostEnvironment.WebRootPath, pathLocations[0], pathLocations[1], pathLocations[2], pathLocations[3], uniqueName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fileStream);
                }

                var attachment = new Attachment() { Name = uniqueName };
                Product.Attachments.Add(attachment);
            }
        }

        private void AttachmentsDeleteFromServer(IEnumerable<Attachment> attachmentsToDelete)
        {
            var pathString = configuration.GetValue<string>("UploadPaths:Attachments");
            var pathLocations = pathString.Split("/");

            foreach (var attachment in attachmentsToDelete)
            {
                var path = Path.Combine(webHostEnvironment.WebRootPath, pathLocations[0], pathLocations[1], pathLocations[2], pathLocations[3], attachment.Name);
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
        }

        private void PicturesDeleteFromServer(IEnumerable<Picture> picturesToDelete)
        {
            var pathString = configuration.GetValue<string>("UploadPaths:Pictures");
            var pathLocations = pathString.Split("/");

            foreach (var picture in picturesToDelete)
            {
                var path = Path.Combine(webHostEnvironment.WebRootPath, pathLocations[0], pathLocations[1], pathLocations[2], pathLocations[3], picture.Name);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
        }

        private string GetUniqueFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            fileName = Path.GetFileNameWithoutExtension(fileName);
            var guid = Guid.NewGuid().ToString().Substring(0, 10);

            return $"{fileName}_{guid}{extension}";
        }

        private void GetAvailableParentsAsSelectListItem()
        {
            AvailableParents = productsData.GetAll().Select(p =>
            {
                return new SelectListItem()
                {
                    Text = $"{p.Name} - id:{p.Id}",
                    Value = p.Id.ToString()
                };
            });
        }
    }
}