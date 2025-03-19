using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Web.ViewModels.Cart;
using System.Linq;

namespace PrismWear.Services.Data
{
    public class CartService : ICartService
    {
        private readonly IDeletableEntityRepository<Cart> cartRepository;
        private readonly IDeletableEntityRepository<CartItem> cartItemRepository;
        private readonly UserManager<IdentityUser> userManager;

        public CartService(IDeletableEntityRepository<Cart> cartRepository,
            IDeletableEntityRepository<CartItem> cartItemRepository,
            UserManager<IdentityUser> userManager)
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
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
            var cart = await this.cartRepository.All()
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
                this.cartItemRepository.Delete(existingItem);
                await this.cartItemRepository.SaveChangesAsync();

                cart.CartItems.Remove(existingItem);
            }
        }


        public async Task ClearCartAsync(string userId)
        {
            var cart = await cartRepository
                .All()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null && cart.CartItems.Any())
            {
                foreach (var item in cart.CartItems.ToList())
                {
                    cartItemRepository.Delete(item);
                }
                await cartItemRepository.SaveChangesAsync();
            }
        }

        public async Task<List<CartItem>> RetrieveUserCartAsync(string userId)
        {
            var cart = await cartRepository.All()
        .Where(c => c.UserId == userId && !c.IsDeleted)
        .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
        .FirstOrDefaultAsync();

            if (cart == null || cart.CartItems == null)
            {
                return new List<CartItem>();
            }

            return cart.CartItems.ToList();
        }


    }
}
