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
        public async Task<List<OrderItemViewModel>> GetListOrders()
        {
            try
            {
                var apiUrl =  "/api/ordersinformation";
                var response = await _httpClient.GetAsync(apiUrl);

                var responseList = JsonConvert.DeserializeObject<ResponseDataApi<List<OrderItemViewModel>>>
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

        public async Task<OrderViewModel> GetOrderViewModel()
        {
            var orderViewModel = new OrderViewModel();
            orderViewModel.ListOrder = await GetListOrders();

            orderViewModel.ExceptionMessage = _message;
            orderViewModel.StatusCode = (HttpStatusCode)_statusCode;
            orderViewModel.IsSuccess = _isSuccess;

            return orderViewModel;
        }

        public async Task<Tuple<OrderDetailItemModel, string>> GetOrdersDetail(string orderId)
        {
            var message = "";
            try
            {
                var apiProductsUrl =  "/api/ordersinformation/products/";
                var response_products = await _httpClient.GetAsync(apiProductsUrl + orderId);
                var responseList_products = JsonConvert.DeserializeObject<ResponseDataApi<List<ProductItemViewModel>>>
                                   (await response_products.Content.ReadAsStringAsync());
                var products = responseList_products.Data;
                var message_products = responseList_products.Message;

                var apiBaseInforUrl = "/api/ordersinformation/customers/";
                var response_baseInfor = await _httpClient.GetAsync(apiBaseInforUrl + orderId);
                var responseList_baseInfor = JsonConvert.DeserializeObject<ResponseDataApi<BaseInformationItem>>
                                   (await response_baseInfor.Content.ReadAsStringAsync());
                var baseInfor = responseList_baseInfor.Data;
                var message_baseInfor = responseList_products.Message;
                message = message_baseInfor + " " + message_products;

                foreach (var product in products)
                {
                    //product.ImageUrl = _urlService.GetFileApiUrl(product.MainImageName);
                }

                var orderDetail = new OrderDetailItemModel
                {
                    BaseInformationItem = baseInfor,
                    Products = products
                };
                if(responseList_baseInfor.IsSuccess)
                    if(responseList_products.IsSuccess)

                        return Tuple.Create(orderDetail, "Load order detail success !");

            }
            catch (Exception exception)
            {
                message = exception.InnerException.Message + " ! " + "Get Order Detail Fail !";
                return Tuple.Create(default(OrderDetailItemModel), message);
            }

            return Tuple.Create(default(OrderDetailItemModel), message);
        }
    }
}
