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
        public virtual IEnumerable<Category> GetAll()
        {
            return _context.Categories.ToList();
        }
   
    }
}