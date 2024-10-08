﻿using EShop.Repository.Interface;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Implementation
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _BookRepository;

        public BookService(IRepository<Book> BookRepository)
        {
            _BookRepository = BookRepository;
        }
        public void CreateNewBook(Book p)
        {
            _BookRepository.Insert(p);
        }

        public void DeleteBook(Guid id)
        {
            var Book = _BookRepository.Get(id);
            _BookRepository.Delete(Book);
        }

        public List<Book> GetAllBooks()
        {
            return _BookRepository.GetAll().ToList();
        }

        public Book GetDetailsForBook(Guid? id)
        {
            var book = _BookRepository.Get(id);
            return book;
        }

        public void UpdeteExistingBook(Book p)
        {
            _BookRepository.Update(p);
        }
    }
}
