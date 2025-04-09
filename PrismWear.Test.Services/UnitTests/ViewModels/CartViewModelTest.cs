using NUnit.Framework;
using PrismWear.Web.ViewModels.Cart;
using System.Collections.Generic;

namespace PrismWear.Test.UnitTests.Models
{
    [TestFixture]
    public class CartViewModelTest
    {
        [Test]
        public void Subtotal_ShouldCalculateCorrectly()
        {
            var cart = new CartViewModel
            {
                Items = new List<CartItemViewModel>
                {
                    new CartItemViewModel { Price = 20.0, Quantity = 2 },
                    new CartItemViewModel { Price = 15.5, Quantity = 1 }
                }
            };

            double expectedSubtotal = 20.0 * 2 + 15.5;
            Assert.That(cart.Subtotal, Is.EqualTo(expectedSubtotal));
        }

        [Test]
        public void Total_ShouldIncludeShippingCost()
        {
            var cart = new CartViewModel
            {
                Items = new List<CartItemViewModel>
                {
                    new CartItemViewModel { Price = 10.0, Quantity = 3 }
                },
                ShippingCost = 5.0
            };

            double expectedSubtotal = 10.0 * 3;
            double expectedTotal = expectedSubtotal + 5.0;

            Assert.That(cart.Subtotal, Is.EqualTo(expectedSubtotal));
            Assert.That(cart.Total, Is.EqualTo(expectedTotal));
        }

        [Test]
        public void Subtotal_ShouldBeZero_WhenCartIsEmpty()
        {
            var cart = new CartViewModel();

            Assert.That(cart.Subtotal, Is.EqualTo(0));
            Assert.That(cart.Total, Is.EqualTo(cart.ShippingCost)); 
        }
    }
}
