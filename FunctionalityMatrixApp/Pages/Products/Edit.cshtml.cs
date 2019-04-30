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

namespace FunctionalityMatrixApp.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductsData productsData;
        private readonly IHtmlHelper htmlHelper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EditModel(IProductsData productsData, IHtmlHelper htmlHelper, IWebHostEnvironment webHostEnvironment)
        {
            this.productsData = productsData;
            this.htmlHelper = htmlHelper;
            this.webHostEnvironment = webHostEnvironment;
            ProductTypes = htmlHelper.GetEnumSelectList<ProductType>();
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile[] Upload { get; set; }

        public IEnumerable<SelectListItem> ProductTypes { get; set; }
        public IEnumerable<SelectListItem> AvailableParents { get; set; }

        public void OnGet()
        {
            GetAvailableParentsAsSelectListItem();
        }

        public async Task OnPostAsync()
        {
            GetAvailableParentsAsSelectListItem();

            foreach (var uploadFile in Upload)
            {
                var file = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot/uploads", uploadFile.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fileStream);
                }
            }


            productsData.Add(Product);
            productsData.Commit();
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