using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetProductsByCategory(int categoryId);
        Product? GetProductWithCategory(int id);
        int GetTotalProducts();
    }
}