using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PrismWear.Web.ViewModels
{
    public class CreateProductInputModel
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

        [Required(ErrorMessage = "Size is required.")]
        [StringLength(10, ErrorMessage = "The size must not exceed {1} characters.")]
        public string Size { get; set; }

        [Required(ErrorMessage = "Image path is required.")]
        public IEnumerable<IFormFile> Images { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }

    }
}
