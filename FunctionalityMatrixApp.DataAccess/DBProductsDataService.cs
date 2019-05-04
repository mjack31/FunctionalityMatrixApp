using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalityMatrixApp.DataAccess
{
    public class DBProductsDataService : IProductsData
    {
        private readonly ProductsDbContext productsDbContext;

        public DBProductsDataService(ProductsDbContext productsDbContext)
        {
            this.productsDbContext = productsDbContext;
        }

        public Product Add(Product product)
        {
            productsDbContext.Products.Add(product);
            return product;
        }

        public int Commit()
        {
            return productsDbContext.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            return productsDbContext.Products.Include("Pictures").Select(p => p);
        }

        public Product GetById(int id)
        {
            return productsDbContext.Products.Include("Pictures").Include("Attachments").Include("Parent").FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetChilds(int parentId)
        {
            return productsDbContext.Products.Where(p => p.ParentId == parentId);
        }

        public Product Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Product Update(Product id)
        {
            throw new NotImplementedException();
        }
    }
}
