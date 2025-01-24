using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrismWear.Services.Data;
using PrismWear.Web.ViewModels;
using PrismWear.Web.ViewModels.Categories;
using PrismWear.Web.ViewModels.Products;

namespace PrismWear.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<IdentityUser> userManager;

        public CategoriesController(ICategoriesService categoriesService,
            UserManager<IdentityUser> userManager)
        {
            this.categoriesService = categoriesService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new CreateCategoryInputModel();
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.categoriesService.CreateAsync(input, user.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return this.Redirect("/");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = this.categoriesService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new EditCategoryInputModel();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Id = id;
                return this.View(input);
            }

            await this.categoriesService.EditAsync(id, input);
            return this.RedirectToAction("All");
        }

        public IActionResult All()
        {
            var keyValuePairs = this.categoriesService.GetAllAsKeyValuePairs();

            var model = keyValuePairs
                .Select(kvp => new CategoryViewModel
                {
                    Id = int.Parse(kvp.Key),
                    Name = kvp.Value
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.categoriesService.DeleteAsync(id);
            return this.RedirectToAction("All");
        }
    }
}
