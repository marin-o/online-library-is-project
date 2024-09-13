using EShop.Repository.Interface;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository.Interface;
using OnlineLibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Implementation
{
    public class BorrowingHistoryService : IBorrowingHistoryService
    {
        private readonly IBorrowingHistoryRepository _borrowingHistoryRepository;
        private readonly IRepository<Book> bookRepository;
        private readonly IRepository<BookInBorrowingHistory> bookInBorrowingHistoryRepository;

        public BorrowingHistoryService(IBorrowingHistoryRepository borrowingHistoryRepository, IRepository<Book> bookRepository, IRepository<BookInBorrowingHistory> bookInBorrowingHistoryRepository)
        {
            _borrowingHistoryRepository = borrowingHistoryRepository;
            this.bookRepository = bookRepository;
            this.bookInBorrowingHistoryRepository = bookInBorrowingHistoryRepository;
        }

        public void Delete(BorrowingHistory entity)
        {
            _borrowingHistoryRepository.Delete(entity);
        }

        public BorrowingHistory Get(Guid? id)
        {
            return _borrowingHistoryRepository.Get(id);
        }

        public IEnumerable<BorrowingHistory> GetAll()
        {
            return _borrowingHistoryRepository.GetAll();
        }

        public void Insert(BorrowingHistory entity)
        {
            _borrowingHistoryRepository.Insert(entity);
        }

        public void Update(BorrowingHistory entity)
        {
            _borrowingHistoryRepository.Update(entity);

        }

        public bool Exists(Guid id)
        {
            return _borrowingHistoryRepository.GetAll().Any(x => x.Id == id);
        }

        public List<BorrowingHistory> GetBorrowingHistoriesForUser(string userId)
        {
            var borrowingHistories = _borrowingHistoryRepository.GetAll().Where(x => x.MemberId == userId).ToList();

            return borrowingHistories;
        }

        public bool ReturnBooks(Guid borrowingHistoryId, string userId)
        {
            var borrowingHistory = _borrowingHistoryRepository.Get(borrowingHistoryId);

            if (borrowingHistory == null || borrowingHistory.MemberId != userId)
            {
                return false;
            }

            foreach (var book in borrowingHistory.Books)
            {
                book.Book.Quantity++;
                book.ReturnedAt = DateTime.Now;
                if (book.Book.Available == false && book.Book.Quantity > 0)
                {
                    book.Book.Available = true;
                }
                bookRepository.Update(book.Book);
            }
            _borrowingHistoryRepository.Update(borrowingHistory);

            return true;
        }

        public bool ReturnBook(Guid borrowingHistoryId, Guid bookId, string userId)
        {
            var borrowingHistory = _borrowingHistoryRepository.Get(borrowingHistoryId);

            if (borrowingHistory == null || borrowingHistory.MemberId != userId)
            {
                return false;
            }

            var bookInBorrowingHistory = borrowingHistory.Books.FirstOrDefault(b => b.BookId == bookId);
            if (bookInBorrowingHistory == null)
            {
                return false;
            }

            bookInBorrowingHistory.Book.Quantity++;
            bookInBorrowingHistory.ReturnedAt = DateTime.Now;
            bookInBorrowingHistory.Returned = true;

            if (bookInBorrowingHistory.Book.Available == false && bookInBorrowingHistory.Book.Quantity > 0)
            {
                bookInBorrowingHistory.Book.Available = true;
            }

            bookRepository.Update(bookInBorrowingHistory.Book);
            _borrowingHistoryRepository.Update(borrowingHistory);
            bookInBorrowingHistoryRepository.Update(bookInBorrowingHistory);
            return true;
        }
    }
}
