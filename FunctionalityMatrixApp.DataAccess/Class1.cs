using FunctionalityMatrixApp.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace FunctionalityMatrixApp.DataAccess
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext()
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
