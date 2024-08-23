using EShop.Repository.Interface;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Implementation
{
    public class CategoryService : ICategorySevice
    {
        private readonly IRepository<Category> _CategoryRepository;

        public CategoryService(IRepository<Category> CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }
        public void CreateNewCategory(Category p)
        {
            _CategoryRepository.Insert(p);
        }

        public void DeleteCategory(Guid id)
        {
            var Category = _CategoryRepository.Get(id);
            _CategoryRepository.Delete(Category);
        }

        public List<Category> GetAllCategories()
        {
            return _CategoryRepository.GetAll().ToList();
        }

        public Category GetDetailsForCategory(Guid? id)
        {
            return _CategoryRepository.Get(id);
        }

        public void UpdeteExistingCategory(Category p)
        {
            _CategoryRepository.Update(p);
        }

        public bool CategoryExists(Guid id)
        {
            return _CategoryRepository.Get(id) != null;
        }
    }
}
