using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.ViewModels
{
	public class ProductItemViewModel
	{
		public bool IsCheck { get; set; }
		public Guid Id { set; get; }

		[Required(ErrorMessage = "NAME IS REQUIRED")]
		public string Name { set; get; }

		public string Provider { set; get; }

		[Range(0, Double.MaxValue, ErrorMessage = "PRICE IS REQUIRED")]
		public decimal Price { set; get; }

		public string PriceDisplay { get => GetPriceFormat(); }

	    [Required(ErrorMessage = "CATEGORY IS REQUIRED")]
        public Guid CategoryId { get; set; }

		[Required(ErrorMessage = "UPLOAD IMAGE IS REQUIRED")]
		public IFormFile Image { get; set; }

		public string ImageName { get; set; }
		public string ImageUrl { get; set; }

		public int QuantityInStock { get; set; }

		public bool IsEnabled { get; set; }
		public string Description { get; set; }
		public List<CategoryItemViewModel> Categories { get; set; }
		public string CategoryName { get; set; }

		public string GetPriceFormat()
		{
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
			return string.Format(cultureInfo, "{0:C0}", this.Price);
		}
	}

	public class ProductViewModel
	{
		public List<ProductItemViewModel> ListProduct { get; set; }
		public string[] ExceptionMessage { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public ProductViewModel()
		{
			this.ListProduct = new List<ProductItemViewModel>();
		}
	}
}
