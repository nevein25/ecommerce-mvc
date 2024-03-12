using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Services;
using OnlineShoppingApp.ViewModels;
using System.Collections.Generic;

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
            // Check if the cart is empty, then add dummy cart items
            var cartItems = _cartService.GetCartItems();
            if (cartItems.Count == 0)
            {
                // Dummy data
                var dummyCartItems = new List<CartItemViewModel>
                {
                    new CartItemViewModel { Id = 1, ProductName = "Product 1", Price = 10.0m },
                    new CartItemViewModel { Id = 2, ProductName = "Product 2", Price = 15.0m }
                };

                // Add dummy cart items to the cart
                foreach (var item in dummyCartItems)
                {
                    _cartService.AddToCart(item);
                }
            }

            // Get cart items from the service
            cartItems = _cartService.GetCartItems();

            return View(cartItems);
        }
    }
}
