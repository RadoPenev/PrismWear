using PrismWear.Web.ViewModels.Cart;
using PrismWear.Web.ViewModels.Sizes;
namespace PrismWear.Services.Data
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemViewModel>> GetCartItemsAsync(string userId);
        Task AddToCartAsync(string userId, int productId,int quantity=1);
        Task UpdateQuantityAsync(string userId, int productId, int quantity);
        Task RemoveItemAsync(string userId, int productId);
        Task ClearCartAsync(string userId);
    }
}
