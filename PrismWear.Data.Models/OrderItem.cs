using PrismWear.Data.Common.Models;

namespace PrismWear.Data.Models
{
    public class OrderItem : BaseDeletableModel<int>
    {
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }


    }
}
