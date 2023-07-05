using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.ViewModels
{
    public class BaseInformationItem
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShipAddress { get; set; }
        public bool  IsPaid{ get; set; }
    }
}
