﻿using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace FashionWeb.Domain.ViewModels
{
	public class ProductItemViewModel
	{
		private const string requiredString =
		"(^([0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12})$)";

		public bool IsCheck { get; set; }
		public Guid Id { set; get; }

		[Required(ErrorMessage = "NAME IS REQUIRED")]
		public string Name { set; get; }

		[Required(ErrorMessage = "PROVIDER IS REQUIRED")]
		public string Provider { set; get; }

		[Range(0, Double.MaxValue, ErrorMessage = "PRICE IS REQUIRED")]
		public decimal Price { set; get; }

		public string PriceDisplay { get => GetPriceFormat(); }

		[RegularExpression($"{requiredString}",
		ErrorMessage = "CATEGORY IS REQUIRED")]
		public Guid CategoryId { get; set; }

		[Required(ErrorMessage = "UPLOAD IMAGE IS REQUIRED")]
        public IFormFile File { get; set; }

		public string ImageName { get; set; }
		public string ImageUrl { get; set; }

		[Range(0, Double.MaxValue, ErrorMessage = "QUANTITY IN STOCK IS REQUIRED")]
		public int QuantityInStock { get; set; }

		public bool IsEnabled { get; set; }
		public string Description { get; set; }
		public List<CategoryItemViewModel> Categories { get; set; }
		public string CategoryName { get; set; }

		public string GetPriceFormat()
		{
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
			return string.Format(cultureInfo, "{0:C2}", this.Price);
		}
	}

	public class ProductViewModel
	{
		public List<ProductItemViewModel> ListProduct { get; set; }
		public string ExceptionMessage { get; set; }
		public ProductViewModel()
		{
			this.ListProduct = new List<ProductItemViewModel>();
		}
	}
}
