using FunctionalityMatrixApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionalityMatrixApp.DataAccess.Interfaces
{
    public interface IProductsData
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        Product Add(Product product);
        Product Update(Product productToUpdate);
        Product Remove(int id);
        int Commit();
        IEnumerable<Product> GetChilds(int parentId);
        IEnumerable<string> GetProductAttachmentsURLs(int productId, string path);
        IEnumerable<string> GetProductPicturesURLs(int productId, string path);
        IEnumerable<Picture> RemovePictures(List<int> picturesIdsToDelete);
        IEnumerable<Attachment> RemoveAttachments(List<int> attachmentsIdsToDelete);
        IEnumerable<Product> GetByName(string name);
        IEnumerable<Product> GetByName(string name, ProductType type);
    }
}
