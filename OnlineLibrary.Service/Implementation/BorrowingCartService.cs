using EShop.Repository.Interface;
using OnlineLibrary.Domain.DTO;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Domain.Models.RelationalModels;
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
        private readonly IRepository<BorrowingCart> _borrowingCartRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BorrowingHistory> _borrowingHistoryRepository;
        private readonly IRepository<BookInBorrowingCart> _bookInBorrowingCartRepository;
        private readonly IRepository<BookInBorrowingHistory> _bookInBorrowingHistoryRepository;
        private readonly IUserRepository _userRepository;

        public BorrowingCartService(IRepository<BorrowingCart> borrowingCartRepository, IRepository<Book> bookRepository, IRepository<BorrowingHistory> borrowingHistoryRepository, IRepository<BookInBorrowingCart> bookInBorrowingCartRepository, IRepository<BookInBorrowingHistory> bookInBorrowingHistoryRepository, IUserRepository userRepository)
        {
            _borrowingCartRepository = borrowingCartRepository;
            _bookRepository = bookRepository;
            _borrowingHistoryRepository = borrowingHistoryRepository;
            _bookInBorrowingCartRepository = bookInBorrowingCartRepository;
            _bookInBorrowingHistoryRepository = bookInBorrowingHistoryRepository;
            _userRepository = userRepository;
        }

        public bool AddToBorrowingConfirmed(BookInBorrowingCart model, string userId)
        {
            if(userId == null)
                return false;

            var loggedInUser = _userRepository.Get(userId);
            var BorrowingCart = loggedInUser?.BorrowingCart;

            if (BorrowingCart.Books == null)
                BorrowingCart.Books = new List<BookInBorrowingCart>(); ;

            BorrowingCart.Books.Add(model);
            _borrowingCartRepository.Update(BorrowingCart);
            return true;
        }

       public bool borrow(string memberId)
        {
            if (memberId != null)
            {
                var loggedInUser = _userRepository.Get(memberId);

                var userBorrowingHistory = loggedInUser.BorrowingCart;
                //EmailMessage message = new EmailMessage(); todo: implement email service
                //message.Subject = "Successfull order";
                //message.MailTo = loggedInUser.Email;

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


                //StringBuilder sb = new StringBuilder(); todo: implement email service

                //var totalPrice = 0.0;

                //sb.AppendLine("Your order is completed. The order conatins: ");

                //for (int i = 1; i <= lista.Count(); i++)
                //{
                //    var currentItem = lista[i - 1];
                //    totalPrice += currentItem.Quantity * currentItem.Product.Price;
                //    sb.AppendLine(i.ToString() + ". " + currentItem.Product.ProductName + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.Product.Price);
                //}

                //sb.AppendLine("Total price for your order: " + totalPrice.ToString());
                //message.Content = sb.ToString();

                bookInBorrowingHistory.AddRange(lista);

                foreach (var book in bookInBorrowingHistory)
                {
                    _bookInBorrowingHistoryRepository.Insert(book);
                }

                loggedInUser.BorrowingCart.Books.Clear();
                _userRepository.Update(loggedInUser);
                //this._emailService.SendEmailAsync(message); todo: implement email service

                return true;
            }
            return false;
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
            var userBorrowingCart = loggedInUser?.BorrowingCart;
            var allBooks = userBorrowingCart?.Books?.ToList();

            BorrowingCartDTO dto = new BorrowingCartDTO
            {
                BooksInCart = allBooks,
                NumBorrowedBooks = allBooks.Count()
            };
            return dto;
        }
    }
}
