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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Category? GetCategoryById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }


        public void AddCategory(Category category)
        {
            _categoryRepository.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            _categoryRepository.Update(category);
        }

        public void DeleteCategory(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category != null)
            {
                _categoryRepository.Remove(category);
            }
        }
    }
}