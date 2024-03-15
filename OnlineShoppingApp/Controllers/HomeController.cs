using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using System.Diagnostics;

namespace OnlineShoppingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		IProductRepo ProductRepo;
		ICategoriesRepo categoriesRepo;
		IBrandRepo brandRepo;

		public HomeController(ILogger<HomeController> logger, IProductRepo _productRepo, ICategoriesRepo _categoriesRepo, IBrandRepo _brandRepo)
        {
            _logger = logger;
			ProductRepo = _productRepo;
			categoriesRepo = _categoriesRepo;
			brandRepo = _brandRepo;
		}

        public IActionResult Index()
        {
			ViewBag.category = categoriesRepo.GetAll();
			ViewBag.brand = brandRepo.GetAll();
			return View(ProductRepo.GetAll());
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
