using FashionWeb.Domain.ViewModels;
namespace FashionWeb.Domain.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryItemViewModel>> GetCategories();
        public Task<CategoryViewModel> GetCategoryViewModel();
        public Task<Tuple<bool, string>> CreateCategoryAsync(CategoryItemViewModel categoryItemViewModel, string token);
        public Task<Tuple<bool, string>> UpdateCategoryAsync(CategoryItemViewModel categoryItemViewModel, string token);
        public Task<Tuple<bool, string>> DeleteCategoryAsync(string categoryId, string token);
        public Task<Tuple<CategoryItemViewModel, string>> GetCategoryByIdAsync(string categoryId);
    }
}
