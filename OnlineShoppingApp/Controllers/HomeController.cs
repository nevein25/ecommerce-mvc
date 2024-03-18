using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Helpers;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;
using System.Diagnostics;

namespace OnlineShoppingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IProductRepo ProductRepo;
        ICategoriesRepo categoriesRepo;
        IBrandRepo brandRepo;
        ICommentsRepo commentsRepo;

        public HomeController(ILogger<HomeController> logger, IProductRepo _productRepo, ICategoriesRepo _categoriesRepo, IBrandRepo _brandRepo, ICommentsRepo commentsRepo)
        {
            _logger = logger;
            ProductRepo = _productRepo;
            categoriesRepo = _categoriesRepo;
            brandRepo = _brandRepo;
            this.commentsRepo = commentsRepo;
        }

        public IActionResult Index()
        {
            // UserHelper.LoggedinUserId
            var viewModel = new ProductViewModel
            {
                Categories = categoriesRepo.GetAll(),
                Brands = brandRepo.GetAll(),
                Products = ProductRepo.GetAll()
            };
            ViewBag.ProductCategories = categoriesRepo.GetAll();
            return View(viewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
