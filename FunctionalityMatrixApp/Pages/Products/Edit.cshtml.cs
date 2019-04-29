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
    public class EditModel : PageModel
    {
        private readonly IProductsData productsData;

        public EditModel(IProductsData productsData)
        {
            this.productsData = productsData;
        }

        [BindProperty]
        public Product Product { get; set; }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            productsData.Add(Product);
            productsData.Commit();
        }
    }
}