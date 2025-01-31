using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Web.ViewModels;
using PrismWear.Web.ViewModels.Products;
using PrismWear.Web.ViewModels.Sizes;

namespace PrismWear.Services.Data
{
    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsService(IDeletableEntityRepository<Product> productRepository)
        {
            this.productsRepository = productRepository;
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
            };

            product.ProductDetails = input.Sizes
       .Select(sq => new ProductDetail
       {
          
           Size = sq.SizeName,
           Quantity = sq.Quantity,
           ProductId=product.Id,
       })
       .ToList();

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

        
        public async Task DeleteAsync(int id)
        {
           var product=this.productsRepository.All().FirstOrDefault(x=>x.Id==id);
           this.productsRepository.Delete(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(int id, EditProductInputModel viewModel)
        {
            var product = this.productsRepository
      .All()                             
      .Include(x => x.ProductDetails)
      .FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                throw new NullReferenceException($"No product found with ID {id}.");
            }

            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.CategoryId = viewModel.CategoryId;

            var detailDict = product.ProductDetails
                .ToDictionary(d => d.Size, StringComparer.OrdinalIgnoreCase);

            foreach (var sizeModel in viewModel.Sizes)
            {
                if (detailDict.TryGetValue(sizeModel.Size, out var existingDetail))
                {
                    existingDetail.Quantity = sizeModel.Quantity;
                }
            }

                await this.productsRepository.SaveChangesAsync();
        }

        public IEnumerable<ProductInListViewModel> GetAll(int page, int itemsPerPage = 2)
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

        public IEnumerable<ProductInListViewModel> GetProductsByCategory(int categoryId)
        {
            var query = productsRepository.All()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .AsQueryable();

                query = query.Where(p => p.CategoryId == categoryId);


            // Convert to ViewModel before returning
            return query.Select(p => new ProductInListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                Images= p.Images,
            }).ToList();
        }

        public SingleProductViewModel GetById(int id)
        {
            var result = this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new SingleProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImageUrl = $"/images/products/{x.Images.FirstOrDefault().Id}.{x.Images.FirstOrDefault().Extension}",
                    Price = x.Price,
                    CategoryName = x.Category.Name,
                    Details = x.ProductDetails
                .Select(d => new ProductDetailViewModel
                {
                    Size = d.Size,
                    Quantity = d.Quantity,
                })
                .ToList(),
                })
                .FirstOrDefault();

            if (result == null) throw new NullReferenceException("No product");

            return result;
        }

        public EditProductInputModel GetByIdEdit(int id)
        {
            var result = this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new EditProductInputModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Sizes = x.ProductDetails
                    .Select(d => new ProductDetailViewModel
                     {
                         Size = d.Size,
                         Quantity = d.Quantity,
                     })
                .ToList(),
                })
                .FirstOrDefault();

            if (result == null) throw new NullReferenceException("No product");

            return result;
        }

        public int GetCount()
        {
            return this.productsRepository.All().Count();
        }
    }
}
