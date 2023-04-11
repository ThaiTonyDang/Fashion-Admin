using FashionWeb.Domain.ViewModels;
using FashionWeb.Infrastructure.FileHelpers;
using FashionWeb.Infrastructure.Models;
using FashionWeb.Infrastructure.Repositories;
using FashionWeb.Utilities.GlobalHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        public ProductService(IProductRepository productRepository , IFileService fileService)
        {
            _productRepository = productRepository;
            _fileService = fileService;
        }

        public async Task<bool> AddProductAsync(ProductItemViewModel productItemViewModel)
        {
            try
            {
                if (productItemViewModel != null)
                {
                    var imagePath = default(string);
                    var image = productItemViewModel.Image;
                    if(image != null)
                    {
                        var fileName = productItemViewModel.Image.FileName;
                        imagePath = _fileService.RefactorFileName(fileName);
                    }

                    var product = new Product()
                    {
                        Id = productItemViewModel.Id,
                        Name = productItemViewModel.Name,
                        Provider = productItemViewModel.Provider,
                        Price = decimal.Parse(productItemViewModel.Price),
                        Description = productItemViewModel.Description,
                        CategoryId = productItemViewModel.CategoryId,
                        ImagePath = imagePath,
                        UnitsInStock = productItemViewModel.UnitsInStock,
                        Enable = productItemViewModel.Enable
                    };

                    var isSuccses = await _productRepository.AddAsync(product);

                    if (isSuccses)
                    {
                        var data = await productItemViewModel.Image.GetBytes();
                        var folderExtra = nameof(Product);
                        await this._fileService.SaveFile(folderExtra, imagePath, data);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}
