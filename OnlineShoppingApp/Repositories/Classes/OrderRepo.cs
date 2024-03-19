using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ShoppingContext _shoppingContext;

        public OrderRepo(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }
        public void CreateOrder(Order order)
        {
            if (order != null)
            {
                _shoppingContext.Orders.Add(order);
                _shoppingContext.SaveChanges();
            }
        }

        public Order GetLastOrder()
        {
            return _shoppingContext.Orders.OrderBy(o => o.Id).LastOrDefault();
        }
    }
}
