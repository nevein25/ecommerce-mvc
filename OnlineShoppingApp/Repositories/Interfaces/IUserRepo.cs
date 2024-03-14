using OnlineShoppingApp.Repositories.Classes;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IUserRepo
    {
        public bool EmailExist(string email);
        public bool UsernameExist(string username);
    }
}
