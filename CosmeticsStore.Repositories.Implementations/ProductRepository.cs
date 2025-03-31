using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CosmeticsStore.Models;
using CosmeticsStore.Data;

namespace CosmeticsStore.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public override IEnumerable<Product> GetAll()
        {
            return _context.Products
                .Include(p => p.Category)
                .ToList();
        }

        public int GetTotalProducts()
        {
            return _context.Products.Count();
        }
    }
} 