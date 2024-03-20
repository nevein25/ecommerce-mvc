using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IProductRepo
    {
        public List<Product> GetAll();

        public Product GetById(int id);

        public List<Product> GetByName(string Name);
        public List<Product> GetProductsStartingWith(string search);

		//public void Edit(int id,Product newProduct);

        public void Insert(Product product,int userId, List<IFormFile> ImageUrl);
        public void Delete(Product product, int userId);

        public List<Product> GetProductsPerSeller(int sellerId);

        public List<Product> GetBestSellingProducts();

    }
}
