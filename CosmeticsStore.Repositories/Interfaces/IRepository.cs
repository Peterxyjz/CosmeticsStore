using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CosmeticsStore.Repositories.Models;

namespace CosmeticsStore.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Read operations
        T? GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        // Create operations
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        // Update operations
        void Update(T entity);

        // Delete operations
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}