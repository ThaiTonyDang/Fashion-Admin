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
		public Task<List<ProductItemViewModel>> GetListProductsAsync();
		public Task<ProductViewModel> GetProductViewModelAsync();
	}
}
