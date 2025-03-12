using Microsoft.AspNetCore.Identity;
using PrismWear.Data.Common.Models;

namespace PrismWear.Data.Models
{
    public class CartItem : BaseDeletableModel<int>
    {
        public int CartId { get; set; }

        public Cart Cart { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public string Size { get; set; }
    }
}
