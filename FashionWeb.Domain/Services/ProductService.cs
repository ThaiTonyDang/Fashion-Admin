using FashionWeb.Domain.HostConfig;
using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Services
{
	public class ProductService : IProductService
	{
		private readonly HttpClient _httpClient;
		private readonly APIConfig _hostAPIConfig;
		public string _exceptionMessage;
		public ProductService(HttpClient httpClient, IOptions<APIConfig> options)
		{
			_httpClient = httpClient;
			_hostAPIConfig = options.Value;
		}

		public async Task<ProductViewModel> GetProductViewModel()
		{
			var productViewModel = new ProductViewModel();
			productViewModel.ListProduct = await GetListProducts();
			if (productViewModel.ListProduct == null)
			{
				productViewModel.ExceptionMessage = _exceptionMessage;
			}	

			return productViewModel;
		}

		public async Task<List<ProductItemViewModel>> GetListProducts()
		{
			try
			{
				var apiUrl = _hostAPIConfig.Url + "api/products";
				var response = await _httpClient.GetAsync(apiUrl);

				var responseList = JsonConvert.DeserializeObject<ResponseAPI<List<ProductItemViewModel>>>
								   (await response.Content.ReadAsStringAsync());

				var isSuccess = responseList.Success;
				if (!isSuccess)
				{
					return new List<ProductItemViewModel>();
				}	

				var products = responseList.Data;

				foreach (var product in products)
				{
					product.ImageUrl = GetFileUrl(product.ImageName);
				}

				return products;
			}
			catch(Exception exception)
			{
				_exceptionMessage = exception.Message;
				return null;
			}
			
		}

		private string GetFileUrl(string fileName)
		{
			var fileUrl = _hostAPIConfig.Url + "resource/" + fileName;
			return fileUrl;
		}
	}  
}