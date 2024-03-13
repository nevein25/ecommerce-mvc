using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IProductRepo
    {
        public List<Product> GetAll();

        public Product GetById(int id);

        public void Edit(int id,Product newProduct);

        public void Insert(Product product);
        public void Delete(int id);

    }
}
