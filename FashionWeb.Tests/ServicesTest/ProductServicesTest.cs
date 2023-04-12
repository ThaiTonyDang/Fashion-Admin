using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using FashionWeb.Infrastructure.Repositories;
using FashionWeb.Infrastructure.FileHelpers;
using Xunit;
using FashionWeb.Infrastructure.Models;
using FashionWeb.Domain.Services;
using FashionWeb.Domain.ViewModels;

namespace FashionWeb.Tests.ServicesTest
{
    public class ProductServicesTest
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IFormFile> _image;
        public ProductServicesTest()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _fileService = new Mock<IFileService>();
            _image = new Mock<IFormFile>();
        }

        [Fact]
        public async Task Should_AddProductAsync_Return_Success()
        {
            // Arrange
            var productViewModel = new ProductItemViewModel()
            {
                Id = Guid.NewGuid(),
                Price = "100",
                CategoryId = Guid.Parse("46a023f4-549b-4f45-8242-a94a2f5e5eb8"),
                Image = _image.Object
            };

            _productRepoMock.Setup(m => m.AddAsync(It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            var productService = new ProductService(_productRepoMock.Object, _fileService.Object);
            var isSuccess = await productService.AddProductAsync(productViewModel);

            // Assert
            Assert.True(isSuccess);
        }

        [Fact]
        public async Task Should_AddProductAsync_Return_Fail_When_Id_Default()
        {
            // Arrange
            var productViewModel = new ProductItemViewModel()
            {
                Id = default(Guid),
                Price = "100",
            };

            _productRepoMock.Setup(m => m.AddAsync(It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            var productService = new ProductService(_productRepoMock.Object, _fileService.Object);
            var isSuccess = await productService.AddProductAsync(productViewModel);

            // Assert
            Assert.False(isSuccess);
        }

        [Fact]
        public async Task Should_AddProductAsync_Return_Error_When_Price_Null()
        {
            // Arrange
            var productViewModel = new ProductItemViewModel()
            {
                Id = default(Guid),
                Price = null,
            };

            _productRepoMock.Setup(m => m.AddAsync(It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            var productService = new ProductService(_productRepoMock.Object, _fileService.Object);
            var isSuccess = await productService.AddProductAsync(productViewModel);

            // Assert
            Assert.False(isSuccess);
        }


        [Fact]
        public async Task Should_GetListProductAsync_Return_Success()
        {
            // Arrage
            var productExpect = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Quần Kaki",
                CategoryId = Guid.NewGuid()
            };

            var productsExpect = new List<Product>() { productExpect};
            _productRepoMock.Setup(mbox => mbox.Products()).ReturnsAsync(productsExpect);

            // Act
            var productService = new ProductService(_productRepoMock.Object, _fileService.Object);
            var productsAct = await productService.GetListProducts();

            // Assert
            Assert.Equal(productsExpect.Count, productsAct.Count);

            var productFirtItemAct = productsAct.FirstOrDefault();

            Assert.NotNull(productFirtItemAct!.Name);
            Assert.Equal(productExpect.Name, productFirtItemAct!.Name);
            Assert.Equal(productExpect.Id, productFirtItemAct!.Id);
        }
    }
}