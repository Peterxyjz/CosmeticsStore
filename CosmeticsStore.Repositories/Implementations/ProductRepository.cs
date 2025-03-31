using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Repositories.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CosmeticsDbContext context) : base(context)
        {
        }

        public override IEnumerable<Product> GetAll()
        {
            return _context.Products
                .Include(p => p.Category)
                .ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.Status == true)
                .ToList();
        }
        public int GetTotalProducts()
        {
            return _context.Products.Count();
        }

        public Product? GetProductWithCategory(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);
        }
    }
}