using Moq;
using NUnit.Framework;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Services.Data;
using PrismWear.Web.ViewModels.Categories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismWear.Tests.Services
{
    [TestFixture]
    public class CategoriesServiceTests
    {
        private Mock<IDeletableEntityRepository<Category>> _mockCategoriesRepo;
        private CategoriesService _categoriesService;

        [SetUp]
        public void SetUp()
        {
            _mockCategoriesRepo = new Mock<IDeletableEntityRepository<Category>>();
            _categoriesService = new CategoriesService(_mockCategoriesRepo.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldAddCategoryAndSaveChanges()
        {
            var input = new CreateCategoryInputModel
            {
                Name = "New Category",
            };
            var userId = "test-user-id";

            _mockCategoriesRepo
                .Setup(r => r.AddAsync(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            _mockCategoriesRepo
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _categoriesService.CreateAsync(input, userId);

            _mockCategoriesRepo.Verify(r => r.AddAsync(It.Is<Category>(c =>
                c.Name == "New Category" &&
                c.AddedByUser == userId
            )), Times.Once);

            _mockCategoriesRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task EditAsync_ShouldUpdateCategoryAndSaveChanges()
        {
            var existingCategory = new Category
            {
                Id = 10,
                Name = "OldName",
            };
            var data = new List<Category> { existingCategory }.AsQueryable();

            _mockCategoriesRepo
                .Setup(r => r.All())
                .Returns(data);

            _mockCategoriesRepo
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            var input = new EditCategoryInputModel
            {
                Id = 10,
                Name = "NewName",
            };

            await _categoriesService.EditAsync(10, input);

            Assert.That(existingCategory.Name, Is.EqualTo("NewName"));
            _mockCategoriesRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void GetAllAsKeyValuePairs_ShouldReturnAllCategoriesAsKeyValuePairsOrderedByName()
        {
            var data = new List<Category>
            {
                new Category { Id = 2, Name = "Beta" },
                new Category { Id = 1, Name = "Alpha" },
                new Category { Id = 3, Name = "Gamma" },
            }.AsQueryable();

            _mockCategoriesRepo
                .Setup(r => r.All())
                .Returns(data);

            var result = _categoriesService.GetAllAsKeyValuePairs().ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Key, Is.EqualTo("1")); 
            Assert.That(result[0].Value, Is.EqualTo("Alpha"));
            Assert.That(result[1].Key, Is.EqualTo("2"));
            Assert.That(result[1].Value, Is.EqualTo("Beta"));
            Assert.That(result[2].Key, Is.EqualTo("3"));
            Assert.That(result[2].Value, Is.EqualTo("Gamma"));
        }

        [Test]
        public void GetById_ShouldReturnEditCategoryInputModel_WhenCategoryExists()
        {
            var data = new List<Category>
            {
                new Category { Id = 100, Name = "CategoryName" },
            }.AsQueryable();

            _mockCategoriesRepo
                .Setup(r => r.AllAsNoTracking())
                .Returns(data);

            var result = _categoriesService.GetById(100);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(100));
            Assert.That(result.Name, Is.EqualTo("CategoryName"));
        }

        [Test]
        public void GetById_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            var data = new List<Category>()
                .AsQueryable();

            _mockCategoriesRepo
                .Setup(r => r.AllAsNoTracking())
                .Returns(data);

            var result = _categoriesService.GetById(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveCategoryAndSaveChanges()
        {
            var categoryToDelete = new Category
            {
                Id = 999,
                Name = "ToDelete",
            };

            var data = new List<Category> { categoryToDelete }.AsQueryable();

            _mockCategoriesRepo
                .Setup(r => r.All())
                .Returns(data);

            _mockCategoriesRepo
                .Setup(r => r.Delete(It.IsAny<Category>()))
                .Verifiable();

            _mockCategoriesRepo
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            await _categoriesService.DeleteAsync(999);

            _mockCategoriesRepo.Verify(r => r.Delete(It.Is<Category>(c => c.Id == 999)), Times.Once);
            _mockCategoriesRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
