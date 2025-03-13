using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        // Add any category-specific methods here if needed
    }
}