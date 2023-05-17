using FashionWeb.Domain.HostConfig;
using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Json;

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
					var apiUrl = _urlService.GetBaseUrl() + "api/products";
					var response = await _httpClient.PostAsJsonAsync(apiUrl, productItemViewModel);
					var responseList = JsonConvert.DeserializeObject<ResponseAPI<ProductItemViewModel>>
									   (await response.Content.ReadAsStringAsync());
					message = responseList.Message;

					if (responseList.Success)
					{
						return Tuple.Create(true, message + " ! " + responseMessage);
					}
				}
				catch (Exception exception)
				{
					message = exception.InnerException.Message + "Create Product Fail !";
                    return Tuple.Create(false, responseMessage + message);
				}
			}
	
			return Tuple.Create(false, message);
        }

        public Task<Tuple<bool, string>> EditProductAsync(ProductItemViewModel productItemViewModel)
        {
            throw new NotImplementedException();
        }
    }  
}