using FashionWeb.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Services
{
	public interface IProductService
	{
		public Task<List<ProductItemViewModel>> GetListProducts();
		public Task<ProductViewModel> GetProductViewModel();
		public Task<Tuple<bool, string>> CreateProductAsync(ProductItemViewModel productItemViewModel);
		public Task<Tuple<bool, string>> UpdateProductAsync(ProductItemViewModel productItemViewModel);
		public Task<Tuple<ProductItemViewModel, string>> GetProductByIdAsync(string productId);
		public Task<Tuple<bool, string>> DeleteProductAsync(string productId);

    }
}
