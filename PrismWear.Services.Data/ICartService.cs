using PrismWear.Data.Models;
using PrismWear.Web.ViewModels.Cart;
namespace PrismWear.Services.Data
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemViewModel>> GetCartItemsAsync(string userId);
        Task AddToCartAsync(string userId, int productId,string size,int quantity=1);
        Task UpdateQuantityAsync(string userId, int productId, int quantity);
        Task RemoveItemAsync(string userId, int productId);
        Task ClearCartAsync(string userId);

        Task<List<CartItem>> RetrieveUserCartAsync(string userId);

     
    }
}
