using NUnit.Framework;
using PrismWear.Web.ViewModels;

namespace PrismWear.Test.UnitTests.Models
{
    [TestFixture]
    public class ErrorViewModelTest
    {
        [Test]
        public void ShowRequestId_ShouldReturnTrue_WhenRequestIdIsNotNullOrEmpty()
        {
            var model = new ErrorViewModel
            {
                RequestId = "abc123"
            };

            Assert.That(model.ShowRequestId, Is.True);
        }

        [Test]
        public void ShowRequestId_ShouldReturnFalse_WhenRequestIdIsNull()
        {
            var model = new ErrorViewModel
            {
                RequestId = null
            };

            Assert.That(model.ShowRequestId, Is.False);
        }

        [Test]
        public void ShowRequestId_ShouldReturnFalse_WhenRequestIdIsEmpty()
        {
            var model = new ErrorViewModel
            {
                RequestId = ""
            };

            Assert.That(model.ShowRequestId,Is.False);
        }
    }
}
