using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using FunctionalityMatrixApp.Services;
using FunctionalityMatrixApp.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalityMatrixApp.Pages.Products
{
    public class ListModel : PageModel, ISearchable
    {
        private readonly IProductsData productsData;
        private readonly IConfiguration configuration;

        public IEnumerable<ProductModelWrapper> WrappedProducts { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public ProductType ProductFilter { get; set; }

        public ListModel(IProductsData productsData, IConfiguration configuration)
        {
            this.productsData = productsData;
            this.configuration = configuration;
        }

        public void OnGet(ProductType productFilter)
        {
            ProductFilter = productFilter;

            if (productFilter == 0)
            {
                WrappedProducts = productsData.GetByName(SearchTerm).Select(p =>
                {
                    var defaultPictureURL = GetDefaultPictureURL(p);
                    var shortenedContent = GetShortenedContent(p.Content);
                    return new ProductModelWrapper(p)
                    {
                        DefaultPictureURL = defaultPictureURL,
                        ShortenedContent = shortenedContent
                    };
                });
            }
            else
            {
                WrappedProducts = productsData.GetByName(SearchTerm, ProductFilter).Select(p =>
                {
                    var defaultPictureURL = GetDefaultPictureURL(p);
                    var shortenedContent = GetShortenedContent(p.Content);
                    return new ProductModelWrapper(p)
                    {
                        DefaultPictureURL = defaultPictureURL,
                        ShortenedContent = shortenedContent
                    };
                });
            }
        }

        private string GetShortenedContent(string content)
        {
            if (content.Length > 200)
            {
                return $"{content.Substring(0, 200)}...";
            }
            else
            {
                return content;
            }
        }

        private string GetDefaultPictureURL(Product product)
        {
            var picture = product.Pictures.FirstOrDefault();
            var path = configuration.GetValue<string>("UploadPaths:Pictures");
            if (picture != null)
            {
                return path + picture.Name;
            }
            else
            {
                return null;
            }
        }
    }
}





