using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Web.ViewModels.Cart;

namespace PrismWear.Services.Data
{
    public class CartService : ICartService
    {
        private readonly IDeletableEntityRepository<Cart> cartRepository;
        private readonly UserManager<IdentityUser> userManager;

        public CartService(IDeletableEntityRepository<Cart> cartRepository,
            UserManager<IdentityUser> userManager)
        {
            this.cartRepository = cartRepository;
            this.userManager = userManager;
        }
        public async Task<IEnumerable<CartItemViewModel>> GetCartItemsAsync(string userId)
        {
            var cart = await cartRepository.All()
                .Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync();

            if (cart == null || cart.CartItems == null)
            {
                return Enumerable.Empty<CartItemViewModel>();
            }

            var items = cart.CartItems.Select(ci => new CartItemViewModel
            {
                ProductId = ci.ProductId,
                Name = ci.Product.Name,
                CategoryName = ci.Product.Category?.Name ?? "Unknown Category",
                Price = ci.Product.Price,
                Quantity = ci.Quantity,
                ImageUrl = $"/images/products/{ci.Product.Images.First().Id}.{ci.Product.Images.First().Extension}",
                Size=ci.Size,
            });

            return items;
        }


        public async Task AddToCartAsync(string userId, int productId, string size, int quantity = 1)
        {
            var cart = await cartRepository.All()
                .Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await cartRepository.AddAsync(cart);
            }

            var existingItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId && ci.Size == size);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Size = size,
                };
                cart.CartItems.Add(newItem);
            }

            await cartRepository.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(string userId, int productId, int quantity)
        {
            var cart = await cartRepository.All()
                .Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                return;
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingItem != null)
            {
                if (quantity > 0)
                {
                    existingItem.Quantity = quantity;
                }
                else
                {
                    cart.CartItems.Remove(existingItem);
                }

                await cartRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveItemAsync(string userId, int productId)
        {
            var cart = await cartRepository.All()
                .Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (cart == null || cart.CartItems == null)
            {
                return;
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingItem != null)
            {
                cartRepository.Delete(existingItem.Cart);
                await cartRepository.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await cartRepository.All()
                .Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (cart != null)
            {
                cart.CartItems.Clear();
                await cartRepository.SaveChangesAsync();
            }
        }
    }
}
