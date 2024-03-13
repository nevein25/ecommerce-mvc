using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Services;
using OnlineShoppingApp.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace OnlineShoppingApp.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            //var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // Check if the cart is empty, then add dummy cart items
            var cartItems = _cartService.GetCartItems();
            // Dummy data
            //var dummyCartItems = new List<CartItemViewModel>
            //{
            //    new CartItemViewModel { Id = 1, ProductName = "Product 1", Price = 10.0m },
            //    new CartItemViewModel { Id = 2, ProductName = "Product 2", Price = 15.0m }
            //};


            var item=new CartItemViewModel (){ Id = 2, ProductName = "Product 2", Price = 10.0m,Quantity=3 };


            _cartService.AddToCart(item);
            //_cartService.UpdateCart(item.Id, item);

            // Get cart items from the service
            cartItems = _cartService.GetCartItems();

            return View(cartItems);
        }
    }
}
