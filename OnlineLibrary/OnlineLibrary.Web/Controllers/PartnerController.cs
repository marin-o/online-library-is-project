using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Domain.Models.BaseModels;
using System.Runtime.CompilerServices;

namespace OnlineLibrary.Web.Controllers
{
    public class PartnerController : Controller
    {
        private readonly IPartnerService partnerService;

        public PartnerController(IPartnerService partnerService)
        {
            this.partnerService = partnerService;
        }

        public IActionResult Index()
        {
            var books = partnerService.GetPartnerBooksAsync();

            return View(books);
        }
    }
}
