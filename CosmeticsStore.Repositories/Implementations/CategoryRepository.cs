using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using System;

namespace CosmeticsStore.Repositories.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CosmeticsDbContext context) : base(context)
        {
        }

        // Implement any ICategoryRepository-specific methods here
    }
}