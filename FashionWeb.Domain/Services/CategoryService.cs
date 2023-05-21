using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUrlService _urlService;
        private readonly IFileService _fileService;
        private readonly HttpClient _httpClient;
        public string _exceptionMessage;
        public CategoryService(IUrlService urlService, IFileService fileService, HttpClient httpClient)
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

                var responseList = JsonConvert.DeserializeObject<ResponseAPI<List<CategoryItemViewModel>>>
                                   (await response.Content.ReadAsStringAsync());

                var isSuccess = responseList.Success;
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
