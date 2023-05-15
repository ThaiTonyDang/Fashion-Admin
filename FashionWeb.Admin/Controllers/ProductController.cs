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
			return View(productViewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var productItemViewModel = new ProductItemViewModel();
			return View(productItemViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm]ProductItemViewModel productItemViewModel)
		{
			var reason = ""; 
			if (ModelState.IsValid)
			{
				productItemViewModel.Id = Guid.NewGuid();
				var result = await _productService.CreateProductAsync(productItemViewModel);
				reason = result.Item3;
				if (result.Item2)
				{
					TempData["StatusMessage"] = "SUCCESS! THIS PRODUCT HAS BEEN ADDED";
					return RedirectToAction("Add", "Product");
				}
			}

			TempData["StatusWarning"] = $"ADDING FAILED ! {reason.ToUpper()}";
			return RedirectToAction("Index", "Product");
		}
	}
}
