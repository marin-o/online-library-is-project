using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Service.Interface;

namespace OnlineLibrary.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService bookService;
        private readonly IBorrowingCartService borrowingCartService;
        private readonly IAuthorService authorService;
        private readonly ICategorySevice categoryService;
        private readonly INotificationService notificationService;

        public BooksController(IBookService bookService, IBorrowingCartService borrowingCartService, IAuthorService authorService, ICategorySevice categoryService, INotificationService notificationService)
        {
            this.bookService = bookService;
            this.borrowingCartService = borrowingCartService;
            this.authorService = authorService;
            this.categoryService = categoryService;
            this.notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return View(bookService.GetAllBooks());
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = bookService.GetDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(authorService.GetAllAuthors(), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(categoryService.GetAllCategories(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,AuthorId,ISBN,Description,ImageUrl,Year,Pages,Quantity,Available,CategoryId")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.Id = Guid.NewGuid();
                book.Author = authorService.GetDetailsForAuthor(book.AuthorId);
                book.Category = categoryService.GetDetailsForCategory(book.CategoryId);
                bookService.CreateNewBook(book);
                notificationService.CreateNewNotification(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public IActionResult AddToCart(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = bookService.GetDetailsForBook(id);

            var bookInBorrowingCart = new BookInBorrowingCart
            {
                BookId = book.Id,
                Book = book
            };

            return View(bookInBorrowingCart);
        }

        [HttpPost]
        public IActionResult AddToCartConfirmed(BookInBorrowingCart model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            borrowingCartService.AddBookToBorrowingCart(model, userId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = bookService.GetDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            ViewData["AuthorId"] = new SelectList(authorService.GetAllAuthors(), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(categoryService.GetAllCategories(), "Id", "Name");
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Title,AuthorId,ISBN,Description,ImageUrl,Year,Pages,Quantity,Available,CategoryId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                book.Author = authorService.GetDetailsForAuthor(book.AuthorId);
                book.Category = categoryService.GetDetailsForCategory(book.CategoryId);
                try
                {
                    bookService.UpdeteExistingBook(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = bookService.GetDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            bookService.DeleteBook(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
