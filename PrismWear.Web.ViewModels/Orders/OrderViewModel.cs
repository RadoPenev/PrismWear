using PrismWear.Data.Models;
using PrismWear.Web.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismWear.Web.ViewModels.Orders
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string CartId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZipCode { get; set; }
        public string PaymentMethod { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }
    }
}
