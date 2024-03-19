using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services;
using OnlineShoppingApp.Services.Interfaces;
using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Controllers
{
    public class OrderController : Controller
    {
        private HttpContextAccessor httpContextAccessor;
        private readonly IOrderRepo _orderRepo;
        private readonly ICartService _cartService ;
        private readonly IProductRepo _productRepo;
        private readonly IDeliveryMethodsRepo _deliveryMethodsRepo;
        private readonly IAddressRepo _addressRepo;
        private readonly IOrderItemRepo _orderItemRepo;

        public OrderController(IOrderRepo orderRepo, ICartService cartService,IProductRepo productRepo,IDeliveryMethodsRepo deliveryMethodsRepo,IAddressRepo addressRepo,IOrderItemRepo orderItemRepo)
        {
            _orderRepo = orderRepo;
            _cartService = cartService;
            _productRepo = productRepo;
            _deliveryMethodsRepo = deliveryMethodsRepo;
            _addressRepo = addressRepo;
            _orderItemRepo = orderItemRepo;
        }
        public IActionResult Create(string news, DataForOrder orderData)
        {
            if (news == null)
            {
                Console.WriteLine("Alert News is Null");
            }
            else
            {
                Order order = new Order();
                order.BuyerId = (int) orderData.BuyerId;
                order.DeliveryMethod = _deliveryMethodsRepo.GetbyId(orderData.DeliveryMethodId);
                order.ShippingAddress = _addressRepo.GetAddressById((int)orderData.AddressId);
                order.SubTotal = orderData.SubTotal;
                //order.PaymentIntentId
                _orderRepo.CreateOrder(order);
                var orderId = _orderRepo.GetLastOrder().Id;
                var CartItems = _cartService.GetCartItems();

                foreach (var cartItem in CartItems)
                {

                    OrderItem orderItem = new OrderItem()
                    {
                        ProductId = cartItem.Id,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price,
                        Product = _productRepo.GetById(cartItem.Id),
                        OrderId = orderId
                    };
                    _orderItemRepo.Insert(orderItem);
                    var gettedOrderItem = _orderItemRepo.GetLast();
                    order.OrderItems.Add(gettedOrderItem);
                }
            }
            return View();
        }
    }
}
