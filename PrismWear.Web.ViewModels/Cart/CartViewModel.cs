namespace PrismWear.Web.ViewModels.Cart
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

        public double Subtotal => Items.Sum(x => x.Price * x.Quantity);
        public double ShippingCost { get; set; }
        public double Total => Subtotal + ShippingCost;
    }
}
