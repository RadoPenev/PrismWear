using Microsoft.AspNetCore.Identity;
using PrismWear.Data.Common.Models;

namespace PrismWear.Data.Models
{
    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
            this.CartItems = new HashSet<CartItem>();
            this.OrderItems = new HashSet<OrderItem>();
            this.Images=new HashSet<Image>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Size { get; set; }

        public string AddedByUser { get; set; }

        public IdentityUser User { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public virtual ICollection<Image> Images { get; set; }  

        public virtual ICollection<CartItem> CartItems { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
