
using PrismWear.Data.Common.Models;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;

namespace PrismWear.Services.Data
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
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
    }
}
