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
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsService(IDeletableEntityRepository<Product> productRepository)
        {
            this.productsRepository = productRepository;
        }


        public IEnumerable<ProductInListViewModel> GetAll()
        {
            // This assumes you have a Product entity and a ProductInListViewModel
            // that matches the properties you need to display on the page.

            var products = this.productsRepository.All()
                .Select(p => new ProductInListViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    Images = p.Images
                        .Select(img => new Image
                        {
                            Id = img.Id,
                            Extension = img.Extension,
                        })
                        .ToList(),
                })
                .ToList();

            return products;
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

        public IEnumerable<ProductInListViewModel> GetProducts(FiltersViewModel filters, int page, int itemsPerPage = 8)
        {
            var query = this.productsRepository
                .AllAsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .AsQueryable();

            if (filters.CategoryId > 0)
            {
                query = query.Where(p => p.CategoryId == filters.CategoryId);
            }

            if (filters.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filters.MinPrice.Value);
            }

            if (filters.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filters.MaxPrice.Value);
            }

            query = query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage);

            return query
                .Select(product => new ProductInListViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    Price = product.Price,
                    Images = product.Images,
                })
                .ToList();
        }

        public IEnumerable<ProductInListViewModel> GetAll(int page, int itemsPerPage = 8)
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
                    Price = product.Price,
                })
                .ToList();
        }

        public IEnumerable<ProductInListViewModel> GetFilteredProducts(int? categoryId, double? minPrice, double? maxPrice)
        {
            var query = productsRepository.All()
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p=>p.ProductDetails)
                .AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            

            return query.Select(p => new ProductInListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                Images = p.Images,
                Price = p.Price,
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
                    Images = x.Images.ToList(),
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
