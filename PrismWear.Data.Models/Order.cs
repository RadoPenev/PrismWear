using Microsoft.AspNetCore.Identity;
using PrismWear.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PrismWear.Data.Models
{
    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }

        public decimal TotalAmount { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ShippingAddressLine1 { get; set; }


        public string ShippingCity { get; set; }

        public string ShippingState { get; set; }

        public string ShippingZipCode { get; set; }

        public string ShippingCountry { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Shipped,
        Completed
    }
}
