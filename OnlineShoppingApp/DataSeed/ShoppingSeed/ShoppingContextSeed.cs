using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using System.Text.Json;

namespace OnlineShoppingApp.DataSeed.ShoppingSeed
{
    public class ShoppingContextSeed
    {
        public async static Task SeedAsync(ShoppingContext shoppingContext)
        {
            if (shoppingContext.Roles.Count() == 0)
            {
                var rolesData = File.ReadAllText("../OnlineShoppingApp/DataSeed/roles.json");
                var roles = JsonSerializer.Deserialize<List<AppRole>>(rolesData);

                if (roles?.Count() > 0)
                {
                    //categories = categories.Select(b => new ProductCategory()
                    //{
                    //    Name = b.Name
                    //}).ToList();
                    foreach (var role in roles)
                    {
                        shoppingContext.Set<AppRole>().Add(role);
                    }
                    await shoppingContext.SaveChangesAsync();
                }
            }
            if (shoppingContext.DeliveryMethods.Count() == 0)
            {
                var deliveryMethodsData = File.ReadAllText("../OnlineShoppingApp/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods?.Count() > 0)
                {
                    //categories = categories.Select(b => new ProductCategory()
                    //{
                    //    Name = b.Name
                    //}).ToList();
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        shoppingContext.Set<DeliveryMethod>().Add(deliveryMethod);
                    }
                    await shoppingContext.SaveChangesAsync();
                }
            }

            if (shoppingContext.Brands.Count() == 0)
            {
                var brandsData = File.ReadAllText("../OnlineShoppingApp/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<Brand>>(brandsData);

                if (brands?.Count() > 0)
                {
                    //brands = brands.Select(b => new ProductBrand()
                    //{
                    //    Name = b.Name
                    //}).ToList();
                    foreach (var brand in brands)
                    {
                        shoppingContext.Set<Brand>().Add(brand);
                    }
                    await shoppingContext.SaveChangesAsync();
                }
            }
            if (shoppingContext.Categories.Count() == 0)
            {
                var categoriesData = File.ReadAllText("../OnlineShoppingApp/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                if (categories?.Count() > 0)
                {
                    //categories = categories.Select(b => new ProductCategory()
                    //{
                    //    Name = b.Name
                    //}).ToList();
                    foreach (var category in categories)
                    {
                        shoppingContext.Set<Category>().Add(category);
                    }
                    await shoppingContext.SaveChangesAsync();
                }
            }
            if (shoppingContext.Products.Count() == 0)
            {
                var productsData = File.ReadAllText("../OnlineShoppingApp/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count() > 0)
                {
                    //categories = categories.Select(b => new ProductCategory()
                    //{
                    //    Name = b.Name
                    //}).ToList();
                    foreach (var product in products)
                    {
                        shoppingContext.Set<Product>().Add(product);
                    }
                    await shoppingContext.SaveChangesAsync();
                }
            }
            if(shoppingContext.Images.Count() == 0)
            {
                var imagesData = File.ReadAllText("../OnlineShoppingApp/DataSeed/images.json");
                var images = JsonSerializer.Deserialize<List<Images>>(imagesData);
                if(images?.Count() > 0)
                {
                    foreach(var image in images)
                    {
                        shoppingContext.Set<Images>().Add(image);
                    }
                    await shoppingContext.SaveChangesAsync();
                }
            }
        }
    }
}
