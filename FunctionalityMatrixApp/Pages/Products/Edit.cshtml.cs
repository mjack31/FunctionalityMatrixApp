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
        public IFormFile[] PicturesUpload { get; set; }

        [BindProperty]
        public IFormFile[] AttachmentsUpload { get; set; }

        public IEnumerable<SelectListItem> ProductTypes { get; set; }
        public IEnumerable<SelectListItem> AvailableParents { get; set; }

        public void OnGet()
        {
            GetAvailableParentsAsSelectListItem();
        }

        public async Task OnPostAsync()
        {
            GetAvailableParentsAsSelectListItem();

            await PicturesUploadToServer();
            await AttachmentsUploadToServer();

            productsData.Add(Product);
            productsData.Commit();
        }

        private async Task PicturesUploadToServer()
        {
            foreach (var uploadFile in PicturesUpload)
            {
                var pathString = configuration.GetValue<string>("UploadPaths:Pictures");
                var pathLocations = pathString.Split("/");

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
                    Text = p.Name,
                };
            });
            AvailableParents.Append(null);
        }
    }
}