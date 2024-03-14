using OnlineShoppingApp.Common;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]

        public string LastName { get; set; }

        [Required]

        public string Email { get; set; }

        [Required]

        public string Username { get; set; }


        [Required]
        public string Password { get; set; }


        public UserType UserType { get; set; } = UserType.Buyer;
        //// For buyers
        //public DateOnly? DOB { get; set; }

        //// For sellers
        //public string? Description { get; set; }

        //public string? BusinessName { get; set; }
    }
}
