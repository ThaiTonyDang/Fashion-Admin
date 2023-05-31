using FashionWeb.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Services
{
    public interface IOrderService
    {
        public Task<List<OrderItemViewModel>> GetListOrders();
        public Task<OrderViewModel> GetOrderViewModel();
        public Task<Tuple<SingleOrderDetailItemModel, string>> GetSingleDetail(string orderId);
    }
}
