using PrismWear.Web.ViewModels.Categories;

namespace PrismWear.Services.Data
{
    public interface ICategoriesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        Task CreateAsync(CreateCategoryInputModel input, string userId);

        Task EditAsync(int id, EditCategoryInputModel input);

        public EditCategoryInputModel GetById(int id);

        public Task DeleteAsync(int id);
    }
}
