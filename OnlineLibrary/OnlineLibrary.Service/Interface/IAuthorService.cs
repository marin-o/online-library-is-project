using OnlineLibrary.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Interface
{
    public interface IAuthorService
    {
        List<Author> GetAllAuthors();
        Author GetDetailsForAuthor(Guid? id);
        void CreateNewAuthor(Author b);
        void UpdeteExistingAuthor(Author b);
        void DeleteAuthor(Guid id);
        bool AuthorExists(Guid id);
    }
}
