using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Services.Data;
using Moq;
using MockQueryable;
using Microsoft.AspNetCore.Identity;

namespace PrismWear.Tests.Services
{
   

    [TestFixture]
    public class CartServiceTests
    {
        private Mock<IDeletableEntityRepository<Cart>> _mockCartRepo;
        private Mock<IDeletableEntityRepository<CartItem>> _mockCartItemRepo;
        private CartService _cartService;
        private Mock<Microsoft.AspNetCore.Identity.UserManager<IdentityUser>> _mockUserManager;

        [SetUp]
        public void SetUp()
        {
            _mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();
            _mockCartItemRepo = new Mock<IDeletableEntityRepository<CartItem>>();
            _mockUserManager = BuildUserManagerMock();

            _cartService = new CartService(
                _mockCartRepo.Object,
                _mockCartItemRepo.Object,
                _mockUserManager.Object);
        }

        private Mock<Microsoft.AspNetCore.Identity.UserManager<IdentityUser>> BuildUserManagerMock()
        {
            var store = new Mock<Microsoft.AspNetCore.Identity.IUserStore<IdentityUser>>();
            return new Mock<Microsoft.AspNetCore.Identity.UserManager<IdentityUser>>(
                store.Object,
                null, null, null, null, null, null, null, null);
        }

        [Test]
        public async Task GetCartItemsAsync_ShouldReturnItems_WhenCartExists()
        {
            var userId = "user-1";
            var product = new Product
            {
                Id = 1,
                Name = "T-Shirt",
                Price = 30,
                Category = new Category { Name = "Clothes" },
                Images = new List<Image> { new Image { Id = "img123", Extension = "jpg" } }
            };
            var cart = new Cart
            {
                UserId = userId,
                CartItems = new List<CartItem>
                {
                    new CartItem { Product = product, ProductId = 1, Quantity = 2, Size = "L" }
                }
            };
            var cartMock = new List<Cart> { cart }.AsQueryable().BuildMock();
            _mockCartRepo.Setup(x => x.All()).Returns(cartMock);

            var result = (await _cartService.GetCartItemsAsync(userId)).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("T-Shirt"));
            Assert.That(result[0].Quantity, Is.EqualTo(2));
            Assert.That(result[0].CategoryName, Is.EqualTo("Clothes"));
        }

        [Test]
        public async Task GetCartItemsAsync_ShouldReturnEmpty_WhenCartMissing()
        {
            var cartMock = new List<Cart>().AsQueryable().BuildMock();
            _mockCartRepo.Setup(x => x.All()).Returns(cartMock);

            var result = await _cartService.GetCartItemsAsync("missing-user");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task AddToCartAsync_ShouldCreateNewCart_WhenNoneExists()
        {
            var userId = "new-user";
            var cartMock = new List<Cart>().AsQueryable().BuildMock();
            _mockCartRepo.Setup(x => x.All()).Returns(cartMock);

            _mockCartRepo.Setup(r => r.AddAsync(It.IsAny<Cart>())).Returns(Task.CompletedTask);
            _mockCartRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            await _cartService.AddToCartAsync(userId, 10, "M", 1);

            _mockCartRepo.Verify(r => r.AddAsync(It.Is<Cart>(c => c.UserId == userId)), Times.Once);
            _mockCartRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task AddToCartAsync_ShouldUpdateExistingItem()
        {
            var userId = "user-update";
            var cart = new Cart
            {
                UserId = userId,
                CartItems = new List<CartItem> { new CartItem { ProductId = 1, Size = "M", Quantity = 1 } }
            };
            var cartMock = new List<Cart> { cart }.AsQueryable().BuildMock();
            _mockCartRepo.Setup(x => x.All()).Returns(cartMock);
            _mockCartRepo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            await _cartService.AddToCartAsync(userId, 1, "M", 2);

            Assert.That(cart.CartItems.First().Quantity, Is.EqualTo(3));
        }

        [Test]
        public async Task UpdateQuantityAsync_ShouldChangeQuantity()
        {
            var userId = "q-user";
            var item = new CartItem { ProductId = 5, Quantity = 1 };
            var cart = new Cart { UserId = userId, CartItems = new List<CartItem> { item } };
            var mock = new List<Cart> { cart }.AsQueryable().BuildMock();
            _mockCartRepo.Setup(x => x.All()).Returns(mock);
            _mockCartRepo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            await _cartService.UpdateQuantityAsync(userId, 5, 10);

            Assert.That(item.Quantity, Is.EqualTo(10));
        }

        [Test]
        public async Task UpdateQuantityAsync_ShouldRemoveItem_WhenQuantityZero()
        {
            var userId = "zero-q";
            var item = new CartItem { ProductId = 99, Quantity = 2 };
            var cart = new Cart { UserId = userId, CartItems = new List<CartItem> { item } };
            var mock = new List<Cart> { cart }.AsQueryable().BuildMock();
            _mockCartRepo.Setup(x => x.All()).Returns(mock);
            _mockCartRepo.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            await _cartService.UpdateQuantityAsync(userId, 99, 0);

            Assert.That(cart.CartItems, Is.Empty);
        }

        [Test]
        public async Task RemoveItemAsync_ShouldDeleteItem()
        {
            var userId = "remove-me";
            var item = new CartItem { ProductId = 55 };
            var cart = new Cart { UserId = userId, CartItems = new List<CartItem> { item } };
            var mock = new List<Cart> { cart }.AsQueryable().BuildMock();
            _mockCartRepo.Setup(x => x.All()).Returns(mock);
            _mockCartItemRepo.Setup(r => r.Delete(item));
            _mockCartItemRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            await _cartService.RemoveItemAsync(userId, 55);

            Assert.That(cart.CartItems.Count, Is.EqualTo(0));
        }
        [Test]
        public async Task ClearCartAsync_ShouldRemoveAllItems()
        {
            var userId = "clear-user";
            var cart = new Cart
            {
                UserId = userId,
                CartItems = new List<CartItem>
        {
            new CartItem { ProductId = 1 },
            new CartItem { ProductId = 2 }
        }
            };

            var mock = new List<Cart> { cart }.AsQueryable().BuildMock();
            _mockCartRepo.Setup(x => x.All()).Returns(mock);

            _mockCartItemRepo.Setup(r => r.Delete(It.IsAny<CartItem>()))
                .Callback<CartItem>(item => cart.CartItems.Remove(item));

            _mockCartItemRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            await _cartService.ClearCartAsync(userId);

            Assert.That(cart.CartItems, Is.Empty);
        }


        [Test]
        public async Task RetrieveUserCartAsync_ShouldReturnAllItems()
        {
            var userId = "r-user";
            var cart = new Cart
            {
                UserId = userId,
                CartItems = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 3 },
                    new CartItem { ProductId = 2, Quantity = 1 }
                }
            };
            var mock = new List<Cart> { cart }.AsQueryable().BuildMock();
            _mockCartRepo.Setup(x => x.All()).Returns(mock);

            var result = await _cartService.RetrieveUserCartAsync(userId);

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task RetrieveUserCartAsync_ShouldReturnEmpty_WhenNoneExists()
        {
            var mock = new List<Cart>().AsQueryable().BuildMock();
            _mockCartRepo.Setup(x => x.All()).Returns(mock);

            var result = await _cartService.RetrieveUserCartAsync("nope");

            Assert.That(result, Is.Empty);
        }
    }
}
