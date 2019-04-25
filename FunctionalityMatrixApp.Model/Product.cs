using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunctionalityMatrixApp.Model
{
    public class Product
    {
        public Product()
        {
            Childs = new List<Product>();
            Pictures = new List<string>();
            Attachments = new List<string>();
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public Product Parent { get; set; }

        public List<Product> Childs { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Producer { get; set; }

        public List<string> Pictures { get; set; }

        public bool IsAllcomp { get; set; }

        public bool IsInOffer { get; set; }

        public bool IsInDevelopment { get; set; }

        public bool InFurniture { get; set; }

        public bool InAutomotive { get; set; }

        public bool InFashion { get; set; }

        [Required]
        public string Content { get; set; }

        public List<string> Attachments { get; set; }
    }
}
