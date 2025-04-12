using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Services.Data;
using PrismWear.Web.ViewModels.Orders;
using Stripe.Climate;
using System.Security.Claims;

namespace PrismWear.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IDeletableEntityRepository<ProductDetail> _productDetailRepository;

        public OrderController(IOrderService orderService,
            ICartService cartService,
            IDeletableEntityRepository<ProductDetail> productDetailRepository)
        {
            _orderService = orderService;
            this._cartService = cartService;
            this._productDetailRepository = productDetailRepository;
        }


        [HttpGet]
        public async Task<IActionResult> ProcessPayment()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
            var cartItems = await _cartService.RetrieveUserCartAsync(model.UserId);
            model.CartItems = cartItems;
            model.TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            foreach (var cartItem in cartItems)
            {
                var productDetail = await _productDetailRepository
                    .All()
                    .FirstOrDefaultAsync(pd => pd.ProductId == cartItem.ProductId && pd.Size == cartItem.Size);

                if (productDetail == null || productDetail.Quantity < cartItem.Quantity)
                {
                    TempData["error"] = "Недостатъчна наличност от този размер";
                    return View(model);
                }
            }

            var orderId = await _orderService.CreateOrderAsync(model.UserId, model);

            await _cartService.ClearCartAsync(model.UserId);

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

            if (User.IsInRole("User"))
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                bool updatedUser = await _orderService.UpdateOrderUserAsync(model, userId);
                if (!updatedUser)
                {
                    return RedirectToAction("MyOrders");
                }

                return RedirectToAction("Details", new { orderId = model.OrderId });

            }
            bool updatedAdmin = await _orderService.UpdateOrderAdminAsync(model);
            if (!updatedAdmin)
            {
                return RedirectToAction("MyOrders");
            }

            return RedirectToAction("Details", new { orderId = model.OrderId });
        }

        [HttpGet]
        public async Task<IActionResult> UserOrderDetails(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (order.UserId != userId)
            {
                return Forbid();
            }

            return View(order);
        }

        [HttpGet]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDetails(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

           
            return View("Details", order);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View("AllOrders", orders);
        }
    }
}
