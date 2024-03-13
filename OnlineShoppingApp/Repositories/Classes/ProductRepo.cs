using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class ProductRepo : IProductRepo
    {
        public ShoppingContext Context { get; }
        public ProductRepo(ShoppingContext _Context)
        {
            Context = _Context;
        }
        public List<Product> GetAll()
        {

            return Context.Products.Include(p=>p.Category).Include(p=>p.Brand).ToList();

        }

        public Product GetById(int id)
        {
            return Context.Products.Include(p => p.Category).Include(p => p.Brand).FirstOrDefault(p => p.Id == id);
        }

        public void Edit(int id, Product newProduct)
        {
            // Retrieve the existing product by its id
            Product oldProd = GetById(id);

            // If the product exists
            if (oldProd != null)
            {
                // Update the properties of the existing product with the new values
                oldProd.Category = newProduct.Category;
                oldProd.Brand = newProduct.Brand;
                oldProd.Description = newProduct.Description;
                oldProd.Price = newProduct.Price;
                oldProd.Name = newProduct.Name;

               
                foreach (var image in oldProd.Images)
                {
                    image.IsMain = 0;
                }

               
                if (newProduct.Images != null && newProduct.Images.Any())
                {
                   
                    foreach (var newImage in newProduct.Images)
                    {
                        if (newImage!=null)
                        {
                            var oldImage = oldProd.Images.FirstOrDefault(i => i.Source == newImage.Source);


                            if (oldImage != null)
                            {
                                oldImage.IsMain = 1;
                            }

                        }
                        
                        
                    }
                }

                
                Context.SaveChanges();
            }
        }


        public void Insert(Product product)
        {
            if(product != null)
            {
                Context.Products.Add(product);
                Context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Product oldProd = GetById(id);
            Context.Products.Remove(Context.Products.FirstOrDefault(p=>p.Id==id));
            Context.SaveChanges();
        }
    }
}
