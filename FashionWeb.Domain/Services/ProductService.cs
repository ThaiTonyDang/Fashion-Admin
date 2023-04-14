using FashionWeb.Domain.ViewModels;
using FashionWeb.Infrastructure.FileHelpers;
using FashionWeb.Infrastructure.Models;
using FashionWeb.Infrastructure.Repositories;
using FashionWeb.Utilities.GlobalHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
                        Price = productItemViewModel.Price,
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

        public async Task<ProductViewModel> GetProductViewModel()
        {
            var productViewModel = new ProductViewModel();
            productViewModel.ListProduct = await GetListProducts();
            return productViewModel;
        }

        public async Task<List<ProductItemViewModel>> GetListProducts()
        {
            var products = await _productRepository.Products();
            var listProducts = products.Select(p => new ProductItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Provider = p.Provider,
                Description = p.Description,
                ImagePath = p.ImagePath,
                UnitsInStock = p.UnitsInStock,
                Enable = p.Enable,
                Type = p.Type,
                CategoryId = p.CategoryId
            }).ToList();

            return listProducts;
        }

        public async Task<bool> EditProductAsync(ProductItemViewModel productItemViewModel)
        {
            try
            {
                var imagePath = DISPLAY.IMAGE_PATH;

                if (productItemViewModel.ImagePath != null)
                {
                    imagePath = productItemViewModel.ImagePath;
                }    

                if (productItemViewModel.Image != null )
                {
                    var fileName = productItemViewModel.Image.FileName;
                    imagePath = _fileService.RefactorFileName(fileName);
                }

                var product = new Product
                {
                    Id = productItemViewModel.Id,
                    Name = productItemViewModel.Name,
                    Price = productItemViewModel.Price,
                    Provider = productItemViewModel.Provider,
                    CategoryId = productItemViewModel.CategoryId,
                    Description = productItemViewModel.Description,
                    UnitsInStock = productItemViewModel.UnitsInStock,
                    Type = productItemViewModel.Type,
                    Enable = productItemViewModel.GetEnable(),
                    ImagePath = imagePath
                };

                var result = await _productRepository.EditAsync(product);
                if (result)
                {
                    if (productItemViewModel.Image != null)
                    {                     
                        var data = await productItemViewModel.Image.GetBytes();
                        var folderExtra = nameof(Product);
                        await this._fileService.SaveFile(folderExtra, imagePath, data);           
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public async Task<ProductItemViewModel> GetProductItemByIdAsync(Guid id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            var productItem = new ProductItemViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Provider = product.Provider,
                CategoryId = product.CategoryId,
                Description = product.Description,
                UnitsInStock = product.UnitsInStock,
                Type = product.Type,
                Enable = product.Enable,
                ImagePath = product.ImagePath
            };

            return productItem;
        }
    }
}
