using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FashionWeb.Domain.Services.HttpClients;
using FashionWeb.Domain.Model;
using Newtonsoft.Json.Linq;

namespace FashionWeb.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientService _urlService;
        private readonly IFileService _fileService;
        private readonly HttpClient _httpClient;
        public string _message;
        public string[] _errors;
        public int _statusCode;
        public bool _isSuccess;
        public OrderService(IHttpClientService urlService, HttpClient httpClient, IFileService fileService)        
        {
            _urlService = urlService;
            _httpClient = httpClient;
            _fileService = fileService;
        }

        public async Task<List<OrderItemViewModel>> GetListOrders(string token)
        {
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "/api/orders/order-information";
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var response = await _httpClient.GetAsync(apiUrl);              
                var responseList = JsonConvert.DeserializeObject<ResponseApiData<List<OrderItemViewModel>>>
                                   (await response.Content.ReadAsStringAsync());
                _isSuccess = responseList.IsSuccess;
                _message = responseList.Message;
                _statusCode = responseList.StatusCode;

                var orderItem = responseList.Data;

                return orderItem;
            }
            catch (Exception exception)
            {
                _message =  exception.Message;
                _statusCode = (int)HttpStatusCode.ServiceUnavailable;

                return null;
            }
        }

        public async Task<OrderViewModel> GetOrderViewModel(string token)
        {
            var orderViewModel = new OrderViewModel();
            orderViewModel.ListOrder = await GetListOrders(token);

            orderViewModel.ExceptionMessage = _message;
            orderViewModel.StatusCode = (HttpStatusCode)_statusCode;
            orderViewModel.IsSuccess = _isSuccess;

            return orderViewModel;
        }

        public async Task<Tuple<OrderDetailItemModel, string>> GetOrdersDetail(string orderId, string token)
        {
            var apiProductsUrl = _urlService.GetBaseUrl() + "/api/orders/products/";
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response_products = await _httpClient.GetAsync(apiProductsUrl + orderId);
            var responseList_products = JsonConvert.DeserializeObject<ResponseApiData<List<ProductItemViewModel>>>
                                        (await response_products.Content.ReadAsStringAsync());
            var products = responseList_products.Data;

            var apiBaseInforUrl = _urlService.GetBaseUrl() + "/api/orders/base-information/";
            var response_baseInfor = await _httpClient.GetAsync(apiBaseInforUrl + orderId);
            var responseList_baseInfor = JsonConvert.DeserializeObject<ResponseApiData<BaseInformationItem>>
                                         (await response_baseInfor.Content.ReadAsStringAsync());
            var baseInfor = responseList_baseInfor.Data;

            if(products != null)
            {
                foreach (var product in products)
                {
                    product.ImageUrl = _urlService.GetFileApiUrl(product.MainImageName);
                }
            }    
            
            var orderDetail = new OrderDetailItemModel
            {
                BaseInformationItem = baseInfor,
                Products = products
            };

            if (!responseList_baseInfor.IsSuccess || !responseList_products.IsSuccess )
                return Tuple.Create(default(OrderDetailItemModel), "Load order detail fail !");
            return Tuple.Create(orderDetail, "Load order detail success !");
        }
    }
}
