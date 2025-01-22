using Microsoft.AspNetCore.Identity;
using PrismWear.Data.Common.Models;

namespace PrismWear.Data.Models
{
    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public string UserId { get; set; }

        public  IdentityUser User { get; set; }

        public decimal TotalAmmount { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
