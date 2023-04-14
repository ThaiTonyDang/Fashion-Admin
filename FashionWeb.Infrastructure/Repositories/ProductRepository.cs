using FashionWeb.Infrastructure.DataContext;
using FashionWeb.Infrastructure.Models;
using FashionWeb.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository        
    {
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AddAsync(Product product)
        {
            if (product.Id == default(Guid) || product.CategoryId == default(Guid))
                return false;

            var productEntity = _appDbContext.Products
                                             .Where(p => p.Name == product.Name && p.Provider == product.Provider)
                                             .FirstOrDefault();

            if (productEntity == null)
            {
                await _appDbContext.AddAsync(product);
                var result = await _appDbContext.SaveChangesAsync();
                return (result > 0);
            }

            return false;
        }

        public Task<List<Product>> Products()
        {
            return _appDbContext.Products.ToListAsync();
        }

        public async Task<bool> EditAsync(Product product)
        {
            var productEntity = await GetProductByIdAsync(product.Id);

            if (productEntity != null)
            {
                productEntity.Id = product.Id;
                productEntity.Name = product.Name;
                productEntity.Provider = product.Provider;
                productEntity.Price = product.Price;
                productEntity.Description = product.Description;
                productEntity.CategoryId = product.CategoryId;
                productEntity.ImagePath = product.ImagePath;
                productEntity.Type = product.Type;
                productEntity.UnitsInStock = product.UnitsInStock;
                productEntity.Enable = product.Enable;

                _appDbContext.Update(productEntity);
                var result = _appDbContext.SaveChanges();
                return (result > 0);
            }

            return false;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var product = await _appDbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

            return product;
        }
    }
}
