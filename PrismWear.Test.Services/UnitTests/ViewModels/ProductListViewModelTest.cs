using NUnit.Framework;
using PrismWear.Web.ViewModels.Products;
using System.Collections.Generic;

namespace PrismWear.Test.UnitTests.Models
{
    [TestFixture]
    public class ProductListViewModelTest
    {
        [Test]
        public void PagesCount_ShouldBeCorrectlyCalculated()
        {
            var model = new ProductListViewModel
            {
                ProductsCount = 45,
                ItemsPerPage = 10
            };

            Assert.That(model.PagesCount, Is.EqualTo(5));
        }

        [Test]
        public void HasPreviousPage_ShouldBeTrue_WhenPageNumberGreaterThan1()
        {
            var model = new ProductListViewModel { PageNumber = 2 };

            Assert.That(model.HasPreviousPage, Is.True);
        }

        [Test]
        public void HasPreviousPage_ShouldBeFalse_WhenOnFirstPage()
        {
            var model = new ProductListViewModel { PageNumber = 1 };

            Assert.That(model.HasPreviousPage, Is.False);
        }

        [Test]
        public void HasNextPage_ShouldBeTrue_WhenNotOnLastPage()
        {
            var model = new ProductListViewModel
            {
                PageNumber = 2,
                ProductsCount = 30,
                ItemsPerPage = 10 // 3 pages total
            };

            Assert.That(model.HasNextPage, Is.True);
        }

        [Test]
        public void HasNextPage_ShouldBeFalse_WhenOnLastPage()
        {
            var model = new ProductListViewModel
            {
                PageNumber = 3,
                ProductsCount = 30,
                ItemsPerPage = 10
            };

            Assert.That(model.HasNextPage, Is.False);
        }

        [Test]
        public void PreviousPageNumber_ShouldBeOneLessThanCurrent()
        {
            var model = new ProductListViewModel { PageNumber = 5 };

            Assert.That(model.PreviousPageNumber, Is.EqualTo(4));
        }

        [Test]
        public void NextPageNumber_ShouldBeOneMoreThanCurrent()
        {
            var model = new ProductListViewModel { PageNumber = 3 };

            Assert.That(model.NextPageNumber, Is.EqualTo(4));
        }
        [Test]
        public void PagesCount_ShouldBeZero_WhenItemsPerPageIsZero()
        {
            var model = new ProductListViewModel
            {
                ProductsCount = 100,
                ItemsPerPage = 0
            };

            Assert.That(model.PagesCount, Is.EqualTo(0));
        }

    }
}
