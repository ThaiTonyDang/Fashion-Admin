using FashionWeb.Domain.Services;
using FashionWeb.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
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
		public IActionResult Index()
		{
			return View();
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
