using System.Collections.Generic;
using System.Linq;

namespace CosmeticsStore.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public int GetTotalProducts()
        {
            return _productRepository.GetTotalProducts(); // Call the repository method
        }
    }
} 