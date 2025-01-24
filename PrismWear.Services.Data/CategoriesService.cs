
using PrismWear.Data.Common.Models;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Web.ViewModels.Categories;
using PrismWear.Web.ViewModels.Products;

namespace PrismWear.Services.Data
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task CreateAsync(CreateCategoryInputModel input, string userId)
        {
            var category = new Category
            {
                Name = input.Name,
                AddedByUser = userId,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(int id, EditCategoryInputModel input)
        {
           var category = this.categoriesRepository.All().FirstOrDefault(c => c.Id == id);

            category.Name=input.Name;

            await this.categoriesRepository.SaveChangesAsync();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.categoriesRepository.All().Select(x => new
            {
                x.Id,
                x.Name
            }).ToList()
            .OrderBy(x => x.Name)
            .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name.ToString()));

        }

        public EditCategoryInputModel GetById(int id)
        {
            var result = this.categoriesRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new EditCategoryInputModel
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .FirstOrDefault();

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var product = this.categoriesRepository.All().FirstOrDefault(x => x.Id == id);
            this.categoriesRepository.Delete(product);
            await this.categoriesRepository.SaveChangesAsync();
        }

    }
}
