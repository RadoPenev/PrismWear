using PrismWear.Data.Models;
using PrismWear.Web.ViewModels;
using PrismWear.Web.ViewModels.Products;

namespace PrismWear.Services.Data
{
    public interface IProductsService
    {
        Task CreateAsync(CreateProductInputModel input, string userId, string imagePath);
        IEnumerable<ProductInListViewModel> GetProducts(FiltersViewModel filters, int page, int itemsPerPage = 8);
        int GetCount();

        SingleProductViewModel GetById(int id);

        EditProductInputModel GetByIdEdit(int id);

        Task DeleteAsync(int id);

        Task EditAsync(int id, EditProductInputModel viewModel);

    }
}
