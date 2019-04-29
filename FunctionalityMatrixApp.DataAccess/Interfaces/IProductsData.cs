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
        Product Update(Product id);
        Product Remove(int id);
        int Commit();
    }
}
