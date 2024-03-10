using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
       
        public string? Description { get; set; }
        public decimal Price { get; set; }

    }
}
