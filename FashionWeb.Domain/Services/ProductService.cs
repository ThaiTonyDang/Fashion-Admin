using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.Services.HttpClients;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace FashionWeb.Domain.Services
{
    public class ProductService : IProductService
	{
		private readonly IHttpClientService _urlService;
		private readonly IFileService _fileService;
		private readonly HttpClient _httpClient;
		public string[] _exceptionMessage;
		public int _statusCode;
		public bool _isSuccess;
		public ProductService(IHttpClientService urlService, IFileService fileService, HttpClient httpClient)
		{
			_urlService = urlService;
			_httpClient = httpClient;
			_fileService = fileService;
		}

		public async Task<ProductViewModel> GetProductViewModel()
		{
			var productViewModel = new ProductViewModel();
			productViewModel.ListProduct = await GetListProducts();

			productViewModel.ExceptionMessage = _exceptionMessage;
			productViewModel.StatusCode = HttpStatusCode.Accepted;
			productViewModel.IsSuccess = _isSuccess;
			
			return productViewModel;
		}

		public async Task<List<ProductItemViewModel>> GetListProducts()
		{
			try
			{
				var apiUrl = _urlService.GetBaseUrl() + "/api/products";
				var response = await _httpClient.GetAsync(apiUrl);

				var responseList = JsonConvert.DeserializeObject<ResponseApiData<List<ProductItemViewModel>>>
								   (await response.Content.ReadAsStringAsync());
				_isSuccess = responseList.IsSuccess;
			    _exceptionMessage = new string[] { };
				_statusCode = responseList.StatusCode;

				var products = responseList.Data;
				if (products != null)
				{
                    foreach (var product in products)
                    {
                        product.ImageUrl = _urlService.GetFileApiUrl(product.ImageName);
                    }
                }	
				
				return products;
			}
			catch(Exception exception)
			{
				_exceptionMessage = new string[] { exception.Message };
				_statusCode = (int)HttpStatusCode.ServiceUnavailable;

                return null;
			}
        }

		public async Task<Tuple<bool, string>> CreateProductAsync(ProductItemViewModel productItemViewModel)
		{
            var responseMessage = "";
			var message = "";
			if (productItemViewModel != null)
			{
                var file = productItemViewModel.File;   
                var result = await _fileService.GetResponeUploadFileAsync(file, _httpClient);
                var responseUploadFileList = result.Item1;
                if (responseUploadFileList != null)
                {
                    productItemViewModel.ImageName = responseUploadFileList.Data[0];
                    productItemViewModel.ImageUrl = responseUploadFileList.Data[1];
                }
				responseMessage = result.Item2;
                try
                {
					var apiUrl = _urlService.GetBaseUrl() + "/api/products";
					var response = await _httpClient.PostAsJsonAsync(apiUrl, productItemViewModel);
					var responseList = JsonConvert.DeserializeObject<ResponseApiData<ProductItemViewModel>>
									   (await response.Content.ReadAsStringAsync());
					message = responseList.Message;

					if (responseList.IsSuccess)
					{
						return Tuple.Create(true, message + " ! " + responseMessage);
					}
				}
				catch (Exception exception)
				{
					message = exception.Message + " ! " + "Create Product Fail !";
                    return Tuple.Create(false, responseMessage + message);
				}
			}
	
			return Tuple.Create(false, message);
        }

        public async Task<Tuple<bool, string>> UpdateProductAsync(ProductItemViewModel productItemViewModel)
        {
            var responseMessage = "";
            var message = "";         
			var link = "";
			var fileName = DISPLAY.IMAGE_PATH;
			var file = productItemViewModel.File;
			if (file != null )
			{
				var result = await _fileService.GetResponeUploadFileAsync(file, _httpClient);
				var responseUploadFileList = result.Item1;
				if (responseUploadFileList != null)
				{
					fileName = responseUploadFileList.Data[0];
					link = responseUploadFileList.Data[1];
				}
				responseMessage = result.Item2;
			}
			if (!string.IsNullOrEmpty(productItemViewModel.ImageName) && file == null )
			{
				fileName = productItemViewModel.ImageName;							
			}
			productItemViewModel.ImageName = fileName;
			link = _urlService.GetFileApiUrl(fileName);
			productItemViewModel.ImageUrl = link;

			try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/products";
                var response = await _httpClient.PutAsJsonAsync(apiUrl, productItemViewModel);
                var responseList = JsonConvert.DeserializeObject<ResponseApiData<ProductItemViewModel>>
                                    (await response.Content.ReadAsStringAsync());
                message = responseList.Message;
                return Tuple.Create(responseList.IsSuccess, message + " ! " + responseMessage);                  
            }
            catch (Exception exception)
            {
                message = exception.Message + " ! " + "Updated Product Fail !";
                return Tuple.Create(false, responseMessage + message);
            }
        }
        public async Task<Tuple<ProductItemViewModel, string>> GetProductByIdAsync(string productId)
        {
			var message = "";
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/products/";
                var response = await _httpClient.GetAsync(apiUrl + productId);
                var responseList = JsonConvert.DeserializeObject<ResponseApiData<ProductItemViewModel>>
                                   (await response.Content.ReadAsStringAsync());
                var productDto = responseList.Data;
                message = responseList.Message;
               
                productDto.ImageUrl = _urlService.GetFileApiUrl(productDto.ImageName);
                return Tuple.Create(productDto, message);
                
            }
            catch (Exception exception)
            {
                message = exception.Message + " ! " + "Get Product Fail !";
                return Tuple.Create(default(ProductItemViewModel), message);
            }
        }

        public async Task<Tuple<bool, string>> DeleteProductAsync(string productId)
        {
            var message = "";
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/products/";
                var response = await _httpClient.DeleteAsync(apiUrl + productId);
                var responseList = JsonConvert.DeserializeObject<ResponseApiData<ProductItemViewModel>>
                                   (await response.Content.ReadAsStringAsync());
                message = responseList.Message;
                return Tuple.Create(responseList.IsSuccess, message);
            }
            catch (Exception exception)
            {
                message = exception.Message + " ! " + "Deleted Product Fail !";
                return Tuple.Create(false, message);
            }
        }
    }  
}