using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Product? GetProductById(int id)
        {
            return _productRepository.GetProductWithCategory(id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return _productRepository.GetProductsByCategory(categoryId);
        }

        public void AddProduct(Product product)
        {
            // Validate product data
            if (string.IsNullOrEmpty(product.ProductName) || product.Price <= 0)
                throw new ArgumentException("Product name and price are required.");

            product.CreatedDate = DateTime.Now;
            product.Status = true;
            _productRepository.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.Update(product);
        }

        public void DeleteProduct(int id)
        {
            var product = _productRepository.GetById(id);
            if (product != null)
            {
                // Soft delete - just change status to inactive
                product.Status = false;
                _productRepository.Update(product);
            }
        }
    }
}