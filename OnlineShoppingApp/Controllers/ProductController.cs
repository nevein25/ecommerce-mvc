﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Controllers
{
    public class ProductController : Controller
    {
        IProductRepo ProductRepo;
        ICategoriesRepo categoriesRepo;
        IBrandRepo brandRepo;
        IRateRepo _rateRepo { get; }
        static int ProductIdAjax = 0;

        public ProductController(IProductRepo _productRepo,ICategoriesRepo _categoriesRepo, IBrandRepo _brandRepo, IRateRepo rateRepo)
        {
            ProductRepo = _productRepo;
            categoriesRepo = _categoriesRepo;
            brandRepo = _brandRepo;
            _rateRepo = rateRepo;


        }

        public IActionResult GetAllProducts()
        {
            ViewBag.category = categoriesRepo.GetAll();
            ViewBag.brand = brandRepo.GetAll();
            return View(ProductRepo.GetAll());
        }

        public IActionResult GetProduct(int id)
        {
            ProductIdAjax = id;
            ViewBag.AvgRating= _rateRepo.GetAvgRateForProduct(id);
            return View(ProductRepo.GetById(id));
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
            if (ModelState.IsValid)
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

        [HttpPost]
        public IActionResult RateProduct(int Id, int NumOfStars)
        {

            if (!_rateRepo.ProductExist(Id)) return View("NotFound");
            _rateRepo.Rate(Id, 2, NumOfStars);
            return PartialView("_RatingPartialView", ProductRepo.GetById(Id));
        }

        [HttpGet]
        public IActionResult GetProductRating(int Id)
        {
            int rating = _rateRepo.GetRateForUser(ProductIdAjax, 2);
            return Json(new { rating = rating });
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