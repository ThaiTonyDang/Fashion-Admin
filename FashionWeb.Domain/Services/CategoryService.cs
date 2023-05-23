using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.Services.HttpClients;
using FashionWeb.Domain.ViewModels;
using Newtonsoft.Json;

namespace FashionWeb.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientService _urlService;
        private readonly IFileService _fileService;
        private readonly HttpClient _httpClient;
        public string _exceptionMessage;
        public CategoryService(IHttpClientService urlService, IFileService fileService, HttpClient httpClient)
        {
            _urlService = urlService;
            _fileService = fileService;
            _httpClient = httpClient;
        }

        public async Task<List<CategoryItemViewModel>> GetListCategories()
        {
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "api/categories";
                var response = await _httpClient.GetAsync(apiUrl);

                var responseList = JsonConvert.DeserializeObject<ResponseApiData<List<CategoryItemViewModel>>>
                                   (await response.Content.ReadAsStringAsync());

                var isSuccess = responseList.IsSuccess;
                if (!isSuccess)
                {
                    return new List<CategoryItemViewModel>();
                }
                var categories = responseList.Data;

                foreach (var category in categories)
                {
                    category.ImageUrl = _urlService.GetFileApiUrl(category.ImageName);
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
            categoryViewModel.ListCategory = await GetListCategories();
            if (categoryViewModel.ListCategory == null)
            {
                categoryViewModel.ExceptionMessage = _exceptionMessage;
            }

            return categoryViewModel;
        }
    }
}
