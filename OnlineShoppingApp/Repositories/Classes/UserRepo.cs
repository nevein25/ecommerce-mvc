using OnlineShoppingApp.Context;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class UserRepo : IUserRepo
    {
        ShoppingContext _context { get; }
        public UserRepo(ShoppingContext context)
        {
            _context = context;
        }

        public bool EmailExist(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
        public bool UsernameExist(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }
    }
}
