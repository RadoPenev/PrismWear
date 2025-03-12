﻿using PrismWear.Data.Models;
using PrismWear.Web.ViewModels.Orders;

namespace PrismWear.Services.Data
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(string userId,OrderInputViewModel input);

        Task<Order?> GetOrderByIdAsync(int orderId);

        Task<bool> AcceptOrderAsync(int orderId);

        Task<bool> HasAnyOrdersAsync(string userId);

        Task<IEnumerable<Order>> GetAllOrdersForUserAsync(string userId);

        Task<bool> UpdateOrderAsync(EditOrderViewModel model, bool isAdmin);
    }
}
