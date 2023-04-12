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
    }
}
