using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Models
{
    public class Seller : AppUser
    {
        public string? Description { get; set; }
       

        public string? BusinessName { get; set; }
        public bool IsVerified { get; set; } = false;


    }
}
