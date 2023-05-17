using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.Services;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FashionWeb.Admin.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		public ProductController(IProductService productService, ICategoryService categoryService)
		{
			_categoryService = categoryService;
			_productService = productService;
		}

		[HttpGet]
		[Route("/products")]
		public async Task<IActionResult> Index()
		{
			var productViewModel = await _productService.GetProductViewModel();
			foreach (var productItemViewModel in productViewModel.ListProduct)
			{
                productItemViewModel.Categories = await _categoryService.GetListCategories();
                foreach (var category in productItemViewModel.Categories)
                {
                    productItemViewModel.CategoryName = category.Name;
                }
            }             
            return View(productViewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var productItemViewModel = new ProductItemViewModel();
			productItemViewModel.Categories = await _categoryService.GetListCategories();
			foreach (var category in productItemViewModel.Categories)
			{
				productItemViewModel.CategoryName = category.Name;
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
				if (isSuccess)
				{
					TempData["StatusMessage"] = $"{message}";
					return RedirectToAction("Create", "Product");
				}
			}

			TempData["StatusWarning"] = $"{message}";
			return RedirectToAction("Create", "Product");
		}

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
         //   var productItemViewModel =  await _productService.GetProductItemByIdAsync(new Guid(id));
        	//productItemViewModel.Categories = await _categoryService.GetListCategoryAsync();
        	return View();
        }
    }
}