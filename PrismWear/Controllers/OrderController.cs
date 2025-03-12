using Microsoft.AspNetCore.Mvc;
using PrismWear.Services.Data;
using PrismWear.Web.ViewModels.Orders;
using System.Security.Claims;

namespace PrismWear.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public OrderController(IOrderService orderService,
            ICartService cartService)
        {
            _orderService = orderService;
            this._cartService = cartService;
        }
        [HttpGet]
        public async Task<IActionResult> ProcessPayment()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve cart items:
            var cartItems = await _cartService.RetrieveUserCartAsync(userId);

            var model = new OrderInputViewModel
            {
                UserId = userId,
                CartItems = cartItems,
            };
            model.TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(OrderInputViewModel model)
        {
            model.CartItems = await _cartService.RetrieveUserCartAsync(model.UserId);
            model.TotalAmount = model.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var orderId = await _orderService.CreateOrderAsync(model.UserId, model);

            return RedirectToAction("Details", new { orderId });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            var orders = await _orderService.GetAllOrdersForUserAsync(userId);

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var model = new EditOrderViewModel
            {
                OrderId = order.Id,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                ShippingAddressLine1 = order.ShippingAddressLine1,
                ShippingCity = order.ShippingCity,
                ShippingState = order.ShippingState,
                ShippingZipCode = order.ShippingZipCode,
                ShippingCountry = order.ShippingCountry,
                OrderStatus = order.OrderStatus 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isAdmin = User.IsInRole("Admin");

            bool updated = await _orderService.UpdateOrderAsync(model, isAdmin);
            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { orderId = model.OrderId });
        }
    }
}
