using NUnit.Framework;
using PrismWear.Web.ViewModels.Cart;
using PrismWear.Web.ViewModels.Orders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PrismWear.Test.UnitTests.Models
{
    [TestFixture]
    public class OrderViewModelTest
    {
        private bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        [Test]
        public void OrderViewModel_ShouldBeInvalid_WhenRequiredFieldsMissing()
        {
            var model = new OrderViewModel();

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results.Any());
            Assert.That(results.Any(r => r.MemberNames.Contains("OrderId")));
            Assert.That(results.Any(r => r.MemberNames.Contains("ShippingAddress")));
            Assert.That(results.Any(r => r.MemberNames.Contains("CartItems")));
        }

        [Test]
        public void OrderViewModel_ShouldBeInvalid_WhenZipTooShort()
        {
            var model = GetValidModel();
            model.ShippingZipCode = "12"; // Too short

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.MemberNames.Contains("ShippingZipCode")));
        }

        [Test]
        public void OrderViewModel_ShouldBeInvalid_WhenTotalNegative()
        {
            var model = GetValidModel();
            model.TotalAmount = -5;

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.MemberNames.Contains("TotalAmount")));
        }

        [Test]
        public void OrderViewModel_ShouldBeValid_WhenAllFieldsCorrect()
        {
            var model = GetValidModel();

            var isValid = ValidateModel(model, out var results);

            Assert.That(isValid, Is.True); 
            Assert.That(results, Is.Empty);    
        }

        private OrderViewModel GetValidModel()
        {
            return new OrderViewModel
            {
                OrderId = "O-123",
                UserId = "U-123",
                CartId = "C-123",
                SubTotal = 50,
                ShippingCost = 5,
                TotalAmount = 55,
                ShippingAddress = "123 Main Street",
                ShippingCity = "Cityville",
                ShippingState = "Stateville",
                ShippingZipCode = "12345",
                CartItems = new List<CartItemViewModel>
                {
                    new CartItemViewModel { ProductId = 1, Price = 25, Quantity = 2 }
                }
            };
        }
    }
}
