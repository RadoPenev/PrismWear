using Microsoft.AspNetCore.Identity;
using PrismWear.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
