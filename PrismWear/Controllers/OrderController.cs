﻿using Microsoft.AspNetCore.Authorization;
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

            // Retrieve cart items:
            var cartItems = await _cartService.RetrieveUserCartAsync(userId);

            var model = new OrderInputViewModel
            {
                UserId = userId,
                CartItems = cartItems,
            };
            model.TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            return View(model); // ✅ Don't clear cart here!
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(OrderInputViewModel model)
        {
            var cartItems = await _cartService.RetrieveUserCartAsync(model.UserId); // ✅ Get cart items once
            model.CartItems = cartItems;
            model.TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // ✅ Check if stock is sufficient before creating an order
            foreach (var cartItem in cartItems) // ✅ Use retrieved cartItems instead of model.CartItems
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

            // ✅ Clear the cart only after successful order placement
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool updated = await _orderService.UpdateOrderAdminAsync(model);

            if (!updated)
            {
                // ✅ Redirect to the admin order list if the order was deleted
                return RedirectToAction("MyOrders");
            }

            return RedirectToAction("AdminDetails", new { orderId = model.OrderId });
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
            return View("AllOrders", orders); // ✅ Use the new "AllOrders" view
        }
    }
}
