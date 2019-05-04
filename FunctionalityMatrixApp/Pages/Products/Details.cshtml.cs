using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
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
            Path = configuration.GetValue<string>("UploadPaths:Pictures");
        }

        public Product Product { get; set; }
        public IEnumerable<string> Pictures { get; set; }
        public IEnumerable<string> Attachments { get; set; }
        public string Path { get; set; }

        public IActionResult OnGet(int productId)
        {
            Product = productsData.GetById(productId);
            Pictures = GetPicturesURLs();
            Attachments = GetAttachmentsURLs();

            return Page();
        }

        private IEnumerable<string> GetAttachmentsURLs()
        {
            foreach (var attachment in Product.Attachments)
            {
                yield return Path + attachment.Name;
            }
        }

        private IEnumerable<string> GetPicturesURLs()
        {
            foreach (var picture in Product.Pictures)
            {
                yield return Path + picture.Name;
            }
        }
    }
}