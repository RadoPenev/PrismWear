using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Web.ViewModels;
using PrismWear.Web.ViewModels.Products;
using System.Net.Http.Headers;

namespace PrismWear.Services.Data
{
    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsService(IDeletableEntityRepository<Product> productsRepository)
        {
            this.productsRepository = productsRepository;
        }

        public async Task CreateAsync(CreateProductInputModel input, string userId, string imagePath)
        {
            var product = new Product
            {
                Description = input.Description,
                CategoryId = input.CategoryId,
                AddedByUser = userId,
                Name = input.Name,
                Price = input.Price,
                Size = input.Size,
            };

            Directory.CreateDirectory($"{imagePath}/products/");

            var allowedExtensions = new[] { "jpeg", "png", "gif" };

            foreach (var image in input.Images)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');

                if (!allowedExtensions.Any(x=>x.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension}!");
                }
                var dbImages = new Image
                {
                    AddedByUser=userId,
                    Extension=extension,
                };
                product.Images.Add(dbImages);

                var physicalPath = $"{imagePath}/products/{dbImages.Id}.{extension}";
                using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
            }

            await this.productsRepository.AddAsync(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public IEnumerable<ProductInListViewModel> GetAll(int page, int itemsPerPage = 12)
        {
            return this.productsRepository.AllAsNoTracking()
                .OrderByDescending(x=>x.Id)
                .Skip((page-1)*itemsPerPage)
                .Take(itemsPerPage)
                .Select(product=> new ProductInListViewModel
                {
                    Id=product.Id,
                    Name=product.Name,
                    CategoryId=product.CategoryId,
                    Images = product.Images,
                    CategoryName=product.Category.Name,
                    
                })
                .ToList();
        }

        public int GetCount()
        {
            return this.productsRepository.All().Count();
        }
    }
}
