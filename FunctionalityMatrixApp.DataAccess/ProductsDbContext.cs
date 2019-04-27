using FunctionalityMatrixApp.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace FunctionalityMatrixApp.DataAccess
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
    }
}
