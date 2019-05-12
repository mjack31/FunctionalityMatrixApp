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
            return productsDbContext.Products.Include("Pictures").AsNoTracking().Select(p => p);
        }

        public Product GetById(int id)
        {
            return productsDbContext.Products.Include("Pictures").Include("Attachments").Include("Parent").AsNoTracking().FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetChilds(int parentId)
        {
            return productsDbContext.Products.Where(p => p.ParentId == parentId);
        }

        public IEnumerable<string> GetProductAttachmentsURLs(int productId, string path)
        {
            var product = productsDbContext.Products.Include("Attachments").FirstOrDefault(p => p.Id == productId);
            foreach (var attachment in product.Attachments)
            {
                yield return path + attachment.Name;
            }
        }

        public IEnumerable<string> GetProductPicturesURLs(int productId, string path)
        {
            var product = productsDbContext.Products.Include("Pictures").FirstOrDefault(p => p.Id == productId);
            foreach (var picture in product.Pictures)
            {
                yield return path + picture.Name;
            }
        }

        public Product Remove(int id)
        {
            var productToDelete = productsDbContext.Products.FirstOrDefault(p => p.Id == id);
            if (productToDelete != null)
            {
                productsDbContext.Products.Remove(productToDelete);
                return productToDelete;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Attachment> RemoveAttachments(List<int> attachmentsIdsToDelete)
        {
            foreach (var id in attachmentsIdsToDelete)
            {
                var attachmentToDelete = productsDbContext.Attachments.FirstOrDefault(a => a.Id == id);
                productsDbContext.Attachments.Remove(attachmentToDelete);
                yield return attachmentToDelete;
            }
            Commit();
        }

        public IEnumerable<Picture> RemovePictures(List<int> picturesIdsToDelete)
        {
            foreach (var id in picturesIdsToDelete)
            {
                var pictureToDelete = productsDbContext.Pictures.FirstOrDefault(a => a.Id == id);
                productsDbContext.Pictures.Remove(pictureToDelete);
                yield return pictureToDelete;
            }
            Commit();
        }

        public Product Update(Product productToUpdate)
        {
            var product = productsDbContext.Products.Attach(productToUpdate);
            product.State = EntityState.Modified;
            return productToUpdate;
        }
    }
}
