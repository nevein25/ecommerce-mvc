using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // Foreign key property
        [ForeignKey(nameof(Category))]
        public int categoryId { get; set; }

        public virtual Category Category { get; set; }


        // Foreign key property
        [ForeignKey(nameof(Brand))]
        public int brandId { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<Images> Images { get; set; }
       
    }
}
