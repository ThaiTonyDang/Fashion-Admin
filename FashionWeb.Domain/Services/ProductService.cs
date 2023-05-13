using FashionWeb.Domain.HostConfig;
using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace FashionWeb.Domain.Services
{
	public class ProductService : IProductService
	{
		private readonly HttpClient _httpClient;
		private readonly APIConfig _hostAPIConfig;
		public string[] _exceptionMessage;
		public ProductService(HttpClient httpClient, IOptions<APIConfig> options)
		{
			_httpClient = httpClient;
			_hostAPIConfig = options.Value;
		}

		public async Task<ProductViewModel> GetProductViewModelAsync()
		{
            var productViewModel = new ProductViewModel();
            productViewModel.ListProduct = null;
            var res = await GetResponseListProductsAsync();
                                      			
			if (res == null)
			{
                productViewModel.ExceptionMessage = _exceptionMessage;
				productViewModel.StatusCode = HttpStatusCode.ServiceUnavailable;
            }

            if (res != null)
            {
                if (!res.Success)
                {
                    return new ProductViewModel()
                    {
                        StatusCode = res.StatusCode,
                        ExceptionMessage = res.ErrorsDetail
                    };
                }    
               
                productViewModel.ListProduct = res.Data;
            }

            return productViewModel;
		}

		public async Task<ResponseAPI<List<ProductItemViewModel>>> GetResponseListProductsAsync()
		{
			try
			{
                var apiUrl = _hostAPIConfig.Url + "api/products";
                var response = await _httpClient.GetAsync(apiUrl);

                var responseList = JsonConvert.DeserializeObject<ResponseAPI<List<ProductItemViewModel>>>
                                   (await response.Content.ReadAsStringAsync());
                if (!responseList.Success)
                {
                    return responseList;
                }
                var products = responseList.Data;

				if (products != null)
				{
                    foreach (var product in products)
                    {
                        product.ImageUrl = GetFileUrl(product.ImageName);
                    }
                }	
                
				return responseList;
            }
			catch (Exception exception)
			{
				_exceptionMessage = new string[] { exception.InnerException.Message };
				return default; 
            }
        }

		private string GetFileUrl(string fileName)
		{
			var fileUrl = _hostAPIConfig.Url + "resource/" + fileName;
			return fileUrl;
		}
	}  
}