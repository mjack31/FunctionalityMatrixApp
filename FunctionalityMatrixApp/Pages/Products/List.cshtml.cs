using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunctionalityMatrixApp.Pages.Products
{
    public class ListModel : PageModel
    {
        private readonly IProductsData productsData;

        public IEnumerable<Product> Products { get; set; }

        public ListModel(IProductsData productsData)
        {
            this.productsData = productsData;
        }
        public void OnGet()
        {
            Products = productsData.GetAll();
        }
    }
}