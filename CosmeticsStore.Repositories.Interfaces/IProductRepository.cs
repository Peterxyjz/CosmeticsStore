using System.Collections.Generic;
using CosmeticsStore.Models;

namespace CosmeticsStore.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsByCategory(int categoryId);
        Product? GetProductWithCategory(int id);
        int GetTotalProducts();
    }
} 