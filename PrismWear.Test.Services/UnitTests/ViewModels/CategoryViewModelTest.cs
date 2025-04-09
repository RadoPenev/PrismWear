using NUnit.Framework;
using PrismWear.Web.ViewModels.Categories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PrismWear.Test.UnitTests.Models
{
    [TestFixture]
    public class CategoryViewModelTest
    {
        private bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        [Test]
        public void CategoryViewModel_ShouldBeInvalid_WhenNameIsMissing()
        {
            var model = new CategoryViewModel { Id = 1, Name = null };

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.MemberNames.Contains("Name")));
        }

        [Test]
        public void CategoryViewModel_ShouldBeInvalid_WhenNameTooShort()
        {
            var model = new CategoryViewModel { Id = 1, Name = "A" };

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.MemberNames.Contains("Name")));
        }

        [Test]
        public void CategoryViewModel_ShouldBeInvalid_WhenNameTooLong()
        {
            var model = new CategoryViewModel
            {
                Id = 1,
                Name = new string('X', 51)
            };

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.MemberNames.Contains("Name")));
        }

        [Test]
        public void CategoryViewModel_ShouldBeValid_WhenNameIsValid()
        {
            var model = new CategoryViewModel
            {
                Id = 1,
                Name = "Shirts"
            };

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.True);
            Assert.That(results, Is.Empty);
        }
    }
}
