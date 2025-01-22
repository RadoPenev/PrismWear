
using PrismWear.Web.ViewModels;

namespace PrismWear.Services.Data
{
    public interface IProductsService
    {
        Task CreateAsync(CreateProductInputModel input, string userId, string imagePath);

    }
}
