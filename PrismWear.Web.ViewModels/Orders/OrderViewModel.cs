using PrismWear.Web.ViewModels.Cart;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class OrderViewModel
{
    [Required]
    public string OrderId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string CartId { get; set; }

    [Range(0.0, double.MaxValue, ErrorMessage = "Subtotal must be a non-negative value.")]
    public decimal SubTotal { get; set; }

    [Range(0.0, double.MaxValue, ErrorMessage = "Shipping cost must be a non-negative value.")]
    public decimal ShippingCost { get; set; }

    [Range(0.0, double.MaxValue, ErrorMessage = "Total amount must be a non-negative value.")]
    public decimal TotalAmount { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Shipping address must be between 5 and 200 characters.")]
    public string ShippingAddress { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "City name must be between 2 and 100 characters.")]
    public string ShippingCity { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "State name must be between 2 and 100 characters.")]
    public string ShippingState { get; set; }

    [Required]
    [StringLength(10, MinimumLength = 4, ErrorMessage = "ZIP Code must be between 4 and 10 characters.")]
    public string ShippingZipCode { get; set; }


    [Required(ErrorMessage = "Cart must contain at least one item.")]
    [MinLength(1, ErrorMessage = "Cart must contain at least one item.")]
    public List<CartItemViewModel> CartItems { get; set; }
}
