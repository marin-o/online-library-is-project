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
    public class BorrowingHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BorrowingHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BorrowingHistories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BorrowingHistories.Include(b => b.Member);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BorrowingHistories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowingHistory = await _context.BorrowingHistories
                .Include(b => b.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowingHistory == null)
            {
                return NotFound();
            }

            return View(borrowingHistory);
        }

        // GET: BorrowingHistories/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BorrowingHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,Id")] BorrowingHistory borrowingHistory)
        {
            if (ModelState.IsValid)
            {
                borrowingHistory.Id = Guid.NewGuid();
                _context.Add(borrowingHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", borrowingHistory.MemberId);
            return View(borrowingHistory);
        }

        // GET: BorrowingHistories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowingHistory = await _context.BorrowingHistories.FindAsync(id);
            if (borrowingHistory == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", borrowingHistory.MemberId);
            return View(borrowingHistory);
        }

        // POST: BorrowingHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MemberId,Id")] BorrowingHistory borrowingHistory)
        {
            if (id != borrowingHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowingHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowingHistoryExists(borrowingHistory.Id))
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
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", borrowingHistory.MemberId);
            return View(borrowingHistory);
        }

        // GET: BorrowingHistories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowingHistory = await _context.BorrowingHistories
                .Include(b => b.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowingHistory == null)
            {
                return NotFound();
            }

            return View(borrowingHistory);
        }

        // POST: BorrowingHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var borrowingHistory = await _context.BorrowingHistories.FindAsync(id);
            if (borrowingHistory != null)
            {
                _context.BorrowingHistories.Remove(borrowingHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowingHistoryExists(Guid id)
        {
            return _context.BorrowingHistories.Any(e => e.Id == id);
        }
    }
}
