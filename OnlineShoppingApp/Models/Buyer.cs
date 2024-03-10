using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Models
{
    public class Buyer : AppUser
    {
        [DataType(DataType.Date)]
        public DateOnly? DOB { get; set; }
    }
}
