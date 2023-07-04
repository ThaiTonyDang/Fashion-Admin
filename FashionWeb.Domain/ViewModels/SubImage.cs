using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.ViewModels
{
    public class SubImage
    {
        public Guid ProductId { get; set; }
        public ProductItemViewModel Product { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
    }
}
