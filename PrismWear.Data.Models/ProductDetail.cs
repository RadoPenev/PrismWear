using PrismWear.Data.Common.Models;

namespace PrismWear.Data.Models
{
    public class ProductDetail : BaseDeletableModel<int>
    {
        public string Size { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
