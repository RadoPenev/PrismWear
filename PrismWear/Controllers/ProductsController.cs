using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrismWear.Data.Models;
using PrismWear.Services.Data;
using PrismWear.Web.ViewModels;
using PrismWear.Web.ViewModels.Products;
using PrismWear.Web.ViewModels.Sizes;

namespace PrismWear.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService productService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment environment;

        public ProductsController(IProductsService productService,
            ICategoriesService categoriesService,
            UserManager<IdentityUser> userManager,
             IWebHostEnvironment environment)
        {
            
            this.productService = productService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
            this.environment = environment;
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new CreateProductInputModel();
            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();

            var allSizes= new[] { "XS", "S", "M", "L", "XL" };

            viewModel.Sizes = allSizes.Select(s => new SizeQuantityViewModel
            {
                SizeName = s,
                Quantity = 0
            }).ToList();


            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateProductInputModel input)
        {
            ModelState.Remove(nameof(CreateProductInputModel.CategoriesItems));
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View(input); 
            }

            var user = await this.userManager.GetUserAsync(this.User);

            

            try
            {
                await this.productService.CreateAsync(input, user.Id, $"{this.environment.WebRootPath}/images");
                TempData["success"] = "Успешно добавихте продукт!";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }



            return this.Redirect("/");
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.productService.DeleteAsync(id);
            TempData["success"] = "Успешно изтрихте продукт!";
            return this.RedirectToAction("All");
        }



        public IActionResult All(FiltersViewModel filters, int pageNumber = 1)
        {
            const int ItemsPerPage = 8;
            var products = this.productService.GetProducts(filters, pageNumber, ItemsPerPage);
            var totalProductsCount = this.productService.GetCount();
            var viewModel = new ProductListViewModel
            {
                Products = products,
                PageNumber = pageNumber,
                ProductsCount = totalProductsCount,
                ItemsPerPage = ItemsPerPage,
                Filters = filters
            };

            return View(viewModel);
        }


        public IActionResult ById(int id)
        {
            var product = this.productService.GetById(id);

            return this.View(product);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel = this.productService.GetByIdEdit(id);
            if (viewModel == null)
            {
                return NotFound();
            }

            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,EditProductInputModel input)
        {
            ModelState.Remove(nameof(EditProductInputModel.CategoriesItems));
            if (!this.ModelState.IsValid)
            {
                input.Id = id;
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            await this.productService.EditAsync(id, input);
            TempData["success"] = "Успешно обработен продукт!";
            return this.RedirectToAction("All");
        }

    }
}
    

