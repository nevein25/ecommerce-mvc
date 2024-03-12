using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Services.Interfaces
{
    public interface ICartService
    {
        public void AddToCart(string userId, CartItemViewModel newItem);
        public List<CartItemViewModel> GetCartItems(string userId);
        public void SaveCartItems(string userId, List<CartItemViewModel> cartItems);

    }
}
