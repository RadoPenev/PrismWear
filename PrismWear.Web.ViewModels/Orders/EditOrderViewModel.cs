using PrismWear.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismWear.Web.ViewModels.Orders
{
    public class EditOrderViewModel
    {
        public int OrderId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ShippingAddressLine1 { get; set; }

        [Required]
        public string ShippingCity { get; set; }

        [Required]
        public string ShippingState { get; set; }

        [Required, RegularExpression(@"^\d{4}$")]
        public string ShippingZipCode { get; set; }

        [Required]
        public string ShippingCountry { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }

}
