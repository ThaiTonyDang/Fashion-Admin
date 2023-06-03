using FashionWeb.Domain.Services;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FashionWeb.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("/categories")]
        public async Task<IActionResult> Index()
        {
            var categoryViewModel = await _categoryService.GetCategoryViewModel();

            return View(categoryViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categoryItemViewModel = new CategoryItemViewModel();
            return View(categoryItemViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryItemViewModel categoryItemViewModel)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                categoryItemViewModel.Id = Guid.NewGuid();
                var result = await _categoryService.CreateCategoryAsync(categoryItemViewModel);
                var isSuccess = result.Item1;
                message = result.Item2;
                TempData[TEMPDATA.OPEN_MODE] = DISPLAY.OPEN_MODE;
                if (isSuccess)
                {
                    TempData[TEMPDATA.SUCCESS_MESSAGE] = $"{message}";
                    return RedirectToAction("Create", "Categories");
                }
            }

            TempData[TEMPDATA.FAIL_MESSAGE] = $"{message}";
            return RedirectToAction("Create", "Categories");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            var categoryItemViewModel = result.Item1;

            return View(categoryItemViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryItemViewModel categoryItemViewModel)
        {
            ModelState.Remove("File");
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateCategoryAsync(categoryItemViewModel);
                var isSuccess = result.Item1;
                var message = result.Item2;
                TempData[TEMPDATA.OPEN_MODE] = DISPLAY.OPEN_MODE;
                if (isSuccess)
                {
                    TempData[TEMPDATA.SUCCESS_MESSAGE] = $"{message}";
                    return RedirectToAction("Update", "Categories", new { id = categoryItemViewModel.Id });
                }
            }

            TempData[TEMPDATA.FAIL_MESSAGE] = "EDITING CATERGORY HAS BEEN FAILED";
            return RedirectToAction("Index", "Categories");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            var isSuccess = result.Item1;
            var message = result.Item2;
            TempData[TEMPDATA.OPEN_MODE] = DISPLAY.OPEN_MODE;
            if (isSuccess)
            {
                TempData[TEMPDATA.SUCCESS_MESSAGE] = $"{message}";
                return RedirectToAction("Index", "Categories");
            }

            TempData[TEMPDATA.FAIL_MESSAGE] = $"{message}";
            return RedirectToAction("Index", "Categories");
        }
    }
}
