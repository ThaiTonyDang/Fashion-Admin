using FashionWeb.Domain.Services;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
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

        [HttpGet]
        [Route("/products")]
        public async Task<IActionResult> Index()
        {
            var productViewModel = await _productService.GetProductViewModel();
            var categoryViewModel = await _categoryService.GetCategoryViewModel();
            var listCategory = categoryViewModel.ListCategory;
            if (productViewModel.IsSuccess)
            {
                foreach (var productItemViewModel in productViewModel.ListProduct)
                {
                    if (listCategory == null)
                    {
                        productItemViewModel.CategoryName = categoryViewModel.ExceptionMessage;
                        break;
                    }
                    var category = listCategory.Where(c => c.Id == productItemViewModel.CategoryId).FirstOrDefault();
                    productItemViewModel.CategoryName = category.Name;
                }
            }

            return View(productViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var productItemViewModel = new ProductItemViewModel();
            var categories = await _categoryService.GetListCategories();
            productItemViewModel.Categories = categories;
            if (categories != null)
            {
                foreach (var category in categories)
                {
                    productItemViewModel.CategoryName = category.Name;
                }
            }

            return View(productItemViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductItemViewModel productItemViewModel)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                productItemViewModel.Id = Guid.NewGuid();
                var result = await _productService.CreateProductAsync(productItemViewModel);
                var isSuccess = result.Item1;
                message = result.Item2;
                TempData[TEMPDATA.OPEN_MODE] = DISPLAY.OPEN_MODE;
                if (isSuccess)
                {
                    TempData[TEMPDATA.SUCCESS_MESSAGE] = $"{message}";
                    return RedirectToAction("Create", "Products");
                }
            }

            TempData[TEMPDATA.FAIL_MESSAGE] = $"{message}";
            return RedirectToAction("Create", "Products");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            var productItemViewModel = result.Item1;
            if (productItemViewModel != null)
            {
                productItemViewModel.Categories = await _categoryService.GetListCategories();
            }

            return View(productItemViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductItemViewModel productItemViewModel)
        {
            ModelState.Remove("File");
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProductAsync(productItemViewModel);
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

    }
}