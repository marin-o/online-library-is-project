using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository;
using OnlineLibrary.Service.Interface;

namespace OnlineLibrary.Web.Controllers
{
    public class BorrowingHistoriesController : Controller
    {
        private readonly IBorrowingHistoryService borrowingHistoryService;
        private readonly ApplicationDbContext _context;

        public BorrowingHistoriesController(IBorrowingHistoryService borrowingHistoryService, ApplicationDbContext context)
        {
            this.borrowingHistoryService = borrowingHistoryService;
            _context = context;
        }

        // GET: BorrowingHistories
        public IActionResult Index()
        {
            return View(borrowingHistoryService.GetAll());
        }

        // GET: BorrowingHistories/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowingHistory = borrowingHistoryService.Get(id);
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
        public IActionResult Create([Bind("MemberId,Id")] BorrowingHistory borrowingHistory)
        {
            if (ModelState.IsValid)
            {
                borrowingHistory.Id = Guid.NewGuid();
                borrowingHistoryService.Insert(borrowingHistory);
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", borrowingHistory.MemberId);
            return View(borrowingHistory);
        }

        // GET: BorrowingHistories/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowingHistory = borrowingHistoryService.Get(id);
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
        public IActionResult Edit(Guid id, [Bind("MemberId,Id")] BorrowingHistory borrowingHistory)
        {
            if (id != borrowingHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    borrowingHistoryService.Update(borrowingHistory);
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
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowingHistory = borrowingHistoryService.Get(id);
            if (borrowingHistory == null)
            {
                return NotFound();
            }

            return View(borrowingHistory);
        }

        // POST: BorrowingHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var borrowingHistory = borrowingHistoryService.Get(id);
            if (borrowingHistory != null)
            {
                borrowingHistoryService.Delete(borrowingHistory);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BorrowingHistoryExists(Guid id)
        {
            return borrowingHistoryService.Exists(id);
        }
    }
}
