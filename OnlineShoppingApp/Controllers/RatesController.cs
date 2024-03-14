using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Extentions;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class RatesController : Controller
    {
        IRateRepo _rateRepo { get; }
        public RatesController(IRateRepo rateRepo)
        {
            _rateRepo = rateRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RateBook(int productId, RateProductViewModel rateProductViewModel)
        {

            if (!_rateRepo.ProductExist(productId)) return View("NotFound");
            _rateRepo.Rate(productId, User.GetUserId(), rateProductViewModel.NumOfStars);
            return View();
        }
    }
}
