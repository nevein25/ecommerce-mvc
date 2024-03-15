using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services;
using OnlineShoppingApp.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace OnlineShoppingApp.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly IProductRepo _ProductRepo;

		public CartController(CartService cartService,IProductRepo productRepo)
        {
            _cartService = cartService;
            _ProductRepo = productRepo;
        }

        public IActionResult Index()
        {
            return View(_cartService.GetCartItems());
        }
        public IActionResult AddToCart(int id)
        {
            var prod = _ProductRepo.GetById(id);
            if(prod != null)
            {
                CartItemViewModel item = new CartItemViewModel()
                {
                    Id = id,
                    ProductName = prod.Name,
                    PictureUrl = prod.Images.Where(I => I.IsMain == 1).Select(I => I.Source).FirstOrDefault(),
                    Price = prod.Price,
                    Brand = prod.Brand.Name,
                    Category = prod.Category.Name,
                    Description = prod.Description
				};
                _cartService.AddToCart(item);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
