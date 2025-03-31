using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Service.Interfaces
{
    public interface IProductService
    {
        Product? GetProductById(int id);
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(int categoryId);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        int GetTotalProducts();
    }
}