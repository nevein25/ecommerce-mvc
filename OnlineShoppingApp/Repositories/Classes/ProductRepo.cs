using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using OnlineShoppingApp.Extentions;
using System;


namespace OnlineShoppingApp.Repositories.Classes
{
    public class ProductRepo : IProductRepo
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ShoppingContext Context { get; }
        public ProductRepo(ShoppingContext _Context, IWebHostEnvironment hostingEnvironment)
        {
            Context = _Context;
            _hostingEnvironment = hostingEnvironment;
        }
        public List<Product> GetAll()
        {

            return Context.Products.Include(p=>p.Category).Include(p=>p.Brand).Include(p=>p.Images).Include(p => p.Comments).Include(p=>p.Rates).ToList();

        }

        public Product GetById(int id)
        {
            return Context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p=>p.Images).Include(p => p.Rates).Include(p=>p.Comments).FirstOrDefault(p => p.Id == id);
        }


        //public void Edit(int id, Product newProduct)
        //{
        //    // Retrieve the existing product by its id including related images
        //    Product oldProd = GetById(id);

        //    // If the product exists
        //    if (oldProd != null)
        //    {
        //        // Update the properties of the existing product with the new values
        //        oldProd.Name = newProduct.Name;
        //        oldProd.Description = newProduct.Description;
        //        oldProd.Price = newProduct.Price;
        //        oldProd.categoryId = newProduct.categoryId;
        //        oldProd.brandId = newProduct.brandId;

        //        // Update the list of image URLs
        //        if (newProduct.ImageUrl != null && newProduct.ImageUrl.Any())
        //        {
        //            foreach (var imageUrl in newProduct.ImageUrl)
        //            {
        //                // Check if the URL already exists in the database for this product
        //                var existingImage = oldProd.Images.FirstOrDefault(img => img.Source == imageUrl);
        //                if (existingImage == null)
        //                {
        //                    // Add new images to the product
        //                    oldProd.Images.Add(new Images { Source = imageUrl, ProductId = oldProd.Id });
        //                }
        //            }

        //            // Remove images that are no longer present in the submitted list
        //            foreach (var image in oldProd.Images.ToList())
        //            {
        //                if (!newProduct.ImageUrl.Contains(image.Source))
        //                {
        //                    // Check if the image to be removed is the main image
        //                    if (image.IsMain == 1)
        //                    {
        //                        // Find the next image after the one to be removed
        //                        var nextImage = oldProd.Images
        //                            .Where(img => img.Id != image.Id) // Exclude the image to be removed
        //                            .FirstOrDefault();



        //                        // If there's a next image, set it as the main image
        //                        if (nextImage != null)
        //                        {
        //                            nextImage.IsMain = 1;
        //                        }
        //                        //else
        //                        //{
        //                        //    // If there's no next image, set the first image of the updated product as the main image
        //                        //    var firstImage = oldProd.Images.FirstOrDefault();
        //                        //    if (firstImage != null)
        //                        //    {
        //                        //        firstImage.IsMain = 1;
        //                        //    }
        //                        //}
        //                    }

        //                    // Remove the image from the product
        //                    Context.Images.Remove(oldProd.Images.FirstOrDefault(i=>i.Source==image.Source&& i.ProductId==oldProd.Id));
        //                    oldProd.Images.Remove(image);
        //                }
        //            }


        //        }

        //        Context.SaveChanges();
        //    }
        //}





        public void Insert(Product product, int userId, List<IFormFile> ImageUrl)
        {
            if (product != null)
            {     // Add the product to the context
                Context.Products.Add(product);

                // Save changes to generate product ID
                Context.SaveChanges();

                // Create a new instance of the junction entity (ProductSeller)
                var productSeller = new ProductSeller
                {
                    ProductId = product.Id,
                    SellerId = userId,
                };


                Context.ProductSellers.Add(productSeller);

                // Ensure there are image URLs
               // if (product.ImageUrl != null && product.ImageUrl.Any())
                {
                    bool isFirstImage = true;
                    //foreach (var imageUrl in product.ImageUrl)
                    //{
                    //    // Check if the image URL already exists in the database
                    //    var existingImage = Context.Images.FirstOrDefault(i => i.Source == imageUrl && i.ProductId == product.Id);
                    //    // If the image URL doesn't exist, add it to the context
                    //    if (existingImage == null)
                    //    {
                    //        var image = new Images { Source = imageUrl, IsMain = 1, ProductId = product.Id };
                    //        // Set IsMain based on isFirstImage flag
                    //        image.IsMain = isFirstImage ? 1 : 0;
                    //        isFirstImage = false;

                    //        Context.Images.Add(image);
                    //    }
                    //}

                    foreach (var file in ImageUrl)
                    {
                        if (file != null && file.Length > 0)
                        {             // Determine a unique file name

                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            // Define the path where the file will be saved

                            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);
                            // Save the file to the server

                            using (var stream = new FileStream(filePath, FileMode.Create))     
                                {   
                                file.CopyTo(stream);        
                                }    
                            // Save the image path to the database
                            var image = new Images { Source = "/images/" + fileName, ProductId = product.Id };   
                             Context.Images.Add(image);     
                        }  
                    }
                                      // Save changes to insert images with product ID
                            Context.SaveChanges();


                }
            }
        }

        public void Delete(Product product , int userId)
        {
            //Product oldProd = GetById(id);
            var prodSeller = new ProductSeller
            {
                ProductId= product.Id,
                SellerId= userId,
            };
            Context.ProductSellers.Remove(prodSeller);
            Context.Products.Remove(product);
            Context.SaveChanges();
        }

		public List<Product> GetByName(string Name)
		{
			return Context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Rates).Include(p=>p.Comments)
                .Include(p => p.Images).Where(p => p.Name.StartsWith(Name) ||
                p.Category.Name.StartsWith(Name) ||  p.Brand.Name.StartsWith(Name) ||  
                  p.Description.Contains(Name) ).ToList();

        }

        


        public List<Product> GetProductsStartingWith(string search)
		{
            return GetByName(search);
		}

        public List<Product> GetProductsPerSeller(int sellerId)
        {
            var prodIDS = Context.ProductSellers.Where(ps => ps.SellerId == sellerId).Select(ps => ps.ProductId).ToList();

            var products = Context.Products.Include(p=>p.Category).Include(p=>p.Brand).Include(p=>p.Images).Include(p => p.Rates).Include(p => p.Comments).Where(p => prodIDS.Contains(p.Id)).ToList();

            return products;
        }



        public List<Product> GetBestSellingProducts() { 

            var bestSellersQuery = Context.OrderItems.GroupBy(o => o.ProductId)
                .Select(g => new { ProductId = g.Key, TotalQuantitySold = g.Sum(o => o.Quantity) })
                .Where(p => p.TotalQuantitySold >= 5);


            var bestSellerProducts = Context.Products.Include(p=>p.Category).Include(p => p.Brand)
                                    .Include(p => p.Images).Include(p => p.Rates).Include(p => p.Comments)
                                    .Where(p => bestSellersQuery.Select(q => q.ProductId).Contains(p.Id)).ToList(); 
            
            return bestSellerProducts; 
        
        }
    }
}
