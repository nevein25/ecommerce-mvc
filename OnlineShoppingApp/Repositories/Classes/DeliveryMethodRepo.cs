using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
	public class DeliveryMethodRepo : IDeliveryMethodsRepo
	{
		private readonly ShoppingContext _shoppingContext;

		public DeliveryMethodRepo(ShoppingContext shoppingContext)
        {
			_shoppingContext = shoppingContext;
		}

		public List<DeliveryMethod> GetAll()
		{
			return _shoppingContext.DeliveryMethods.ToList();
		}

		public DeliveryMethod GetbyId(int id)
		{
			return _shoppingContext.DeliveryMethods.Find(id);
		}
	}
}
