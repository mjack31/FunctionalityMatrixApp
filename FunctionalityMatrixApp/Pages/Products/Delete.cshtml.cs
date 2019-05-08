using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace FunctionalityMatrixApp.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductsData productsData;

        public DeleteModel(IProductsData productsData)
        {
            this.productsData = productsData;
        }

        [BindProperty]
        public Product Product { get; set; }

        public IActionResult OnGet(int productId)
        {
            Product = productsData.GetById(productId);
            if(Product == null)
            {
                return RedirectToPage("NotFound");
            } else
            {
                return Page();
            }
        }

        public IActionResult OnPost(int productId)
        {
            var childs = productsData.GetChilds(productId);
            if (childs.Count() > 0)
            {
                foreach (var child in childs)
                {
                    child.ParentId = null;
                    child.Parent = null;
                }
            }

            productsData.Remove(productId);
            productsData.Commit();

            return RedirectToPage("./List");
        }
    }
}