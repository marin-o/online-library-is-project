using EShop.Repository.Interface;
using OnlineLibrary.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Implementation
{
    public class AuthorService
    {
        private readonly IRepository<Author> _AuthorRepository;

        public AuthorService(IRepository<Author> AuthorRepository)
        {
            _AuthorRepository = AuthorRepository;
        }
        public void CreateNewAuthor(Author p)
        {
            _AuthorRepository.Insert(p);
        }

        public void DeleteAuthor(Guid id)
        {
            var Author = _AuthorRepository.Get(id);
            _AuthorRepository.Delete(Author);
        }

        public List<Author> GetAllAuthors()
        {
            return _AuthorRepository.GetAll().ToList();
        }

        public Author GetDetailsForAuthor(Guid? id)
        {
            return _AuthorRepository.Get(id);
        }

        public void UpdeteExistingAuthor(Author p)
        {
            _AuthorRepository.Update(p);
        }

        public bool AuthorExists(Guid id)
        {
            return _AuthorRepository.Get(id) != null;
        }
    }
}
