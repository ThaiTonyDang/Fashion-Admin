using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.ViewModels
{
    public class CategoryItemViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "CATEGORY NAME IS REQUIRED")]
        public string Name { get; set; }

        [Required(ErrorMessage = "DESCRIPTION IS REQUIRED")]
        public string Description { get; set; }

        [Required(ErrorMessage = "IMAGE UPLOAD IS REQUIRED")]
        public IFormFile File { get; set; }

        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CategoryViewModel
    {
        public List<CategoryItemViewModel> ListCategory { get; set; }
        public string ExceptionMessage { get; set; }
        public CategoryViewModel()
        {
            this.ListCategory = new List<CategoryItemViewModel>();
        }
    }
}
