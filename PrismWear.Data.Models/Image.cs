using Microsoft.AspNetCore.Identity;
using PrismWear.Data.Common.Models;

namespace PrismWear.Data.Models
{
    public class Image : BaseDeletableModel<string>
    {
        public Image() 
        {
            this.Id=Guid.NewGuid().ToString();
        }

        public int ProductId { get; set; }

        public  Product Product { get; set; }

        public string Extension { get; set; }

        public string AddedByUser { get; set; }

        public IdentityUser User { get; set; }

    }
}
