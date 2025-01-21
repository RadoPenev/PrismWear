using Microsoft.AspNetCore.Identity;
using PrismWear.Data.Common.Models;

namespace PrismWear.Data.Models
{
    public class Cart : BaseDeletableModel<int>
    {

        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}
