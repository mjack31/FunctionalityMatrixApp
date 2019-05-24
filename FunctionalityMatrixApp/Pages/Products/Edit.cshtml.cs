using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using FunctionalityMatrixApp.Services.ServerFilesManager;
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
        private readonly IServerFilesManager serverFilesManager;

        private string picturesPath;
        private string attachmentsPath;

        public EditModel(IProductsData productsData, 
            IConfiguration configuration, 
            IHtmlHelper htmlHelper, 
            IServerFilesManager serverFilesManager)
        {
            this.productsData = productsData;
            this.configuration = configuration;
            this.htmlHelper = htmlHelper;
            this.serverFilesManager = serverFilesManager;
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
                if (Product.ParentId.HasValue)
                {
                    SelectedParentId = Product.ParentId.Value;
                }
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
            picturesPath = serverFilesManager.GetEnvFullPath(configuration.GetValue<string>("UploadPaths:Pictures"));
            attachmentsPath = serverFilesManager.GetEnvFullPath(configuration.GetValue<string>("UploadPaths:Attachments"));

            GetAvailableParentsAsSelectListItem();
            if(SelectedParentId != -1)
            {
                Product.ParentId = SelectedParentId;
            } else
            {
                Product.ParentId = null;
            }

            var uploadedPicturesNames = await serverFilesManager.UploadFilesToServer(PicturesUpload, picturesPath);
            var uploadedAttachmentsNames = await serverFilesManager.UploadFilesToServer(AttachmentsUpload, attachmentsPath);
            AddPicturesToProduct(uploadedPicturesNames);
            AddAttachmentsToProduct(uploadedAttachmentsNames);

            var deletedPictures = productsData.RemovePictures(PicturesIdsToDelete).Select(p => p.Name);
            var deletedAttachments = productsData.RemoveAttachments(AttachmentsIdsToDelete).Select(p => p.Name);
            serverFilesManager.DeleteFilesFromServer(deletedPictures, picturesPath);
            serverFilesManager.DeleteFilesFromServer(deletedAttachments, attachmentsPath);

            if (ModelState.IsValid)
            {
                if (Product.Id > 0)
                {
                    productsData.Update(Product);
                    productsData.Commit();
                    TempData["Message"] = "Product updated";
                    return RedirectToPage("Details", new { productId = Product.Id });
                }
                else
                {
                    productsData.Add(Product);
                    productsData.Commit();
                    TempData["Message"] = "Product added";
                    return RedirectToPage("List");
                }
            }
            else
            {
                return Page();
            }
        }

        private void AddAttachmentsToProduct(List<string> attachments)
        {
            foreach (var attachment in attachments)
            {
                var attachmentObject = new Attachment() { Name = attachment };
                Product.Attachments.Add(attachmentObject);
            }
        }

        private void AddPicturesToProduct(List<string> pictures)
        {
            foreach (var picture in pictures)
            {
                var pictureObject = new Picture() { Name = picture };
                Product.Pictures.Add(pictureObject);
            }
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