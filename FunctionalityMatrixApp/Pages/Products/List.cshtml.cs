using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using FunctionalityMatrixApp.Wrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FunctionalityMatrixApp.Pages.Products
{
    public class ListModel : PageModel
    {
        private readonly IProductsData productsData;
        private readonly IConfiguration configuration;

        public IEnumerable<ProductModelWrapper> WrappedProducts { get; set; }
        public ListModel(IProductsData productsData, IConfiguration configuration)
        {
            this.productsData = productsData;
            this.configuration = configuration;
        }

        public void OnGet()
        {
            WrappedProducts = productsData.GetAll().Select(p =>
            {
                var defaultPictureURL = GetDefaultPictureURL(p);
                return new ProductModelWrapper(p)
                {
                    DefaultPictureURL = defaultPictureURL,
                };
            });
        }

        private string GetDefaultPictureURL(Product product)
        {
            var picture = product.Pictures.FirstOrDefault();
            var path = configuration.GetValue<string>("UploadPaths:Pictures");
            if(picture != null)
            {
                return path + picture.Name;
            }
            else
            {
                return "No pictures";
            }
        }
    }
}





