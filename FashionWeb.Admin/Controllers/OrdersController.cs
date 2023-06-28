using FashionWeb.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FashionWeb.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("/orders")]
        public async Task<IActionResult> Index()
        {
            var orderViewModel = await _orderService.GetOrderViewModel(); 
            return View(orderViewModel);
        }

        [HttpGet]
        [Route("orders/{orderId}")]
        public async Task<IActionResult> Detail(string orderId)
        {
            var result = await _orderService.GetOrdersDetail(orderId);
            var orderDetailItemModel = result.Item1;

            return View(orderDetailItemModel);
        }

    }
}
