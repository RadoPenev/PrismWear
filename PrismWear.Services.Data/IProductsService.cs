using PrismWear.Web.ViewModels;
using PrismWear.Web.ViewModels.Products;

namespace PrismWear.Services.Data
{
    public interface IProductsService
    {
        Task CreateAsync(CreateProductInputModel input, string userId, string imagePath);

        IEnumerable<ProductInListViewModel> GetAll(int page, int itemsPerPage = 2);

        int GetCount();

        SingleProductViewModel GetById(int id);

        EditProductInputModel GetByIdEdit(int id);

        Task DeleteAsync(int id);

        Task EditAsync(int id, EditProductInputModel viewModel);
    }
}
