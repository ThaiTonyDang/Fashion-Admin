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
		public ProductsController(IProductService productService,
								 ICategoryService categoryService)
		{
			_categoryService = categoryService;
			_productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var productViewModel = await _productService.GetProductViewModelAsync();
			return View(productViewModel);
		}
	}
}
