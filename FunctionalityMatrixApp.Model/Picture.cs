using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FunctionalityMatrixApp.Model
{
    public class Picture
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
