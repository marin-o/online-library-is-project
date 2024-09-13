using EShop.Repository.Interface;
using OnlineLibrary.Domain;
using OnlineLibrary.Domain.DTO;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository.Implementation;
using OnlineLibrary.Repository.Interface;
using OnlineLibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Service.Implementation
{
    public class BorrowingCartService : IBorrowingCartService
    {
        private readonly IBorrowingCartRepository _borrowingCartRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BorrowingHistory> _borrowingHistoryRepository;
        private readonly IRepository<BookInBorrowingCart> _bookInBorrowingCartRepository;
        private readonly IRepository<BookInBorrowingHistory> _bookInBorrowingHistoryRepository;
        private readonly IMemberRepository _userRepository;
        private readonly IEmailService emailService;

        public BorrowingCartService(IBorrowingCartRepository borrowingCartRepository, IRepository<Book> bookRepository, IRepository<BorrowingHistory> borrowingHistoryRepository, IRepository<BookInBorrowingCart> bookInBorrowingCartRepository, IRepository<BookInBorrowingHistory> bookInBorrowingHistoryRepository, IMemberRepository userRepository, IEmailService emailService)
        {
            _borrowingCartRepository = borrowingCartRepository;
            _bookRepository = bookRepository;
            _borrowingHistoryRepository = borrowingHistoryRepository;
            _bookInBorrowingCartRepository = bookInBorrowingCartRepository;
            _bookInBorrowingHistoryRepository = bookInBorrowingHistoryRepository;
            _userRepository = userRepository;
            this.emailService = emailService;
        }

        public bool AddBookToBorrowingCart(BookInBorrowingCart model, string userId)
        {
            if(userId == null)
                return false;

            var loggedInUser = _userRepository.Get(userId);
            var borrowingCart = loggedInUser?.BorrowingCart;

            if (borrowingCart == null)
            {
                borrowingCart = new BorrowingCart
                {
                    Id = Guid.NewGuid(),
                    MemberId = userId,
                    Member = loggedInUser
                };
                loggedInUser.BorrowingCart = borrowingCart;
            }
            if (borrowingCart.Books == null)
                borrowingCart.Books = new List<BookInBorrowingCart>();

            model.BorrowingCart = borrowingCart;
            borrowingCart.Books.Add(model);
            _borrowingCartRepository.Update(borrowingCart);
            return true;
        }

        public (bool success, List<Book>? unavailableBooks) borrow(string memberId)
        {
            if (memberId != null)
            {
                var loggedInUser = _userRepository.Get(memberId);

                var userBorrowingHistory = loggedInUser.BorrowingCart;

                // List to store books that are unavailable
                List<Book> unavailableBooks = new List<Book>();

                EmailMessage message = new EmailMessage();
                message.Subject = "Borrowing Confirmation";
                message.MailTo = loggedInUser.Email;

                // Check if each book has enough quantity
                foreach (var item in userBorrowingHistory.Books)
                {
                    var book = _bookRepository.Get(item.Book.Id); // Retrieve the book from the database
                    if (book.Quantity <= 0)
                    {
                        unavailableBooks.Add(book); // Add to the list if it's unavailable
                    }
                }

                // If any book is unavailable, return and show them to the user
                if (unavailableBooks.Any())
                {
                    return (false, unavailableBooks); // Return false with the list of unavailable books
                }

                // Proceed with the borrowing process
                BorrowingHistory borrowingHistory = new BorrowingHistory
                {
                    Id = Guid.NewGuid(),
                    MemberId = memberId,
                    Member = loggedInUser
                };

                _borrowingHistoryRepository.Insert(borrowingHistory);

                List<BookInBorrowingHistory> bookInBorrowingHistory = new List<BookInBorrowingHistory>();

                var lista = userBorrowingHistory.Books.Select(
                    x => new BookInBorrowingHistory
                    {
                        Id = Guid.NewGuid(),
                        BookId = x.Book.Id,
                        Book = x.Book,
                        BorrowingHistoryId = borrowingHistory.Id,
                        BorrowingHistory = borrowingHistory,
                        BorrowedAt = DateTime.Now,
                        Returned = false
                    }
                ).ToList();

                StringBuilder sb = new StringBuilder();
                sb.Append("You have successfully borrowed the following books: \n");
                float cost = 0f;
                foreach (var book in lista)
                {
                    sb.Append(book.Book.Title + "\n");
                    cost += 0.5f;
                }
                sb.AppendLine("Please return the books within 14 days.");
                sb.AppendLine("Cost: 0.5$ per book.");
                message.Content = sb.ToString();

                // Now decrease the quantity of each book
                foreach (var bookInList in lista)
                {
                    var currentBook = _bookRepository.Get(bookInList.BookId); // Retrieve the book from the database
                    currentBook.Quantity -= 1; // Reduce the quantity by 1
                    _bookRepository.Update(currentBook); // Save the updated quantity to the database
                }

                bookInBorrowingHistory.AddRange(lista);

                foreach (var book in bookInBorrowingHistory)
                {
                    _bookInBorrowingHistoryRepository.Insert(book);
                }

                loggedInUser.BorrowingCart.Books.Clear();
                _userRepository.Update(loggedInUser);
                emailService.SendEmailAsync(message);
                return (true, null); // Borrowing successful, no unavailable books
            }
            return (false, null); // Failed due to null memberId
        }


        public bool deleteBookFromBorrowingCart(string userId, Guid BookId)
        {
            if (userId == null)
                return false;

            var loggedInUser = _userRepository.Get(userId);
            var userBorrowingCart = loggedInUser?.BorrowingCart;
            var Book = userBorrowingCart?.Books?.Where(x => x.BookId == BookId).FirstOrDefault();

            userBorrowingCart?.Books?.Remove(Book);
            _borrowingCartRepository.Update(userBorrowingCart);
            return true;
        }

        public BorrowingCartDTO getBorrowingCartInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);
            var userBorrowingCart = _borrowingCartRepository.Get(loggedInUser.BorrowingCart.Id);

            if(userBorrowingCart == null)
            {
                return new BorrowingCartDTO
                {
                    BooksInCart = new List<BookInBorrowingCart>(),
                    NumBorrowedBooks = 0
                };
            }

            var allBooks = userBorrowingCart?.Books?.ToList();
            BorrowingCartDTO dto = new BorrowingCartDTO
            {
                BooksInCart = allBooks,
                NumBorrowedBooks = allBooks.Count()
            };
            return dto;
        }

        public void RemoveFromCart(string userId, Guid value)
        {
            if (userId == null)
                return;

            var loggedInUser = _userRepository.Get(userId);
            var userBorrowingCart = loggedInUser?.BorrowingCart;
            var Book = userBorrowingCart?.Books?.Where(x => x.Id == value).FirstOrDefault();

            userBorrowingCart?.Books?.Remove(Book);
            _borrowingCartRepository.Update(userBorrowingCart);
        }
    }
}
