namespace PrismWear.Web.ViewModels.Products
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductInListViewModel> Products { get; set; }
        public bool HasPreviousPage => this.PageNumber > 1;
        public bool HasNextPage => this.PageNumber < this.PagesCount;
        public int PreviousPageNumber => this.PageNumber - 1;
        public int NextPageNumber => this.PageNumber + 1;
        public int PageNumber { get; set; }
        public int PagesCount => (int)Math.Ceiling((double)this.ProductsCount / this.ItemsPerPage);
        public int ProductsCount { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
