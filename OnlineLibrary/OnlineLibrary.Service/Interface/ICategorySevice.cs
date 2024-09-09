using OnlineLibrary.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Interface
{
    public interface ICategorySevice
    {
        List<Category> GetAllCategories();
        Category GetDetailsForCategory(Guid? id);
        void CreateNewCategory(Category b);
        void UpdeteExistingCategory(Category b);
        void DeleteCategory(Guid id);
        bool CategoryExists(Guid id);
    }
}
