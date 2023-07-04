using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.ViewModels
{
    public class OrderItemViewModel : BaseInformationItem
    {
        public int OrderProductsQuantity { get; set; }
    }

    public class OrderViewModel
    {
        public List<OrderItemViewModel> ListOrder { get; set; }
        public string ExceptionMessage { get; set; }
        public string[] Errors { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public OrderViewModel()
        {
            this.ListOrder = new List<OrderItemViewModel>();
        }
    }
}
