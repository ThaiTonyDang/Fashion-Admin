﻿using FashionWeb.Domain.ResponseModel;
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
		public Task<List<ProductItemViewModel>> GetPagingProductListAsync(int currentPage);
        public Task<ProductViewModel> GetPagingProductViewModel(int currentPage);
        public Task<Tuple<bool, string>> CreateProductAsync(ProductItemViewModel productItemViewModel, string token);
		public Task<Tuple<bool, string>> UpdateProductAsync(ProductItemViewModel productItemViewModel, string token);
		public Task<Tuple<ProductItemViewModel, string>> GetProductByIdAsync(string productId);
		public Task<Tuple<bool, string>> DeleteProductAsync(string productId);
    }
}
