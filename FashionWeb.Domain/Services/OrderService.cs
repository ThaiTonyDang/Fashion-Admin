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
        public string[] _exceptionMessage;
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
                var apiUrl = _urlService.GetBaseUrl() + "api/aggregateorders";
                var response = await _httpClient.GetAsync(apiUrl);

                var responseList = JsonConvert.DeserializeObject<ResponseApiData<List<OrderItemViewModel>>>
                                   (await response.Content.ReadAsStringAsync());
                _isSuccess = responseList.IsSuccess;
                _exceptionMessage = new string[] { };
                _statusCode = responseList.StatusCode;

                var orderItem = responseList.Data;

                return orderItem;
            }
            catch (Exception exception)
            {
                _exceptionMessage = new string[] { exception.InnerException.Message };
                _statusCode = (int)HttpStatusCode.ServiceUnavailable;

                return null;
            }
        }

        public async Task<OrderViewModel> GetOrderViewModel()
        {
            var orderViewModel = new OrderViewModel();
            orderViewModel.ListOrder = await GetListOrders();

            orderViewModel.ExceptionMessage = _exceptionMessage;
            orderViewModel.StatusCode = (HttpStatusCode)_statusCode;
            orderViewModel.IsSuccess = _isSuccess;

            return orderViewModel;
        }

        public async Task<Tuple<SingleOrderDetailItemModel, string>> GetSingleDetail(string orderId)
        {
            var message = "";
            try
            {
                var apiUrl = _urlService.GetBaseUrl() + "api/aggregateorders/";
                var response = await _httpClient.GetAsync(apiUrl + orderId);
                var responseList = JsonConvert.DeserializeObject<ResponseApiData<SingleOrderDetailItemModel>>
                                   (await response.Content.ReadAsStringAsync());
                var singleOrderDetail = responseList.Data;
                message = responseList.Message;

                if (singleOrderDetail != null)
                {
                    foreach (var product in singleOrderDetail.Products)
                    {
                        product.ImageUrl = _urlService.GetFileApiUrl(product.ImageName);
                    }
                }    
               
                return Tuple.Create(singleOrderDetail, message);

            }
            catch (Exception exception)
            {
                message = exception.InnerException.Message + " ! " + "Get Order Detail Fail !";
                return Tuple.Create(default(SingleOrderDetailItemModel), message);
            }
        }
    }
}
