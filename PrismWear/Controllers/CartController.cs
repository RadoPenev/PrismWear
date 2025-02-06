using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrismWear.Services.Data;
using System.Security.Claims;

namespace PrismWear.Controllers
{
    public class CartController : Controller
    {
       

        private readonly ICartService _cartService;
        private readonly UserManager<IdentityUser> userManager;

        public CartController(ICartService cartService,
            UserManager<IdentityUser> userManager)
        {
            _cartService = cartService;
            this.userManager = userManager;
        }

        // Display the user's cart
        [HttpGet]
        public async Task<IActionResult> Index()
        {
             var user = await this.userManager.GetUserAsync(this.User);
            var cartItems = await _cartService.GetCartItemsAsync(user.Id);
            return View(cartItems);
        }

        // Add an item to the cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
             var user = await this.userManager.GetUserAsync(this.User);
            await _cartService.AddToCartAsync(user.Id, productId, quantity);
            // For dynamic updates via AJAX you could return JSON instead
            return RedirectToAction(nameof(Index));
        }

        // Update the quantity of a cart item
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
             var user = await this.userManager.GetUserAsync(this.User);
            await _cartService.UpdateQuantityAsync(user.Id, productId, quantity);
            var cartItems = await _cartService.GetCartItemsAsync(user.Id);
            var totalItemCount = cartItems.Sum(ci => ci.Quantity);
            return Json(new {success=true, totalItemCount });
        }

        // Remove an item from the cart
        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            await _cartService.RemoveItemAsync(user.Id, productId);
            var cartItems = await _cartService.GetCartItemsAsync(user.Id);
            var totalItemCount = cartItems.Sum(ci => ci.Quantity);
            return Json(new { totalItemCount });
        }
            // Show the checkout page
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        // Optional: An AJAX endpoint to retrieve the updated cart item count dynamically.
        // This method can be invoked via JavaScript after an update.
        [HttpPost]
        public async Task<IActionResult> GetCartItemCount()
        {
             var user = await this.userManager.GetUserAsync(this.User);
            var cartItems = await _cartService.GetCartItemsAsync(user.Id);
            var totalItemCount = cartItems.Sum(ci => ci.Quantity);
            return Json(new { totalItemCount });
        }
    }
}
