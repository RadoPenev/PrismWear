using Microsoft.EntityFrameworkCore;
using PrismWear.Data.Common.Repositories;
using PrismWear.Data.Models;
using PrismWear.Web.ViewModels.Orders;

namespace PrismWear.Services.Data
{
    public class OrderService : IOrderService
    {
        private readonly IDeletableEntityRepository<Order> _orderRepository;
        private readonly IDeletableEntityRepository<ProductDetail> _productDetailRepository;

        public OrderService(IDeletableEntityRepository<Order> orderRepository,
            IDeletableEntityRepository<ProductDetail> productDetailRepository)
        {
            this._orderRepository = orderRepository;
            this._productDetailRepository = productDetailRepository;
        }

        public async Task<int> CreateOrderAsync(string userId, OrderInputViewModel input)
        {
            var order = new Order
            {
                UserId = userId,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                ShippingAddressLine1 = input.ShippingAddressLine1,
                ShippingCity = input.ShippingCity,
                ShippingState = input.ShippingState,
                ShippingZipCode = input.ShippingZipCode,
                ShippingCountry = input.ShippingCountry,

                OrderStatus = OrderStatus.Pending,
                OrderItems = new List<OrderItem>() 
            };

            var totalAmount = input.CartItems.Sum(x => x.Quantity * x.Product.Price);
            order.TotalAmount = (decimal)totalAmount;

            foreach (var cartItem in input.CartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = (decimal)cartItem.Product.Price,
                    Size = cartItem.Size
                };

                order.OrderItems.Add(orderItem);
            }

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return order.Id;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository
                .All()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<bool> AcceptOrderAsync(int orderId)
        {
            var order = await _orderRepository
                .All()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return false;
            }

            if (order.OrderStatus != OrderStatus.Pending)
            {
                return false;
            }

            order.OrderStatus = OrderStatus.Completed;

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HasAnyOrdersAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            return await _orderRepository
                .All()
                .AnyAsync(o => o.UserId == userId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersForUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Enumerable.Empty<Order>();
            }

            return await _orderRepository
                .All()
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedOn) 
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderUserAsync(EditOrderViewModel model, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var order = await _orderRepository
                .All()
                .FirstOrDefaultAsync(o => o.Id == model.OrderId && o.UserId == userId);

            if (order == null)
            {
                return false;
            }

            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.Email = model.Email;
            order.ShippingAddressLine1 = model.ShippingAddressLine1;
            order.ShippingCity = model.ShippingCity;
            order.ShippingState = model.ShippingState;
            order.ShippingZipCode = model.ShippingZipCode;
            order.ShippingCountry = model.ShippingCountry;

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateOrderAdminAsync(EditOrderViewModel model)
        {
            var order = await _orderRepository
                .All()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.ProductDetails)
                .FirstOrDefaultAsync(o => o.Id == model.OrderId);

            if (order == null)
            {
                return false;
            }

            var oldStatus = order.OrderStatus;
            order.OrderStatus = model.OrderStatus;

            bool transitioningToShipped = model.OrderStatus == OrderStatus.Shipped && oldStatus != OrderStatus.Shipped;
            if (transitioningToShipped)
            {
                foreach (var orderItem in order.OrderItems)
                {
                    var productDetail = orderItem.Product.ProductDetails
                        .FirstOrDefault(pd => pd.Size == orderItem.Size);

                    if (productDetail != null)
                    {
                        productDetail.Quantity -= orderItem.Quantity;
                    }
                }
            }

            if (model.OrderStatus == OrderStatus.Completed)
            {
                _orderRepository.Delete(order);
                await _orderRepository.SaveChangesAsync();
                return false;
            }

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository
                .All()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedOn)
                .ToListAsync();
        }
    }
}
