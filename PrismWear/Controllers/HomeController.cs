using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PrismWear.Services.Data;
using PrismWear.Web.ViewModels;
using PrismWear.Web.ViewModels.Products;

namespace PrismWear.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsService productService;

        public HomeController(ILogger<HomeController> logger,
            IProductsService productService)
        {
            _logger = logger;
            this.productService = productService;
        }

        public IActionResult Index()
        {
            var productsFromDb = this.productService.GetAll();
            // or however you load products, e.g. .GetFeatured() etc.

            var viewModel = new ProductListViewModel
            {
                Products = productsFromDb,
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
