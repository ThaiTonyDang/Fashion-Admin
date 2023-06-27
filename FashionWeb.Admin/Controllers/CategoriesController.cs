using FashionWeb.Domain.Services;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionWeb.Admin.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Create()
        {
            var categoryItemViewModel = new CategoryItemViewModel();
            var categories = await _categoryService.GetCategories();
            categories.Insert(0, new CategoryItemViewModel()
            {
                Id = default,
                Name = "No Parent Category",             
            }) ;

            var selectLists = new SelectList(categories, "Id", "Name");
            ViewData["ParentId"] = selectLists;
            return View(categoryItemViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryItemViewModel categoryItemViewModel)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var token = User.FindFirst("token").Value;
                categoryItemViewModel.Id = Guid.NewGuid();
                if (categoryItemViewModel.ParentCategoryId == default(Guid))
                {
                    categoryItemViewModel.ParentCategoryId = null;
                }    
                var result = await _categoryService.CreateCategoryAsync(categoryItemViewModel, token);
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

            var categories = await _categoryService.GetCategories();

            categories.Insert(0, new CategoryItemViewModel()
            {
                Id = default,
                Name = "Parent Category",
            });

            var selectLists = new SelectList(categories, "Id", "Name");
            ViewData["ParentId"] = selectLists;

            return View(categoryItemViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryItemViewModel categoryItemViewModel)
        {
            TempData[TEMPDATA.OPEN_MODE] = DISPLAY.OPEN_MODE;
            ModelState.Remove("File");
            var token = User.FindFirst("token").Value;
            if (ModelState.IsValid)
            {
                if (categoryItemViewModel.Id != categoryItemViewModel.ParentCategoryId)
                {
                    if (categoryItemViewModel.ParentCategoryId == default(Guid))
                    {
                        categoryItemViewModel.ParentCategoryId = null;
                    }
                    var result = await _categoryService.UpdateCategoryAsync(categoryItemViewModel, token);
                    var isSuccess = result.Item1;
                    var message = result.Item2;
                    if (isSuccess)
                    {
                        TempData[TEMPDATA.SUCCESS_MESSAGE] = $"{message}";
                        return RedirectToAction("Update", "Categories", new { id = categoryItemViewModel.Id });
                    }
                }

                TempData[TEMPDATA.FAIL_MESSAGE] = "Select The Parent Category Again";
                return RedirectToAction("Update", "Categories");

            }

            TempData[TEMPDATA.FAIL_MESSAGE] = "EDITING CATERGORY HAS BEEN FAILED";
            return RedirectToAction("Index", "Categories");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var token = ""; 
            if(User.FindFirst("token") != null)
            {
                token = User.FindFirst("token").Value;
            }

            var result = await _categoryService.DeleteCategoryAsync(id, token);
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

        private void CreateSelectItems(List<CategoryItemViewModel> source, List<CategoryItemViewModel> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("----", level));
            foreach (var category in source)
            {
                des.Add(new CategoryItemViewModel
                {
                    Id  = category.Id,
                    Name = prefix + " " + category.Name
            });
                if (category.CategoryChildrens?.Count > 0)
                {
                    CreateSelectItems(category.CategoryChildrens.ToList(), des, level + 1);
                }    
            }
        }
    }
}
