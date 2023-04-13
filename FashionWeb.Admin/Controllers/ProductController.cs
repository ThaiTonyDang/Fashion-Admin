using FashionWeb.Domain.Services;
using FashionWeb.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FashionWeb.Admin.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		public ProductController(IProductService productService,
								 ICategoryService categoryService)
		{
			_categoryService = categoryService;
			_productService = productService;
		}

		public async Task<IActionResult> Index()
		{
			var productViewModel = await _productService.GetProductViewModel();
			foreach (var productItem in productViewModel.ListProduct)
			{
				productItem.Categories = await _categoryService.GetListCategoryAsync();
				var productCategory = productItem.Categories.Where(c => c.CategoryId == productItem.CategoryId)
													  .FirstOrDefault();
				if (productCategory != null)
				{
					productItem.CategoryName = productCategory.Name;
				}

			}

				return View(productViewModel);			
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var productItemViewModel = new ProductItemViewModel();
			productItemViewModel.Categories = await _categoryService.GetListCategoryAsync();
			return View(productItemViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Add(ProductItemViewModel productItemViewModel)
		{
			if (ModelState.IsValid)
			{
				productItemViewModel.Id = Guid.NewGuid();
				var result = await _productService.AddProductAsync(productItemViewModel);

				if (result)
				{
					TempData["StatusMessage"] = "SUCCESS! THIS PRODUCT HAS BEEN ADDED";
					return RedirectToAction("Add", "Product");
				}
			}

			TempData["StatusWarning"] = "FAIL! ADDING PRODUCT HAS BEEN FAILED";
			return RedirectToAction("Index", "Product");
		}
	}
}
