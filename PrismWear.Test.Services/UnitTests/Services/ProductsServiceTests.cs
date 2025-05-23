﻿using Moq;
using NUnit.Framework;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Services.Data;
using PrismWear.Web.ViewModels;
using PrismWear.Web.ViewModels.Products;
using PrismWear.Web.ViewModels.Sizes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 
using Microsoft.EntityFrameworkCore;
using static System.StringComparer;

namespace PrismWear.Tests.Services
{
    [TestFixture]
    public class ProductsServiceTests
    {
        private Mock<IDeletableEntityRepository<Product>> _mockProductsRepo;
        private ProductsService _productsService;

        [SetUp]
        public void SetUp()
        {
            _mockProductsRepo = new Mock<IDeletableEntityRepository<Product>>();

            _productsService = new ProductsService(_mockProductsRepo.Object);
        }

        [Test]
        public void GetAll_ShouldReturnAllProductsAsViewModels()
        {
            var data = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Shirt",
                    Price = 29.99,
                    Category = new Category { Name = "Tops" },
                    Images = new List<Image>
                    {
                        new Image { Id = Guid.NewGuid().ToString(), Extension="png" }
                    }
                },
                new Product
                {
                    Id = 2,
                    Name = "Jeans",
                    Price = 59.99,
                    Category = new Category { Name = "Bottoms" },
                    Images = new List<Image>()
                }
            }.AsQueryable();

            _mockProductsRepo.Setup(r => r.All()).Returns(data);

            var result = _productsService.GetAll().ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Shirt"));
            Assert.That(result[0].Price, Is.EqualTo(29.99));
            Assert.That(result[0].CategoryName, Is.EqualTo("Tops"));
            Assert.That(result[0].Images.Count, Is.EqualTo(1));

            Assert.That(result[1].Name, Is.EqualTo("Jeans"));
            Assert.That(result[1].CategoryName, Is.EqualTo("Bottoms"));
            Assert.That(result[1].Images.Count, Is.EqualTo(0));

            _mockProductsRepo.Verify(r => r.All(), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ShouldAddProductAndSaveChanges()
        {
            var inputModel = new CreateProductInputModel
            {
                Name = "New Product",
                Description = "Description",
                Price = 49.99,
                CategoryId = 123,
                Sizes = new List<SizeQuantityViewModel>
        {
            new() { SizeName = "S", Quantity = 10 },
            new() { SizeName = "M", Quantity = 5 },
        },
                Images = new List<IFormFile>
        {
            new FakeFormFile { FileName = "test.png" }
        },
            };

            var userId = "TestUserId";
            var imagePath = "SomeTestPath";

            _mockProductsRepo
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            _mockProductsRepo
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _productsService.CreateAsync(inputModel, userId, imagePath);

            _mockProductsRepo.Verify(r => r.AddAsync(It.Is<Product>(p =>
                p.Name == "New Product"
                && p.Description == "Description"
                && p.CategoryId == 123
                && p.ProductDetails.Count == 2
                && p.Images.Count == 1
            )), Times.Once);

            _mockProductsRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }


        [Test]
        public async Task DeleteAsync_ShouldRemoveProductAndSaveChanges()
        {
            var productToDelete = new Product
            {
                Id = 999,
                Name = "ToDelete",
            };

            var data = new List<Product> { productToDelete }.AsQueryable();

            _mockProductsRepo.Setup(r => r.All()).Returns(data);
            _mockProductsRepo.Setup(r => r.Delete(It.IsAny<Product>())).Verifiable();

            _mockProductsRepo.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _productsService.DeleteAsync(999);

            _mockProductsRepo.Verify(r => r.Delete(It.Is<Product>(p => p.Id == 999)), Times.Once);
            _mockProductsRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task EditAsync_ShouldUpdateProductAndSaveChanges()
        {
            var existingProduct = new Product
            {
                Id = 10,
                Name = "OldName",
                Description = "OldDesc",
                Price = 10.0,
                CategoryId = 1,
                ProductDetails = new List<ProductDetail>
        {
            new ProductDetail { Size = "S", Quantity = 5 },
            new ProductDetail { Size = "M", Quantity = 10 },
        }
            };

            var data = new List<Product> { existingProduct }.AsQueryable();

            _mockProductsRepo
                .Setup(r => r.All())
                .Returns(data);

            _mockProductsRepo
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            var editModel = new EditProductInputModel
            {
                Id = 10,
                Name = "NewName",
                Description = "NewDesc",
                Price = 25.0,
                CategoryId = 2,
                Sizes = new List<ProductDetailViewModel>
        {
            new ProductDetailViewModel { Size = "S", Quantity = 99 },
            new ProductDetailViewModel { Size = "M", Quantity = 50 },
        }
            };

            await _productsService.EditAsync(10, editModel);

            Assert.That(existingProduct.Name, Is.EqualTo("NewName"));
            Assert.That(existingProduct.Description, Is.EqualTo("NewDesc"));
            Assert.That(existingProduct.Price, Is.EqualTo(25.0));
            Assert.That(existingProduct.CategoryId, Is.EqualTo(2));

            var sDetail = existingProduct.ProductDetails.FirstOrDefault(d => d.Size == "S");
            Assert.That(sDetail.Quantity, Is.EqualTo(99));

            var mDetail = existingProduct.ProductDetails.FirstOrDefault(d => d.Size == "M");
            Assert.That(mDetail.Quantity, Is.EqualTo(50));

            _mockProductsRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }


        [Test]
        public void GetFilteredProducts_ShouldFilterByCategoryAndPrice()
        {
            var productsData = new List<Product>
            {
                new Product { Id = 1, CategoryId = 5, Price = 50, Category = new Category { Name = "Shirts" } },
                new Product { Id = 2, CategoryId = 5, Price = 100, Category = new Category { Name = "Shirts" } },
                new Product { Id = 3, CategoryId = 10, Price = 200, Category = new Category { Name = "Pants" } },
            }.AsQueryable();

            _mockProductsRepo.Setup(r => r.All())
                .Returns(productsData);

            var results = _productsService.GetFilteredProducts(categoryId: 5, minPrice: 50, maxPrice: 150).ToList();

            Assert.That(results.Count, Is.EqualTo(2));
            Assert.That(results.Any(x => x.Id == 3), Is.False);
        }

        [Test]
        public void GetById_ShouldReturnSingleProduct_WhenFound()
        {
            var productsData = new List<Product>
            {
                new Product
                {
                    Id = 100,
                    Name = "The Product",
                    Description = "Desc",
                    Price = 99.99,
                    Category = new Category { Name = "CategoryName" },
                    Images = new List<Image>
                    {
                        new Image { Id = Guid.NewGuid().ToString(), Extension="jpg" }
                    },
                    ProductDetails = new List<ProductDetail>
                    {
                        new ProductDetail { Size="S", Quantity=5 }
                    }
                }
            }.AsQueryable();

            _mockProductsRepo.Setup(r => r.AllAsNoTracking())
                .Returns(productsData);

            var result = _productsService.GetById(100);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(100));
            Assert.That(result.Name, Is.EqualTo("The Product"));
            Assert.That(result.CategoryName, Is.EqualTo("CategoryName"));
            Assert.That(result.Images.Count, Is.EqualTo(1));
            Assert.That(result.Details.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetById_ShouldThrow_WhenProductDoesNotExist()
        {
            var emptyData = new List<Product>().AsQueryable();
            _mockProductsRepo.Setup(r => r.AllAsNoTracking()).Returns(emptyData);

            Assert.Throws<NullReferenceException>(() => _productsService.GetById(1234));
        }

        [Test]
        public void GetCount_ShouldReturnTotalNumberOfProducts()
        {
            var productsData = new List<Product>
            {
                new Product { Id = 1 },
                new Product { Id = 2 },
                new Product { Id = 3 }
            }.AsQueryable();

            _mockProductsRepo.Setup(r => r.All()).Returns(productsData);

            var count = _productsService.GetCount();

            Assert.That(count, Is.EqualTo(3));
        }

    }

    public class FakeFormFile : IFormFile
    {
        public string FileName { get; set; } = "fake.png";
        public string ContentType => "image/png";
        public string ContentDisposition => string.Empty;
        public IHeaderDictionary Headers => null;
        public long Length => 0;
        public string Name => "fake";

        public void CopyTo(Stream target) {  }
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
        public Stream OpenReadStream()
        {
            return new MemoryStream();
        }
    }
}
