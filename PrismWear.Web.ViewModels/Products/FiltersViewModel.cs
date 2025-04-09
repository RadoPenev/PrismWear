using System.ComponentModel.DataAnnotations;

namespace PrismWear.Web.ViewModels.Products
{
    public class FiltersViewModel
    {
        public int CategoryId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum price must be a non-negative number.")]
        public double? MinPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maximum price must be a non-negative number.")]
        public double? MaxPrice { get; set; }
    }
}
