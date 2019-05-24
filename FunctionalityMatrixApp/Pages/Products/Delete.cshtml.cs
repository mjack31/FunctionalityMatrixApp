using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using FunctionalityMatrixApp.Services.ServerFilesManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace FunctionalityMatrixApp.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductsData productsData;
        private readonly IServerFilesManager serverFilesManager;
        private readonly IConfiguration configuration;

        private string picturesPath;
        private string attachmentsPath;

        public DeleteModel(IProductsData productsData, IServerFilesManager serverFilesManager, IConfiguration configuration)
        {
            this.productsData = productsData;
            this.serverFilesManager = serverFilesManager;
            this.configuration = configuration;
        }

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
            picturesPath = serverFilesManager.GetEnvFullPath(configuration.GetValue<string>("UploadPaths:Pictures"));
            attachmentsPath = serverFilesManager.GetEnvFullPath(configuration.GetValue<string>("UploadPaths:Attachments"));

            Product = productsData.GetById(productId);
            var childs = productsData.GetChilds(productId);

            if (childs.Count() > 0)
            {
                foreach (var child in childs)
                {
                    child.ParentId = null;
                    child.Parent = null;
                }
            }

            var picturesIdsToDelete = Product.Pictures.Select(p => p.Id).ToList();
            var attachmentsIdsToDelete = Product.Attachments.Select(a => a.Id).ToList();

            var deletedPictures = productsData.RemovePictures(picturesIdsToDelete).Select(p => p.Name);
            var deletedAttachments = productsData.RemoveAttachments(attachmentsIdsToDelete).Select(p => p.Name);
            serverFilesManager.DeleteFilesFromServer(deletedPictures, picturesPath);
            serverFilesManager.DeleteFilesFromServer(deletedAttachments, attachmentsPath);

            productsData.Remove(productId);
            productsData.Commit();

            return RedirectToPage("./List");
        }
    }
}