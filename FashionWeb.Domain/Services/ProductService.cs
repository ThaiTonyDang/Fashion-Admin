using FashionWeb.Domain.Config;
using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.HostConfig;
using FashionWeb.Domain.Model;
using FashionWeb.Domain.Model.Files;
using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.Services.HttpClients;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FashionWeb.Domain.Services
{
    public class ProductService : IProductService
	{
		private readonly IFileService _fileService;
        private readonly IHttpClientService _httpClientService;
        private readonly int _pageSize;
        private readonly HttpClient _httpClient;
        private const string _apiResource = "products";
        public ProductService(IHttpClientService httpClientService, IFileService fileService, IOptions<PageConfig> pageOptions)
		{
			_fileService = fileService;
            _pageSize = pageOptions.Value.PageSize;
            _httpClientService = httpClientService;

        }

        public async Task<ResultDto> GetProductsAsync(int currentPage)
        {
            var productsTask = Task.Run(() => GetListProductAsync(currentPage));
            var countTask = Task.Run(() => GetTotalItemsAsync());

            await Task.WhenAll(new Task[] { productsTask, countTask });

            var productsResponse = await productsTask;
            var countResponse = await countTask;
            if (!productsResponse.IsSuccess)
            {
                return productsResponse;
            }

            if (!countResponse.IsSuccess)
            {
                return countResponse;
            }

            var products = productsResponse.ToSuccessDataResult<List<ProductItemViewModel>>().Data;
            var totalProducts = countResponse.ToSuccessDataResult<int>().Data;

            var productViewModel = new ProductViewModel()
            {
                ListProduct = products,
                Paging = new Paging(currentPage, _pageSize, totalProducts)
            };

            return new SuccessDataResult<ProductViewModel>(productsResponse.Message, productViewModel);
        }

        public async Task<ResultDto> GetProductByIdAsync(string productId)
        {
            var apiUrl = $"{_apiResource}/{productId}";
            var response = await _httpClientService.GetDataAsync<ProductItemViewModel>(apiUrl);
            return response;
        }

        public async Task<ResultDto> CreateProductAsync(ProductItemViewModel productItemViewModel, string token)
		{
            var file = productItemViewModel.File;
            if (file != null)
            {
                var content = new MultipartFormDataContent
                {
                    {
                        new StreamContent(file.OpenReadStream())
                        {
                            Headers =
                            {
                                ContentLength = file.Length,
                                ContentType = new MediaTypeHeaderValue(file.ContentType)
                            }
                        },
                        "File",
                        file.FileName
                    }
                };

                var fileResult = await _fileService.UploadFileAsync(content, token);
                if (!fileResult.IsSuccess)
                {
                    var fileLinks = fileResult.ToSuccessDataResult<FileUpload>().Data;
                    productItemViewModel.MainImageName = fileLinks.FileName;
                    productItemViewModel.ImageUrl = fileLinks.FileLink;
                } 
            }

            var apiUrl = $"{_apiResource}";
            var response = await _httpClientService.PostAsync(productItemViewModel, apiUrl);
            return response;
        }

        public async Task<Tuple<bool, string>> UpdateProductAsync(ProductItemViewModel productItemViewModel, string token)
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
			if (!string.IsNullOrEmpty(productItemViewModel.MainImageName) && file == null )
			{
				fileName = productItemViewModel.MainImageName;							
			}
			productItemViewModel.MainImageName = fileName;
            link = _urlService.GetFileApiUrl(fileName);
            productItemViewModel.ImageUrl = link;

			try
            {
                var apiUrl = "";
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var response = await _httpClient.PutAsJsonAsync(apiUrl, productItemViewModel);
                var responseList = JsonConvert.DeserializeObject<ResponseDataApi<List<ProductItemViewModel>>>
                                    (await response.Content.ReadAsStringAsync());
                message = responseList.Message;
				var data = responseList.Data;
                return Tuple.Create(responseList.IsSuccess, message + " ! " + responseMessage);                  
            }
            catch (Exception exception)
            {
                message = exception.Message + " ! " + "Updated Product Fail !";
                return Tuple.Create(false, responseMessage + message);
            }
        }
        
        public async Task<Tuple<bool, string>> DeleteProductAsync(string productId)
        {
            var message = "";
            try
            {
                var apiUrl =  "/api/products/";
                var response = await _httpClient.DeleteAsync(apiUrl + productId);
                var responseList = JsonConvert.DeserializeObject<ResponseDataApi<ProductItemViewModel>>
                                   (await response.Content.ReadAsStringAsync());
                message = responseList.Message;
				var data = responseList.Data;
                return Tuple.Create(responseList.IsSuccess, message);
            }
            catch (Exception exception)
            {
                message = exception.Message + " ! " + "Deleted Product Fail !";
                return Tuple.Create(false, message);
            }
        }

        private async Task<ResultDto> GetListProductAsync(int currentPage)
        {
            var apiUrl = $"{_apiResource}?currentpage={currentPage}&pagesize={_pageSize}";
            var response = await _httpClientService.GetDataAsync<List<ProductItemViewModel>>(apiUrl);

            if (!response.IsSuccess)
            {
                return response;
            }

            var products = response.ToSuccessDataResult<List<ProductItemViewModel>>().Data;
            products.Select(product =>
            {
                product.ImageUrl = _httpClientService.GetFileApiUrl(product.MainImageName);
                return product;
            });

            return new SuccessDataResult<List<ProductItemViewModel>>(response.Message, products);
        }

        private async Task<ResultDto> GetTotalItemsAsync()
        {
            var apiUrl = $"{_apiResource}/count";
            var response = await _httpClientService.GetDataAsync<int>(apiUrl);
            return response;
        }
    }  
}