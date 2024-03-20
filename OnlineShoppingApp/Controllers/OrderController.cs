using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services.Classes;
using OnlineShoppingApp.Services.Interfaces;
using OnlineShoppingApp.ViewModels;
using Stripe;

namespace OnlineShoppingApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICartService _cartService;
        private readonly IProductRepo _productRepo;
        private readonly IDeliveryMethodsRepo _deliveryMethodsRepo;
        private readonly IAddressRepo _addressRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IPaymentService _paymentService;
        const string endpointSecret = "whsec_f36dedfbb8ce6494b93a97361ae106438f871af81497e9ea6c6db2a08344712a";
        public OrderController(IOrderRepo orderRepo, 
            ICartService cartService, 
            IProductRepo productRepo, 
            IDeliveryMethodsRepo deliveryMethodsRepo, 
            IAddressRepo addressRepo, 
            IOrderItemRepo orderItemRepo, 
            IPaymentService paymentService)
        {
            _orderRepo = orderRepo;
            _cartService = cartService;
            _productRepo = productRepo;
            _deliveryMethodsRepo = deliveryMethodsRepo;
            _addressRepo = addressRepo;
            _orderItemRepo = orderItemRepo;
            _paymentService = paymentService;
        }

        public IActionResult Create(string news, DataForOrder orderData)
        {
            Order order = new Order();
            if (news == null)
            {
                Console.WriteLine("Alert News is Null");
            }
            else
            {
                //BuyerCartViewModel buyerCartViewModel = new BuyerCartViewModel();
                order.BuyerId = (int)orderData.BuyerId;
                order.DeliveryMethod = _deliveryMethodsRepo.GetbyId(orderData.DeliveryMethodId);
                order.ShippingAddress = _addressRepo.GetAddressById((int)orderData.AddressId);
                order.SubTotal = orderData.SubTotal;
                //order.PaymentIntentId
                _orderRepo.CreateOrder(order);
                var orderId = _orderRepo.GetLastOrder().Id;
                var CartItems = _cartService.GetCartItems();
                //buyerCartViewModel.Id = orderId;
                //buyerCartViewModel.BuyerId = (int) orderData.BuyerId;
                //buyerCartViewModel.DeliveryMethodId = orderData.DeliveryMethodId;
                //buyerCartViewModel.ShippingPrice = orderData.ShippingPrice;
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
                _paymentService.CreateOrUpdatePaymentIntent(orderId);
                //buyerCartViewModel.Items = order.OrderItems;
            }
            return View(order);
        }

        public ActionResult PaymentProcess(string nameOnCard, string cardNumber, string cardExpiry, int cardCvc)
        {
            //var order = _orderRepo.GetOrderById(orderId);
            if ((nameOnCard != null || nameOnCard != string.Empty)
                && (cardNumber != null || cardNumber != string.Empty)
                && (cardExpiry != null || cardExpiry != string.Empty)
                && cardCvc != 0)
            {

                return RedirectToAction("Webhook");
            }
            return new EmptyResult();
        }
        //[HttpPost]
        public IActionResult Webhook()
        {
            var json =  new StreamReader(HttpContext.Request.Body).ReadToEnd();
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                Order order;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    order =_paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                    return View("PaymentSucceeded");
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                   order = _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return View("PaymentFailed");
        }
    }
}
// Update order status to PaymentReceived if payment is successful
//if (order != null)
//{
//    _paymentService.UpdateOrderStatusToPaymentReceived(orderId);
//}

// Redirect or return appropriate response
