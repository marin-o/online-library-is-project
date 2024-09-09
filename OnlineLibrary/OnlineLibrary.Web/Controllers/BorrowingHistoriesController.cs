using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Service.Interface;

namespace OnlineLibrary.Web.Controllers
{
    public class BorrowingHistoriesController : Controller
    {
        private readonly IBorrowingHistoryService borrowingHistoryService;

        public BorrowingHistoriesController(IBorrowingHistoryService borrowingHistoryService)
        {
            this.borrowingHistoryService = borrowingHistoryService;
        }

        // GET: BorrowingHistories
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            var borrowingHistories = borrowingHistoryService.GetBorrowingHistoriesForUser(userId);
            return View(borrowingHistories);
        }


        // GET: BorrowingHistories/ReturnBooks/5
        public IActionResult ReturnBooks(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowingHistory = borrowingHistoryService.Get(id);
            if (borrowingHistory == null || borrowingHistory.MemberId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            return View(borrowingHistory);
        }


        // POST: BorrowingHistories/ReturnBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnBook(Guid borrowingHistoryId, Guid bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            var result = borrowingHistoryService.ReturnBook(borrowingHistoryId, bookId, userId);
            if (!result)
            {
                return BadRequest("Failed to return the book.");
            }

            return RedirectToAction(nameof(ReturnBooks), new { id = borrowingHistoryId });
        }


        // POST: BorrowingHistories/ReturnBooks/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnBooks(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            var result = borrowingHistoryService.ReturnBooks(id, userId);
            if (!result)
            {
                return BadRequest("Failed to return books.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
