using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using System.Runtime.InteropServices;

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

            return Context.Products.Include(p=>p.Category).Include(p=>p.Brand).Include(p=>p.Images).ToList();

        }

        public Product GetById(int id)
        {
            return Context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p=>p.Images).FirstOrDefault(p => p.Id == id);
        }

        public void Edit(int id, Product newProduct)
        {
            // Retrieve the existing product by its id
            Product oldProd = GetById(id);

            // If the product exists
            if (oldProd != null)
            {
                // Update the properties of the existing product with the new values
                oldProd.categoryId = newProduct.categoryId;
                oldProd.brandId = newProduct.brandId;
                oldProd.Description = newProduct.Description;
                oldProd.Price = newProduct.Price;
                oldProd.Name = newProduct.Name;

                if (newProduct.Images != null && newProduct.Images.Any())
                {
                    foreach (var img in oldProd.Images.ToList()) 
                    {
                        Context.Images.Remove(img);
                    }      
                    // Add new images to the product
                    foreach (var img in newProduct.Images)
                    {
                        var image = new Images { Source = img.Source, IsMain = img.IsMain, ProductId = oldProd.Id };
                    //  oldProd.Images.Add(new Images { Source = img.Source, IsMain = img.IsMain, ProductId = oldProd.Id });
                        Context.Images.Add(img);
                    } 
                }

                    Context.SaveChanges();
            }
        }





        public void Insert(Product product)
        {
            if (product != null)
            {     // Add the product to the context
                Context.Products.Add(product);
                // Save changes to generate product ID
                Context.SaveChanges();
                // Ensure there are image URLs
                if (product.ImageUrl != null && product.ImageUrl.Any())
                {
                    bool isFirstImage = true;
                    foreach (var imageUrl in product.ImageUrl)
                    {
                        // Check if the image URL already exists in the database
                        var existingImage = Context.Images.FirstOrDefault(i => i.Source == imageUrl && i.ProductId==product.Id);
                        // If the image URL doesn't exist, add it to the context
                        if (existingImage == null)
                        {
                            var image = new Images { Source = imageUrl, IsMain = 1, ProductId = product.Id };
                            // Set IsMain based on isFirstImage flag
                            image.IsMain = isFirstImage ? 1 : 0;
                            isFirstImage = false;

                            Context.Images.Add(image);
                        }
                    }                   
                    // Save changes to insert images with product ID
                    Context.SaveChanges();


                }
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
