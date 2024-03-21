using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineShoppingApp.Common;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services.Classes;
using OnlineShoppingApp.Services.Interfaces;
using OnlineShoppingApp.ViewModels;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using System.Net.WebSockets;
using static System.Net.WebRequestMethods;
using Order = OnlineShoppingApp.Models.Order;

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
       // const string endpointSecret = "whsec_f36dedfbb8ce6494b93a97361ae106438f871af81497e9ea6c6db2a08344712a";
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




                var domain = "https://localhost:7289/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"Order/Confirmation",
                    CancelUrl = domain + $"Order/Cancel",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    

                };




                foreach (var cartItem in CartItems)
                {
                    var sessionListItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(cartItem.Price * cartItem.Quantity*100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = cartItem.ProductName.ToString(),
                            }
                        },
                        Quantity = cartItem.Quantity,
                    };

                    options.LineItems.Add(sessionListItem);

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

                var service = new SessionService();
                Session session=service.Create(options);
                Response.Headers.Add("Location", session.Url);

                //OrderConfirmationDataViewModel orderConfirmationData = new OrderConfirmationDataViewModel
                //{
                //    OrderId = order.Id,
                //    SessionId = session.Id

                //};

                TempData["SessionId"] = session.Id;
                TempData["OrderId"] = order.Id;

                //  _paymentService.CreateOrUpdatePaymentIntent(orderId);
                //buyerCartViewModel.Items = order.OrderItems;



            }
            return new StatusCodeResult(303);
        }


        public IActionResult Confirmation()
        {
            if (!TempData.ContainsKey("SessionId") || !TempData.ContainsKey("OrderId"))
            {
                // Handle case where TempData keys are missing
                return BadRequest("Invalid request");
            }

            var sessionId = TempData.Peek("SessionId").ToString();
            var orderId = Convert.ToInt32(TempData.Peek("OrderId"));

            var service = new SessionService();
            Session session = service.Get(sessionId);

            if (session.PaymentStatus == "paid")
            {
                var order = _orderRepo.GetOrderById(orderId);
                if (order != null)
                {
                    order.Status = OrderStatus.PaymentReceived;
                    _orderRepo.UpdateOrder(orderId, order);
                    _cartService.DeleteCart();

                    return Content("Payment Done");
                }
                else
                {
                    // Handle case where order is not found
                    return NotFound();
                }
            }
            else
            {
                var order = _orderRepo.GetOrderById(orderId);
                order.Status = OrderStatus.PaymentFailed;
                _orderRepo.UpdateOrder(orderId, order);
               
                return Content("Payment Fail");
            }
        }





        //public ActionResult PaymentProcess(string nameOnCard, string cardNumber, string cardExpiry, int cardCvc)
        //{
        //    //var order = _orderRepo.GetOrderById(orderId);
        //    if ((nameOnCard != null || nameOnCard != string.Empty)
        //        && (cardNumber != null || cardNumber != string.Empty)
        //        && (cardExpiry != null || cardExpiry != string.Empty)
        //        && cardCvc != 0)
        //    {

        //        return RedirectToAction("Webhook");
        //    }
        //    return new EmptyResult();
        //}
        ////[HttpPost]
        //public IActionResult Webhook()
        //{
        //    var json =  new StreamReader(HttpContext.Request.Body).ReadToEnd();
        //        var stripeEvent = EventUtility.ConstructEvent(json,
        //            Request.Headers["Stripe-Signature"], endpointSecret);
        //        var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
        //        Order order;
        //        // Handle the event
        //        if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
        //        {
        //            order =_paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
        //            return View("PaymentSucceeded");
        //        }
        //        else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        //        {
        //           order = _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
        //        }
        //        // ... handle other event types
        //        else
        //        {
        //            Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
        //        }

        //        return View("PaymentFailed");
        //}















    }
}
// Update order status to PaymentReceived if payment is successful
//if (order != null)
//{
//    _paymentService.UpdateOrderStatusToPaymentReceived(orderId);
//}

// Redirect or return appropriate response
