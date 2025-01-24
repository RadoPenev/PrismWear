using System.ComponentModel.DataAnnotations;

namespace PrismWear.Web.ViewModels.Categories
{
    public class CreateCategoryInputModel
    {

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(10, ErrorMessage = "The name must be between {2} and {1} characters.", MinimumLength = 2)]
        public string Name { get; set; }
    }
}
