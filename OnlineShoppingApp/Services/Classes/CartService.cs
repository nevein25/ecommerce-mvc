using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnlineShoppingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShoppingApp.Services
{
    public class CartService
    {
        private const string CartKey = "UserCart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(CartItemViewModel newItem)
        {
            // Retrieve existing cart items from cookies
            var existingCart = GetCartItems();

            // Add the new item to the cart
            existingCart.Add(newItem);

            // Save the updated cart items to cookies
            SaveCartItems(existingCart);
        }

        public List<CartItemViewModel> GetCartItems()
        {
            var context = _httpContextAccessor.HttpContext;
            var cartJson = context.Request.Cookies[CartKey];
            return cartJson != null ? JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartJson) : new List<CartItemViewModel>();
        }

        public void SaveCartItems(List<CartItemViewModel> cartItems)
        {
            var context = _httpContextAccessor.HttpContext;
            var cartJson = JsonConvert.SerializeObject(cartItems);
            context.Response.Cookies.Append(CartKey, cartJson, new CookieOptions
            {
                Expires = DateTime.Now.AddMonths(1), // Cookie expiration time
                HttpOnly = true // Make the cookie accessible only through HTTP requests
            });
        }
    }
}
