using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
	public interface IDeliveryMethodsRepo
	{
		public List<DeliveryMethod> GetAll();
		public DeliveryMethod GetbyId(int id);
	}
}
