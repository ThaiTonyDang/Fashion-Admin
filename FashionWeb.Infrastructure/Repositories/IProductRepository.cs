﻿using FashionWeb.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        public Task<bool> AddAsync(Product product);
        public Task<List<Product>> Products();
    }
}