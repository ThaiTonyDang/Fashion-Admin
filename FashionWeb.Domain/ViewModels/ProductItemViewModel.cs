using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.ViewModels
{
    public class ProductItemViewModel
    {
        private const string requiredString =
        "(^([0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12})$)";

        public bool IsCheck { get; set; }
        public Guid Id { set; get; }

        [Required(ErrorMessage = "NAME IS REQUIRED")]
        public string Name { set; get; }

        [Required(ErrorMessage = "PROVIDER IS REQUIRED")]
        public string Provider { set; get; }

        [Range(0, Double.MaxValue, ErrorMessage = "PRICE IS REQUIRED")]
        public string Price { set; get; }

        [RegularExpression($"{requiredString}",
        ErrorMessage = "CATEGORY IS REQUIRED")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "UPLOAD IMAGE IS REQUIRED")]
        public IFormFile Image { get; set; }

        public string ImagePath { get; set; }
        public int UnitsInStock { get; set; }
        public bool Enable { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public List<CategoryItemViewModel> Categories { get; set; }
        public string CategoryName { get; set; }
    }

    public class ProductViewModel
    {
        public List<ProductItemViewModel> ListProduct { get; set; }
        public ProductViewModel()
        {
            this.ListProduct = new List<ProductItemViewModel>();
        }
    }
}
