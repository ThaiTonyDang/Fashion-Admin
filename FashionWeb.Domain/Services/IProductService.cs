using FashionWeb.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Services
{
    public interface IProductService
    {
        public Task<bool> AddProductAsync(ProductItemViewModel productItemViewModel);
        public Task<List<ProductItemViewModel>> GetListProducts();
        public Task<ProductViewModel> GetProductViewModel();
        public Task<bool> EditProductAsync(ProductItemViewModel productItemViewModel);
        public Task<ProductItemViewModel> GetProductItemByIdAsync(Guid id);
    }
}
