using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository;

namespace OnlineLibrary.Web.Controllers
{
    public class BorrowingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BorrowingCartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BorrowingCarts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BorrowingCarts.Include(b => b.Member);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BorrowingCarts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowingCart = await _context.BorrowingCarts
                .Include(b => b.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowingCart == null)
            {
                return NotFound();
            }

            return View(borrowingCart);
        }

        // GET: BorrowingCarts/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BorrowingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,Id")] BorrowingCart borrowingCart)
        {
            if (ModelState.IsValid)
            {
                borrowingCart.Id = Guid.NewGuid();
                _context.Add(borrowingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", borrowingCart.MemberId);
            return View(borrowingCart);
        }

        // GET: BorrowingCarts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowingCart = await _context.BorrowingCarts.FindAsync(id);
            if (borrowingCart == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", borrowingCart.MemberId);
            return View(borrowingCart);
        }

        // POST: BorrowingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MemberId,Id")] BorrowingCart borrowingCart)
        {
            if (id != borrowingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowingCartExists(borrowingCart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", borrowingCart.MemberId);
            return View(borrowingCart);
        }

        // GET: BorrowingCarts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowingCart = await _context.BorrowingCarts
                .Include(b => b.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowingCart == null)
            {
                return NotFound();
            }

            return View(borrowingCart);
        }

        // POST: BorrowingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var borrowingCart = await _context.BorrowingCarts.FindAsync(id);
            if (borrowingCart != null)
            {
                _context.BorrowingCarts.Remove(borrowingCart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowingCartExists(Guid id)
        {
            return _context.BorrowingCarts.Any(e => e.Id == id);
        }
    }
}
