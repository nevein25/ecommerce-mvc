using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IOrderRepo
    {
        public void CreateOrder(Order order);
        public Order GetLastOrder();
    }
}
