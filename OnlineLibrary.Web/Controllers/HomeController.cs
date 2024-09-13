using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Domain;
using OnlineLibrary.Service.Interface;
using System.Diagnostics;

namespace OnlineLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotificationService _notificationService;
        private readonly int numNotifications = 6;

        public HomeController(ILogger<HomeController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            var notifications = _notificationService.GetLatestNotifications(numNotifications);
            return View(notifications);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
