using PrismWear.Data.Models;

namespace PrismWear.Web.ViewModels.Products
{
    public class ProductInListViewModel
    {
        public int Id { get; set; }
        public ICollection<Image> Images { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}
