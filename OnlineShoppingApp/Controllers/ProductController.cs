using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Controllers
{
    public class ProductController : Controller
    {
        IProductRepo ProductRepo;
        ICategoriesRepo categoriesRepo;
        IBrandRepo brandRepo;
        public ProductController(IProductRepo _productRepo,ICategoriesRepo _categoriesRepo, IBrandRepo _brandRepo)
        {
            ProductRepo = _productRepo;
            categoriesRepo = _categoriesRepo;
            brandRepo = _brandRepo;
        }

        public IActionResult GetAllProducts()
        {
           
            return View(ProductRepo.GetAll());
        }

        public IActionResult GetProduct(int id)
        {

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
            if (product!=null)
            {
                if (product.brandId == int.Parse("-1") && !string.IsNullOrEmpty(otherBrand))
                {
                    Brand newBrand = new Brand { Name = otherBrand };
                    brandRepo.Insert(newBrand);
                    product.brandId = newBrand.Id;
                }
                ProductRepo.Insert(product);
                return RedirectToAction("GetAllProducts");
            }

            SelectList categories = new SelectList(categoriesRepo.GetAll(), "Id", "Name");
            SelectList Brands = new SelectList(brandRepo.GetAll(), "Id", "Name");
            ViewBag.category = categories;
            ViewBag.brand = Brands;
            return View(product);
        }
    }
}
