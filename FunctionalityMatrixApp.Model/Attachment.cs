using System.ComponentModel.DataAnnotations;

namespace FunctionalityMatrixApp.Model
{
    public class Attachment
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
