using PrismWear.Web.ViewModels.Sizes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismWear.Web.ViewModels.Products
{
    public class BaseProductInputModel
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "The name must be between {2} and {1} characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "The description must not exceed {1} characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public double Price { get; set; }

        public int CategoryId { get; set; }

        public List<SizeQuantityViewModel> Sizes { get; set; }
        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }
    }
}
