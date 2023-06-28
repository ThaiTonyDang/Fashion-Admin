using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.Services.HttpClients;
using FashionWeb.Domain.ViewModels;
using FashionWeb.Utilities.GlobalHelpers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace FashionWeb.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientService _urlService;
        private readonly IFileService _fileService;
        private readonly HttpClient _httpClient;
        public string _exceptionMessage;
        public bool _isSuccess;
        public CategoryService(IHttpClientService urlService, IFileService fileService, HttpClient httpClient)
        {
            _urlService = urlService;
            _fileService = fileService;
            _httpClient = httpClient;
        }

        public async Task<List<CategoryItemViewModel>> GetCategories()
        {
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/categories";
                var response = await _httpClient.GetAsync(apiUrl);

                var responseList = JsonConvert.DeserializeObject<ResponseApiData<List<CategoryItemViewModel>>>
                                   (await response.Content.ReadAsStringAsync());

                _isSuccess = responseList.IsSuccess;
                var categories = responseList.Data;

                foreach (var category in categories)
                {
                    category.ImageUrl = _urlService.GetFileApiUrl(category.ImageName);
                    foreach (var child in category.CategoryChildrens)
                    {
                        child.ImageUrl = _urlService.GetFileApiUrl(child.ImageName);
                    }                 
                }

                return categories;
            }
            catch (Exception exception)
            {
                _exceptionMessage = exception.Message;
                return null;
            }
        }

        public async Task<CategoryViewModel> GetCategoryViewModel()
        {
            var categoryViewModel = new CategoryViewModel();
            categoryViewModel.ListCategory = await GetCategories();
            categoryViewModel.IsSuccess = _isSuccess;

            if (categoryViewModel.ListCategory == null)
            {
                categoryViewModel.ExceptionMessage = _exceptionMessage;
            }

            return categoryViewModel;
        }

        public async Task<Tuple<bool, string>> CreateCategoryAsync(CategoryItemViewModel categoryItemViewModel, string token)
        {
            var responseMessage = "";
            var message = "";
            if (categoryItemViewModel != null)
            {
                var file = categoryItemViewModel.File;
                var result = await _fileService.GetResponeUploadFileAsync(file, _httpClient);
                var responseUploadFileList = result.Item1;
                if (responseUploadFileList != null)
                {
                    categoryItemViewModel.ImageName = responseUploadFileList.Data[0];
                    categoryItemViewModel.ImageUrl = responseUploadFileList.Data[1];
                }
                responseMessage = result.Item2;
                try
                {
                    var apiUrl = _urlService.GetBaseUrl() + "/api/categories";
                    _httpClient.DefaultRequestHeaders.Authorization =
                               new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await _httpClient.PostAsJsonAsync(apiUrl, categoryItemViewModel);
                    var responseList = JsonConvert.DeserializeObject<ResponseApiData<CategoryItemViewModel>>
                                       (await response.Content.ReadAsStringAsync());
                    message = responseList.Message;

                    if (responseList.IsSuccess)
                    {
                        return Tuple.Create(true, message + " ! " + responseMessage);
                    }
                }
                catch (Exception exception)
                {
                    message = exception.Message + " ! " + "Create Category Fail !";
                    return Tuple.Create(false, responseMessage + message);
                }
            }

            return Tuple.Create(false, message);
        }

        public async Task<Tuple<bool, string>> UpdateCategoryAsync(CategoryItemViewModel categoryItemViewModel, string token)
        {
            var responseMessage = "";
            var message = "";
            var link = "";
            var fileName = DISPLAY.IMAGE_PATH;
            var file = categoryItemViewModel.File;
            if (file != null)
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
            if (!string.IsNullOrEmpty(categoryItemViewModel.ImageName) && file == null)
            {
                fileName = categoryItemViewModel.ImageName;
            }
            categoryItemViewModel.ImageName = fileName;
            categoryItemViewModel.ImageUrl = link;

            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/categories";
                _httpClient.DefaultRequestHeaders.Authorization =
                               new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsJsonAsync(apiUrl, categoryItemViewModel);
                var responseList = JsonConvert.DeserializeObject<ResponseApiData<List<CategoryItemViewModel>>>
                                    (await response.Content.ReadAsStringAsync());
                message = responseList.Message;
                return Tuple.Create(responseList.IsSuccess, message + " ! " + responseMessage);
            }
            catch (Exception exception)
            {
                message = exception.Message + " ! " + "Updated Category Fail !";
                return Tuple.Create(false, responseMessage + message);
            }
        }

        public async Task<Tuple<bool, string>> DeleteCategoryAsync(string categoryId, string token)
        {
            var message = "";
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/categories/";
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var response = await _httpClient.DeleteAsync(apiUrl + categoryId);
                var responseList = JsonConvert.DeserializeObject<ResponseApiData<CategoryItemViewModel>>
                                   (await response.Content.ReadAsStringAsync());
                message = responseList.Message;
                return Tuple.Create(responseList.IsSuccess, message);
            }
            catch (Exception exception)
            {
                message = exception.Message + " ! " + "Deleted Category Fail !";
                return Tuple.Create(false, message);
            }
        }

        public async Task<Tuple<CategoryItemViewModel, string>> GetCategoryByIdAsync(string categoryId)
        {
            var message = "";
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/categories/";
                var response = await _httpClient.GetAsync(apiUrl + categoryId);
                var responseList = JsonConvert.DeserializeObject<ResponseApiData<CategoryItemViewModel>>
                                   (await response.Content.ReadAsStringAsync());
                var category = responseList.Data;
                message = responseList.Message;

                category.ImageUrl = _urlService.GetFileApiUrl(category.ImageName);
                return Tuple.Create(category, message);

            }
            catch (Exception exception)
            {
                message = exception.Message + " ! " + "Get Product Fail !";
                return Tuple.Create(default(CategoryItemViewModel), message);
            }
        }
    }
}
