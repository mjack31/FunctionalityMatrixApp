using FunctionalityMatrixApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionalityMatrixApp.Wrappers
{
    public class ProductModelWrapper
    {
        public ProductModelWrapper(Product Product)
        {
            this.Product = Product;
        }

        public Product Product { get; }

        public string DefaultPictureURL { get; set; }
        public string ShortenedContent { get; set; }
    }
}
