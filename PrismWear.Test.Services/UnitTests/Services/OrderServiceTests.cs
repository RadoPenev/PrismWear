using Moq;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Services.Data;
using PrismWear.Web.ViewModels.Orders;
using MockQueryable;

namespace PrismWear.Tests.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IDeletableEntityRepository<Order>> _orderRepoMock;
        private Mock<IDeletableEntityRepository<ProductDetail>> _productDetailRepoMock;
        private OrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            _orderRepoMock = new Mock<IDeletableEntityRepository<Order>>();
            _productDetailRepoMock = new Mock<IDeletableEntityRepository<ProductDetail>>();
            _orderService = new OrderService(_orderRepoMock.Object, _productDetailRepoMock.Object);
        }

        [Test]
        public async Task CreateOrderAsync_ShouldCreateAndReturnOrderId()
        {
            var userId = "user-1";
            var input = new OrderInputViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ShippingAddressLine1 = "123 St",
                ShippingCity = "New York",
                ShippingState = "NY",
                ShippingZipCode = "1001",
                ShippingCountry = "USA",
                CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        ProductId = 1,
                        Quantity = 2,
                        Size = "M",
                        Product = new Product { Price = 50, Id = 1 }
                    }
                }
            };

            Order? createdOrder = null;
            _orderRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Order>()))
                .Callback<Order>(o => createdOrder = o)
                .Returns(Task.CompletedTask);

            _orderRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var resultId = await _orderService.CreateOrderAsync(userId, input);

            Assert.That(createdOrder, Is.Not.Null);
            Assert.That(createdOrder!.OrderItems.Count, Is.EqualTo(1));
            Assert.That(createdOrder.TotalAmount, Is.EqualTo(100));
            Assert.That(resultId, Is.EqualTo(createdOrder.Id));
        }

        [Test]
        public async Task AcceptOrderAsync_ShouldReturnFalse_WhenOrderDoesNotExist()
        {
            var mock = new List<Order>().AsQueryable().BuildMock();
            _orderRepoMock.Setup(r => r.All()).Returns(mock);

            var result = await _orderService.AcceptOrderAsync(1);
            Assert.That(result,Is.False);
        }

        [Test]
        public async Task AcceptOrderAsync_ShouldReturnFalse_WhenOrderIsNotPending()
        {
            var order = new Order { Id = 1, OrderStatus = OrderStatus.Completed };
            var mock = new List<Order> { order }.AsQueryable().BuildMock();
            _orderRepoMock.Setup(r => r.All()).Returns(mock );

            var result = await _orderService.AcceptOrderAsync(1);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AcceptOrderAsync_ShouldMarkOrderAsCompleted()
        {
            var order = new Order { Id = 1, OrderStatus = OrderStatus.Pending };
            var mock = new List<Order> { order }.AsQueryable().BuildMock();
            _orderRepoMock.Setup(r => r.All()).Returns(mock );
            _orderRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _orderService.AcceptOrderAsync(1);

            Assert.That(result, Is.True);
            Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.Completed));
        }

        [Test]
        public async Task HasAnyOrdersAsync_ShouldReturnTrue_WhenOrdersExist()
        {
            var mock = new List<Order> { new Order { UserId = "abc" } }.AsQueryable().BuildMock();
            _orderRepoMock.Setup(r => r.All()).Returns(mock);

            var result = await _orderService.HasAnyOrdersAsync("abc");
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task HasAnyOrdersAsync_ShouldReturnFalse_WhenUserIdIsNull()
        {
            var result = await _orderService.HasAnyOrdersAsync(null);
            Assert.That(result,Is.False);
        }

        [Test]
        public async Task UpdateOrderUserAsync_ShouldUpdateOrder_WhenValid()
        {
            var model = new EditOrderViewModel
            {
                OrderId = 1,
                FirstName = "Updated",
                LastName = "Name",
                Email = "updated@example.com",
                ShippingAddressLine1 = "New St",
                ShippingCity = "New York",
                ShippingState = "NY",
                ShippingZipCode = "1234",
                ShippingCountry = "USA"
            };
            var order = new Order { Id = 1, UserId = "user-1" };
            var mock = new List<Order> { order }.AsQueryable().BuildMock();
            _orderRepoMock.Setup(r => r.All()).Returns(mock);
            _orderRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _orderService.UpdateOrderUserAsync(model, "user-1");

            Assert.That(result, Is.True);
            Assert.That(order.FirstName, Is.EqualTo("Updated"));
        }

        [Test]
        public async Task UpdateOrderUserAsync_ShouldReturnFalse_WhenOrderNotFound()
        {
            var mock = new List<Order>().AsQueryable().BuildMock();
            _orderRepoMock.Setup(r => r.All()).Returns(mock);

            var result = await _orderService.UpdateOrderUserAsync(new EditOrderViewModel { OrderId = 1 }, "user-1");
            Assert.That(result,Is.False);
        }

        [Test]
        public async Task UpdateOrderAdminAsync_ShouldUpdateOrderStatusToShippedAndDeductStock()
        {
            var detail = new ProductDetail { Size = "M", Quantity = 10 };
            var product = new Product { ProductDetails = new List<ProductDetail> { detail } };
            var orderItem = new OrderItem { Size = "M", Quantity = 2, Product = product };
            var order = new Order
            {
                Id = 1,
                OrderStatus = OrderStatus.Pending,
                OrderItems = new List<OrderItem> { orderItem }
            };

            var mock = new List<Order> { order }.AsQueryable().BuildMock();
            _orderRepoMock.Setup(r => r.All()).Returns(mock);
            _orderRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _orderService.UpdateOrderAdminAsync(new EditOrderViewModel { OrderId = 1, OrderStatus = OrderStatus.Shipped });

            Assert.That(result,Is.True);
            Assert.That(detail.Quantity, Is.EqualTo(8));
        }

        [Test]
        public async Task UpdateOrderAdminAsync_ShouldDeleteOrder_WhenStatusCompleted()
        {
            var order = new Order { Id = 1, OrderStatus = OrderStatus.Pending };
            var mock = new List<Order> { order }.AsQueryable().BuildMock();
            _orderRepoMock.Setup(r => r.All()).Returns(mock);
            _orderRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _orderService.UpdateOrderAdminAsync(new EditOrderViewModel { OrderId = 1, OrderStatus = OrderStatus.Completed });

            Assert.That(result,Is.False);
            _orderRepoMock.Verify(r => r.Delete(order), Times.Once);
        }

        [Test]
        public async Task UpdateOrderAdminAsync_ShouldReturnFalse_WhenOrderNotFound()
        {
            var mock = new List<Order>().AsQueryable().BuildMock();
            _orderRepoMock.Setup(r => r.All()).Returns(mock);

            var result = await _orderService.UpdateOrderAdminAsync(new EditOrderViewModel { OrderId = 999 });
            Assert.That(result, Is.False);
        }
    }
}
