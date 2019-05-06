using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunctionalityMatrixApp.DataAccess.Interfaces;
using FunctionalityMatrixApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunctionalityMatrixApp.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsData productsData;

        public ProductsController(IProductsData productsData)
        {
            this.productsData = productsData;
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return productsData.GetAll();
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public Product Get(int id)
        {
            return productsData.GetById(id);
        }
    }
}
