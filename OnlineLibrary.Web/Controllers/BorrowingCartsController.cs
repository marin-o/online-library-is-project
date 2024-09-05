using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.DTO;
using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository;
using OnlineLibrary.Service.Interface;

namespace OnlineLibrary.Web.Controllers
{
    public class BorrowingCartsController : Controller
    {
        private readonly IBorrowingCartService borrowingCartService;

        public BorrowingCartsController(IBorrowingCartService borrowingCartService)
        {
            this.borrowingCartService = borrowingCartService;
        }

        // GET: BorrowingCarts
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge(); 
            }

            var borrowingCart = borrowingCartService.getBorrowingCartInfo(userId);
            return View(borrowingCart);
        }

        public IActionResult RemoveFromCart(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge(); 
            }

            borrowingCartService.RemoveFromCart(userId, id.Value);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Borrow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge(); 
            }

            var result = borrowingCartService.borrow(userId);

            if (!result.success)
            {
                // Pass the unavailable books to the view to notify the user
                return View("UnavailableBooks", result.unavailableBooks);
            }
            else
            {
                // Continue with the borrowing process or redirect to a success page
                return RedirectToAction("BorrowSuccess");
            }
        }

        public IActionResult BorrowSuccess()
        {
            return View();
        }
    }
}
