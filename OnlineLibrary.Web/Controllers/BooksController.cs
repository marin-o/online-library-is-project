using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository;
using OnlineLibrary.Service.Interface;

namespace OnlineLibrary.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _BookService;
        private readonly IBorrowingCartService _BorrowingCartService;
        private readonly IAuthorService _AuthorService;
        private readonly ICategorySevice _CategoryService;

        public BooksController(IBookService bookService, IBorrowingCartService borrowingCartService, IAuthorService authorService, ICategorySevice categoryService)
        {
            _BookService = bookService;
            _BorrowingCartService = borrowingCartService;
            _AuthorService = authorService;
            _CategoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View(_BookService.GetAllBooks());
        }

        // GET: Books/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Book = _BookService.GetDetailsForBook(id);
            if (Book == null)
            {
                return NotFound();
            }

            return View(Book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,BookName,BookImage,BookDescription,Price,Rating")] Book Book)
        {
            if (ModelState.IsValid)
            {
                Book.Id = Guid.NewGuid();
                _BookService.CreateNewBook(Book);
                return RedirectToAction(nameof(Index));
            }
            return View(Book);
        }

        public IActionResult AddToCart(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Book = _BookService.GetDetailsForBook(id);

            BookInBorrowingCart ps = new BookInBorrowingCart();

            if (Book != null)
            {
                ps.BookId = Book.Id;
            }

            return View(ps);
        }

        [HttpPost]
        public IActionResult AddToCartConfirmed(BookInBorrowingCart model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _BorrowingCartService.AddToBorrowingConfirmed(model, userId);



            return View("Index", _BookService.GetAllBooks());
        }


        // GET: Books/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Book = _BookService.GetDetailsForBook(id);
            if (Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,BookName,BookImage,BookDescription,Price,Rating")] Book Book)
        {
            if (id != Book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _BookService.UpdeteExistingBook(Book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Book);
        }

        // GET: Books/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Book = _BookService.GetDetailsForBook(id);
            if (Book == null)
            {
                return NotFound();
            }

            return View(Book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _BookService.DeleteBook(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
