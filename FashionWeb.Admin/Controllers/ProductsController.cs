using FashionWeb.Domain.Services;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionWeb.Admin.Controllers
{ 
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("/products")]
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var productViewModel = await _productService.GetPagingProductViewModel(currentPage);
          
            return View(productViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var productItemViewModel = new ProductItemViewModel();
            var categories = await _categoryService.GetCategories();
            productItemViewModel.Categories = categories;

            foreach (var item in productItemViewModel.Categories)
            {
                item.Id = default;
            }

            var items = new List<CategoryItemViewModel>();
            CreateSelectItems(categories, items, 0);

            var selectLists = new SelectList(items, "Id", "Name");
            ViewData["ParentId"] = selectLists;

            return View(productItemViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductItemViewModel productItemViewModel)
        {
            TempData[TEMPDATA.OPEN_MODE] = DISPLAY.OPEN_MODE;
            var message = "";
            var token = User.FindFirst("token").Value;
            if (ModelState.IsValid)
            {
                if (productItemViewModel.CategoryId == default)
                {
                    TempData[TEMPDATA.FAIL_MESSAGE] = "CHOOSE CATEGORY AGAIN !";
                    return RedirectToAction("Create", "Products");
                }    
                productItemViewModel.Id = Guid.NewGuid();
                var result = await _productService.CreateProductAsync(productItemViewModel, token);
                var isSuccess = result.Item1;
                message = result.Item2;              
                if (isSuccess)
                {
                    TempData[TEMPDATA.SUCCESS_MESSAGE] = message;
                    return RedirectToAction("Create", "Products");
                }
            }

            TempData[TEMPDATA.FAIL_MESSAGE] = message;
            return RedirectToAction("Create", "Products");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            var productItemViewModel = result.Item1;
        
            productItemViewModel.Categories = await _categoryService.GetCategories();
            

            var items = new List<CategoryItemViewModel>();
            CreateSelectItems(productItemViewModel.Categories, items, 0);

            var selectLists = new SelectList(items, "Id", "Name");
            ViewData["ParentId"] = selectLists;

            return View(productItemViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(ProductItemViewModel productItemViewModel)
        {
            ModelState.Remove("File");
            var token = User.FindFirst("token").Value;
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProductAsync(productItemViewModel, token);
                var isSuccess = result.Item1;
                var message = result.Item2;
                TempData[TEMPDATA.OPEN_MODE] = DISPLAY.OPEN_MODE;
                if (isSuccess)
                {
                    TempData[TEMPDATA.SUCCESS_MESSAGE] = $"{message}";
                    return RedirectToAction("Update", "Products", new { id = productItemViewModel.Id });
                }
            }

            TempData[TEMPDATA.FAIL_MESSAGE] = "EDITING PRODUCT HAS BEEN FAILED";
            return RedirectToAction("Index", "Products");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _productService.DeleteProductAsync(id);
            var isSuccess = result.Item1;
            var message = result.Item2;
            TempData[TEMPDATA.OPEN_MODE] = DISPLAY.OPEN_MODE;
            if (isSuccess)
            {
                TempData[TEMPDATA.SUCCESS_MESSAGE] = $"{message}";
                return RedirectToAction("Index", "Products");
            }

            TempData[TEMPDATA.FAIL_MESSAGE] = $"{message}";
            return RedirectToAction("Index", "Products");
        }

        private void CreateSelectItems(List<CategoryItemViewModel> source, List<CategoryItemViewModel> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("----", level));
            foreach (var category in source)
            {
                category.Name = prefix + " " + category.Name;
                des.Add(category);
                if (category.CategoryChildrens?.Count > 0)
                {
                    CreateSelectItems(category.CategoryChildrens.ToList(), des, level + 1);
                }
            }
        }
    }
}