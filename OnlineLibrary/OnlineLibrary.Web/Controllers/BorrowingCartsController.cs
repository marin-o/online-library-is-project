using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineLibrary.Domain;
using OnlineLibrary.Domain.DTO;
using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Domain.Models.RelationalModels;
using OnlineLibrary.Repository;
using OnlineLibrary.Service.Interface;
using Stripe;

namespace OnlineLibrary.Web.Controllers
{
    public class BorrowingCartsController : Controller
    {
        private readonly IBorrowingCartService borrowingCartService;
        private readonly StripeSettings stripeSettings;

        public BorrowingCartsController(IBorrowingCartService borrowingCartService, IOptions<StripeSettings> stripeSettings)
        {
            this.borrowingCartService = borrowingCartService;
            this.stripeSettings = stripeSettings.Value;
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

        [HttpPost]
        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            StripeConfiguration.ApiKey = stripeSettings.SecretKey;
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = borrowingCartService.getBorrowingCartInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var amount = Math.Max(50, 0.5 * order.BooksInCart.Count * 100); // ensures at least 50 cents
            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(amount) * 100, // Amount is in cents, so multiply by 100
                Description = "Online Library Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                this.Borrow();
                return RedirectToAction("BorrowSuccess");
            }
            else
            {
                return RedirectToAction("NotsuccessPayment");
            }
        }


        public IActionResult BorrowSuccess()
        {
            return View();
        }
    }
}
