using Microsoft.EntityFrameworkCore;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Web.ViewModels;
using PrismWear.Web.ViewModels.Products;
using PrismWear.Web.ViewModels.Sizes;

namespace PrismWear.Services.Data
{
    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsService;

        public ProductsService(IDeletableEntityRepository<Product> productsService)
        {
            this.productsService = productsService;
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

            await this.productsService.AddAsync(product);
            await this.productsService.SaveChangesAsync();
        }

        
        public async Task DeleteAsync(int id)
        {
           var product=this.productsService.All().FirstOrDefault(x=>x.Id==id);
           this.productsService.Delete(product);
            await this.productsService.SaveChangesAsync();
        }

        public async Task EditAsync(int id, EditProductInputModel viewModel)
        {
            var product = this.productsService
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

                await this.productsService.SaveChangesAsync();
        }

        public IEnumerable<ProductInListViewModel> GetAll(int page, int itemsPerPage = 2)
        {
            return this.productsService.AllAsNoTracking()
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

        public SingleProductViewModel GetById(int id)
        {
            var result = this.productsService
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
            var result = this.productsService
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
            return this.productsService.All().Count();
        }
    }
}
