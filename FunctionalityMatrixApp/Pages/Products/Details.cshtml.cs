using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FunctionalityMatrixApp.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductsData productsData;
        private readonly IConfiguration configuration;

        public DetailsModel(IProductsData productsData, IConfiguration configuration)
        {
            this.productsData = productsData;
            this.configuration = configuration;
        }

        public Product Product { get; set; }
        public IEnumerable<string> Pictures { get; set; }
        public IEnumerable<string> Attachments { get; set; }
        public IEnumerable<Product> ChildProducts { get; set; }
        public string PicturesPath { get; set; }
        public string AttachmentsPath { get; set; }

        public IActionResult OnGet(int productId)
        {
            PicturesPath = configuration.GetValue<string>("UploadPaths:Pictures");
            AttachmentsPath = configuration.GetValue<string>("UploadPaths:Attachments");

            Product = productsData.GetById(productId);

            if(Product == null)
            {
                return RedirectToPage("NotFound");
            }
            else
            {
                Pictures = productsData.GetProductPicturesURLs(productId, PicturesPath);
                Attachments = productsData.GetProductAttachmentsURLs(productId, AttachmentsPath);
                ChildProducts = productsData.GetChilds(productId);

                return Page();
            }
        }
    }
}