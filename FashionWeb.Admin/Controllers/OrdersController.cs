using FashionWeb.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace FashionWeb.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("/orders-information")]
        public async Task<IActionResult> Index()
        {
            var token = User.FindFirst("token").Value;
            var orderViewModel = await _orderService.GetOrderViewModel(token);
            return View(orderViewModel);
        }

        [HttpGet]
        [Route("orders/{orderId}")]
        public async Task<IActionResult> Detail(string orderId)
        {
            var token = User.FindFirst("token").Value;
            var result = await _orderService.GetOrdersDetail(orderId, token);
            var orderDetailItemModel = result.Item1;

            return View(orderDetailItemModel);
        }

    }
}
