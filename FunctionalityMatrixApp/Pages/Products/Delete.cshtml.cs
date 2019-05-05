using FunctionalityMatrixApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunctionalityMatrixApp.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductsData productsData;

        public DeleteModel(IProductsData productsData)
        {
            this.productsData = productsData;
        }

        public void OnGet(int productId)
        {
            productsData.Remove(productId);
            productsData.Commit();
        }
    }
}