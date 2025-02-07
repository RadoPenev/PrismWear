using Microsoft.AspNetCore.Cors.Infrastructure;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
             var user = await this.userManager.GetUserAsync(this.User);
            var cartItems = await _cartService.GetCartItemsAsync(user.Id);
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, string size, int quantity = 1)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            await _cartService.AddToCartAsync(user.Id, productId, size, quantity);
            var updatedCartItems = await _cartService.GetCartItemsAsync(user.Id);
            var totalItemCount = updatedCartItems.Sum(x => x.Quantity);

            return Ok(new { totalItemCount });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
             var user = await this.userManager.GetUserAsync(this.User);
            await _cartService.UpdateQuantityAsync(user.Id, productId, quantity);
            var cartItems = await _cartService.GetCartItemsAsync(user.Id);
            var totalItemCount = cartItems.Sum(ci => ci.Quantity);
            return Json(new {success=true, totalItemCount });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            await _cartService.RemoveItemAsync(user.Id, productId);
            var cartItems = await _cartService.GetCartItemsAsync(user.Id);
            var totalItemCount = cartItems.Sum(ci => ci.Quantity);
            return Json(new { totalItemCount });
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

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
