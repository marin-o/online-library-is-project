using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.DTO;
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
                return Challenge(); // Redirect to login if user is not authenticated
            }

            var borrowingCart = borrowingCartService.getBorrowingCartInfo(userId);
            return View(borrowingCart);
        }
    }
}
