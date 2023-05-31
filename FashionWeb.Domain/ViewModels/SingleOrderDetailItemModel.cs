using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.ViewModels
{
    public class SingleOrderDetailItemModel :BaseInformationItem
    {
        public List<ProductItemViewModel> Products{ get; set; }
    }
}
