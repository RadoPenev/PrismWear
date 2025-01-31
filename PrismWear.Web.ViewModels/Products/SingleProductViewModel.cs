using PrismWear.Web.ViewModels.Sizes;

namespace PrismWear.Web.ViewModels.Products
{
    public class SingleProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Size { get; set; }

        public string? ImageUrl { get; set; }

        public double Price { get; set; }

        public string CategoryName { get; set; }

        public List<ProductDetailViewModel> Details { get; set; }


    }
}
