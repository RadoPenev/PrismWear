using NUnit.Framework;
using PrismWear.Web.ViewModels.Products;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PrismWear.Test.UnitTests.Models
{
    [TestFixture]
    public class FiltersViewModelTest
    {
        private bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        [Test]
        public void FiltersViewModel_ShouldBeValid_WhenValuesAreNull()
        {
            var model = new FiltersViewModel
            {
                CategoryId = 1,
                MinPrice = null,
                MaxPrice = null
            };

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.True);
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void FiltersViewModel_ShouldBeInvalid_WhenMinPriceIsNegative()
        {
            var model = new FiltersViewModel
            {
                CategoryId = 1,
                MinPrice = -1,
                MaxPrice = 100
            };

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.MemberNames.Contains("MinPrice")));
        }

        [Test]
        public void FiltersViewModel_ShouldBeInvalid_WhenMaxPriceIsNegative()
        {
            var model = new FiltersViewModel
            {
                CategoryId = 1,
                MinPrice = 10,
                MaxPrice = -50
            };

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.MemberNames.Contains("MaxPrice")));
        }

        [Test]
        public void FiltersViewModel_ShouldBeValid_WhenAllValuesAreCorrect()
        {
            var model = new FiltersViewModel
            {
                CategoryId = 2,
                MinPrice = 10,
                MaxPrice = 100
            };

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.True);
            Assert.That(results, Is.Empty);
        }
    }
}
