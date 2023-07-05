using FashionWeb.Domain.ViewModels;

namespace FashionWeb.Domain.Services
{
    public interface IOrderService
    {
        public Task<List<OrderItemViewModel>> GetListOrders(string token);
        public Task<OrderViewModel> GetOrderViewModel(string token);
        public Task<Tuple<OrderDetailItemModel, string>> GetOrdersDetail(string orderId, string token);
    }
}
