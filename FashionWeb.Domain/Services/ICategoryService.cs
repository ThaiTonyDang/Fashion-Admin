using FashionWeb.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryItemViewModel>> GetListCategories();
        public Task<CategoryViewModel> GetCategoryViewModel();
        public Task<Tuple<bool, string>> CreateCategoryAsync(CategoryItemViewModel categoryItemViewModel);
        public Task<Tuple<bool, string>> UpdateCategoryAsync(CategoryItemViewModel categoryItemViewModel);
        public Task<Tuple<bool, string>> DeleteCategoryAsync(string categoryId);
        public Task<Tuple<CategoryItemViewModel, string>> GetCategoryByIdAsync(string categoryId);
    }
}
