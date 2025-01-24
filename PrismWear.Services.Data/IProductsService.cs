
using PrismWear.Web.ViewModels;
using PrismWear.Web.ViewModels.Products;

namespace PrismWear.Services.Data
{
    public interface IProductsService
    {
        Task CreateAsync(CreateProductInputModel input, string userId, string imagePath);

        IEnumerable<ProductInListViewModel> GetAll(int page, int itemsPerPage = 12);

        int GetCount();

        SingleProductViewModel GetById(int id);


        Task DeleteAsync(int id);

        Task EditAsync(int id, EditProductInputModel viewModel);
    }
}
