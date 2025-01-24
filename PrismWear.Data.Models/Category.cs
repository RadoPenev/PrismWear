using Microsoft.AspNetCore.Identity;
using PrismWear.Data.Common.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWear.Data.Models
{
    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        public string AddedByUser { get; set; }

        [NotMapped]
        public IdentityUser User { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
