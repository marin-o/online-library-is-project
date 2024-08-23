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
        private readonly IRepository<BorrowingCart> _BorrowingCartRepository;
        private readonly IRepository<Book> _BookRepository;
        private readonly IRepository<BorrowingHistory> _BorrowingHistoryRepository;
        private readonly IRepository<BookInBorrowingCart> _BookInBorrowingCartRepository;
        private readonly IRepository<BookInBorrowingHistory> _BookInBorrowingHistoryRepository;
        private readonly IUserRepository _userRepository;

        public BorrowingCartService(IRepository<BorrowingCart> borrowingCartRepository, IRepository<Book> bookRepository, IRepository<BorrowingHistory> borrowingHistoryRepository, IRepository<BookInBorrowingCart> bookInBorrowingCartRepository, IRepository<BookInBorrowingHistory> bookInBorrowingHistoryRepository, IUserRepository userRepository)
        {
            _BorrowingCartRepository = borrowingCartRepository;
            _BookRepository = bookRepository;
            _BorrowingHistoryRepository = borrowingHistoryRepository;
            _BookInBorrowingCartRepository = bookInBorrowingCartRepository;
            _BookInBorrowingHistoryRepository = bookInBorrowingHistoryRepository;
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
            _BorrowingCartRepository.Update(BorrowingCart);
            return true;
        }

        public bool borrow(string userId)
        {
            if(userId == null)
                return false;

            var loggedInUser = _userRepository.Get(userId);
            /*
            var userBorrowingCart = loggedInUser?.BorrowingCart;
            // EmailMessage message = new EmailMessage();
            message.Subject = "Successfull BorrowingHistory";
            message.MailTo = loggedInUser.Email;
            BorrowingHistory BorrowingHistory = new BorrowingHistory
            {
                Id = Guid.NewGuid(),
                MemberId = userId,
                User = loggedInUser
            };

            List<BookInBorrowingHistory> BooksInBorrowingHistory = new List<BookInBorrowingHistory>();

            var rez = userBorrowingCart?.Books.Select(
                z => new BookInBorrowingHistory
                {
                    Id = Guid.NewGuid(),
                    BookId = z.Book.Id,
                    Book = z.Book,
                    BorrowingHistoryId = BorrowingHistory.Id,
                    BorrowingHistory = BorrowingHistory,
                    BorrowedAt = DateTime.Now
                }).ToList();


            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            sb.AppendLine("Your BorrowingHistory is completed. The BorrowingHistory conatins: ");

            for (int i = 1; i <= rez.Count(); i++)
            {
                var currentItem = rez[i - 1];
                totalPrice += currentItem.Quantity * currentItem.Book.Price;
                sb.AppendLine(i.ToString() + ". " + currentItem.Book.BookName + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.Book.Price);
            }

            sb.AppendLine("Total price for your BorrowingHistory: " + totalPrice.ToString());
            message.Content = sb.ToString();

            BooksInBorrowingHistory.AddRange(rez);

            foreach (var Book in BooksInBorrowingHistory)
            {
                _BookInBorrowingHistoryRepository.Insert(Book);
            }

            loggedInUser.BorrowingCart.BookInBorrowingCarts.Clear();
            _userRepository.Update(loggedInUser);
            this._emailService.SendEmailAsync(message);

            return true;*/
            throw new NotImplementedException();
        }

        public bool deleteBookFromBorrowingCart(string userId, Guid BookId)
        {
            if (userId == null)
                return false;

            var loggedInUser = _userRepository.Get(userId);
            var userBorrowingCart = loggedInUser?.BorrowingCart;
            var Book = userBorrowingCart?.Books?.Where(x => x.BookId == BookId).FirstOrDefault();

            userBorrowingCart?.Books?.Remove(Book);
            _BorrowingCartRepository.Update(userBorrowingCart);
            return true;
        }

        public BorrowingCartDTO getBorrowingCartInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);
            var userBorrowingCart = loggedInUser?.BorrowingCart;
            var allBook = userBorrowingCart?.Books?.ToList();

            BorrowingCartDTO dto = new BorrowingCartDTO
            {
                BooksInCart = allBook,
                NumBorrowedBooks = allBook.Count()
            };
            return dto;
        }
    }
}
