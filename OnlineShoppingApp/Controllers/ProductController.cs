using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnlineShoppingApp.Controllers
{
    public class ProductController : Controller
    {
        IProductRepo ProductRepo;
        ICategoriesRepo categoriesRepo;
        IBrandRepo brandRepo;

        IRateRepo _rateRepo { get; }
        static int ProductIdForJs = 0;
        ICommentsRepo commentsRepo;
        public ProductController(IProductRepo _productRepo,ICategoriesRepo _categoriesRepo, IBrandRepo _brandRepo, ICommentsRepo commentsRepo, IRateRepo rateRepo)
        {
            ProductRepo = _productRepo;
            categoriesRepo = _categoriesRepo;
            brandRepo = _brandRepo;
            this.commentsRepo = commentsRepo;
            _rateRepo = rateRepo;

        }


        //public IActionResult GetAllProductss()
        //{
        //    ViewBag.category = categoriesRepo.GetAll();
        //    ViewBag.brand = brandRepo.GetAll();
        //    return View("GetAllProducts", ProductRepo.GetAll());
        //}


        public IActionResult GetAllProducts()
		{
			var viewModel = new ProductViewModel
			{
				Categories = categoriesRepo.GetAll(),
				Brands = brandRepo.GetAll(),
				Products = ProductRepo.GetAll()
			};

			return View("~/Views/Home/Index.cshtml", viewModel);
		}




		public IActionResult GetProduct(int id)
        {

            var product = ProductRepo.GetById(id);
            if (product == null)
            {
                return NotFound("No Product is found");
            }
            var comments = commentsRepo.GetAllComments(id);
            //var productViewModel = new ProductViewModel
            //{
            //    Products = new List<Product> { product },
            //    Comments = comments
            //    // Populate other data in the view model as needed
            //};

            ProductIdForJs = id;
            ViewBag.AvgRating= _rateRepo.GetAvgRateForProduct(id);
            return View(product);

        }


     


        public IActionResult SearchProducts(string search,int? category)
		{
			var filteredProducts = ProductRepo.GetProductsStartingWith(search);

			if (category.HasValue && category.Value != 0) // Assuming 0 represents "All Categories"
			{
				filteredProducts = filteredProducts.Where(p => p.categoryId == category.Value).ToList();
			}
			var viewModel = new ProductViewModel
			{
				Products = filteredProducts,
                Categories= categoriesRepo.GetAll()
			};
			// Pass the filtered products to the view
			return View(viewModel);
		}



		public IActionResult InsertNewProduct()
        {
            SelectList categories= new SelectList( categoriesRepo.GetAll(),"Id","Name");
            SelectList Brands = new SelectList(brandRepo.GetAll(), "Id", "Name");
            ViewBag.category= categories;
            ViewBag.brand= Brands;
            return View();
        }


        [HttpPost]
        public IActionResult InsertNewProduct(Product product, string otherBrand)
        {
            if (product != null)
            {
                if (product.brandId == -1 && !string.IsNullOrEmpty(otherBrand))
                {
                    // Insert a new brand if the user selected "Other"
                    Brand newBrand = new Brand { Name = otherBrand };
                    brandRepo.Insert(newBrand);
                    product.brandId = newBrand.Id;
                }

                // Insert the product
                ProductRepo.Insert(product);

                return RedirectToAction("GetAllProducts");
            }

            // If the model state is not valid, return the view with errors
            SelectList categories = new SelectList(categoriesRepo.GetAll(), "Id", "Name");
            SelectList brands = new SelectList(brandRepo.GetAll(), "Id", "Name");
            ViewBag.Category = categories;
            ViewBag.Brand = brands;
            return View(product);
        }
		public IActionResult EditProduct(int id)
		{

            SelectList categories = new SelectList(categoriesRepo.GetAll(), "Id", "Name");
            SelectList brands = new SelectList(brandRepo.GetAll(), "Id", "Name");
            ViewBag.Category = categories;
            ViewBag.Brand = brands;
            var selectedProd = ProductRepo.GetById(id);
            ViewBag.URLs = selectedProd.Images
                .Where(i => i.ProductId == selectedProd.Id) 
                .Select(i => i.Source);
            return View(selectedProd);

		}

        [HttpPost]
        public IActionResult EditProduct(int id, Product product)
        {
            if (product!=null)
            {
                ProductRepo.Edit(id, product);
                return RedirectToAction("GetAllProducts");
            }

            // If the model state is not valid, reload the view with the existing product and the necessary data
            SelectList categories = new SelectList(categoriesRepo.GetAll(), "Id", "Name");
            SelectList brands = new SelectList(brandRepo.GetAll(), "Id", "Name");
            ViewBag.Category = categories;
            ViewBag.Brand = brands;
            var selectedProd = ProductRepo.GetById(id);
            ViewBag.URLs = selectedProd.Images
                .Where(i => i.ProductId == selectedProd.Id)
                .Select(i => i.Source);
            return View(product);
        }

      
        public IActionResult DeleteProduct(int id)
        {
            // Retrieve the product by its id
            Product prod = ProductRepo.GetById(id);

            // Check if the product exists
            if (prod != null)
            {
                // Delete the product from the repository
                ProductRepo.Delete(prod);

                // Redirect to the appropriate view (e.g., a list of all products)
               // return RedirectToAction("GetAllProducts");
               return NoContent();
            }

            // If the product does not exist, return a not found error or redirect to an error page
            return NotFound();
        }


        [HttpPost]
        public IActionResult InsertComment(string review, int prodID)
        {

            var comment = new Comment { Text = review, 
                                        Date = DateTime.Now, 
                                        AppUserId = User.GetUserId(), 
                                        Product = ProductRepo.GetById(prodID) };

            commentsRepo.AddComment(comment);
            return View("~/Views/Product/GetProduct.cshtml",
            new ProductViewModel
            {
                    Products = new List<Product> { ProductRepo.GetById(prodID) },
                    Comments =  commentsRepo.GetAllComments(prodID) ,
                    
            });
        }

  

        [HttpPost]
        public IActionResult RateProduct(int Id, int NumOfStars)
        {
            int userId = User.GetUserId();
            if (userId != null)
            {
                if (!_rateRepo.ProductExist(Id)) return View("NotFound");
                _rateRepo.Rate(Id, userId, NumOfStars);
            }
          
            return PartialView("_RatingPartialView", ProductRepo.GetById(Id));
        }

        [HttpGet]
        public IActionResult GetProductRating(int Id)
        {
            int userId = User.GetUserId();
            if (userId != null)
            {
                int rating = _rateRepo.GetRateForUser(ProductIdForJs, userId);
                return Json(new { rating = rating });
            }
            return Json(new {});
        }

        //      public IActionResult DeleteProduct(int id)
        //      {

        //          return View();
        //      }

        //[HttpPost]
        //public IActionResult DeleteProduct(int id)
        //{
        //	// Retrieve the product by its id
        //	var product = ProductRepo.GetById(id);

        //	// Check if the product exists
        //	if (product != null)
        //	{
        //		// Delete the product from the repository
        //		ProductRepo.Delete(product);

        //		// Redirect to the appropriate view (e.g., a list of all products)
        //		return RedirectToAction("GetAllProducts");
        //	}

        //	// If the product does not exist, return a not found error or redirect to an error page
        //	return NotFound();
        //}
    }

}