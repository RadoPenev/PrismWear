using PrismWear.Data.Models;

namespace PrismWear.Web.ViewModels.Cart
{
    public class CartItemViewModel
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public string CategoryName { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
