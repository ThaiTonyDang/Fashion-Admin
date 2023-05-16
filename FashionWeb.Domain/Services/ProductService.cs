using FashionWeb.Domain.HostConfig;
using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace FashionWeb.Domain.Services
{
	public class ProductService : IProductService
	{
		private readonly IUrlService _urlService;
		private readonly IFileService _fileService;
		private readonly HttpClient _httpClient;
		public string _exceptionMessage;
		public ProductService(IUrlService urlService, IFileService fileService, HttpClient httpClient)
		{
			_urlService = urlService;
			_httpClient = httpClient;
			_fileService = fileService;
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
				var apiUrl = _urlService.GetBaseUrl() + "api/products";
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
					product.ImageUrl = _urlService.GetFileApiUrl(product.ImageName);
				}

				return products;
			}
			catch(Exception exception)
			{
				_exceptionMessage = exception.Message;
				return null;
			}
        }

		public async Task<Tuple<bool, string>> CreateProductAsync(ProductItemViewModel productItemViewModel)
		{
			var message = "";
			try
			{
				if (productItemViewModel != null)
				{
                    productItemViewModel.ImageName = DISPLAY.IMAGE_PATH ;
					var file = productItemViewModel.File;
					if (file != null)
					{
                        var imageData = await _fileService.UploadFileAsync(file, _httpClient);
						var imageName = imageData[0];
						var imageLink = imageData[1];
						if (imageData != null)
						{
							productItemViewModel.ImageName = imageData[0];
							productItemViewModel.ImageUrl = imageData[1];
						}
                    }
                  
                    var apiUrl = _urlService.GetBaseUrl() + "api/products";
                    var response = await _httpClient.PostAsJsonAsync(apiUrl, productItemViewModel);
                    var responseList = JsonConvert.DeserializeObject<ResponseAPI<ProductItemViewModel>>
						               (await response.Content.ReadAsStringAsync());
					message = responseList.Message;

                    if (responseList.Success)
					{
						return Tuple.Create(true, message);
					}
				}
			}
			catch (Exception exception)
			{

                return Tuple.Create(false, exception.Message);
            }
	
			return Tuple.Create(false, message);
        }
	}  
}