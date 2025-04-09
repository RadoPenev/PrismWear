namespace PrismWear.Web.ViewModels.Products
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductInListViewModel> Products { get; set; }
        public FiltersViewModel Filters { get; set; }
        public bool HasPreviousPage => this.PageNumber > 1;
        public bool HasNextPage => this.PageNumber < this.PagesCount;
        public int PreviousPageNumber => this.PageNumber - 1;
        public int NextPageNumber => this.PageNumber + 1;
        public int PageNumber { get; set; }
        public int PagesCount =>
      ItemsPerPage > 0
          ? (int)Math.Ceiling((double)this.ProductsCount / this.ItemsPerPage)
          : 0;
        public int ProductsCount { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
