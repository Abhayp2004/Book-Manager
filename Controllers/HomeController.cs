using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SimpleBookManager.Data;
using SimpleBookManager.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBookManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookDbContext _context;

        public HomeController(ILogger<HomeController> logger, BookDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Check if user is logged in
        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        public async Task<IActionResult> Index()
        {
            // If user is logged in, show dashboard
            if (IsLoggedIn())
            {
                // Get statistics for dashboard
                var totalBooks = await _context.Books.CountAsync();
                var readBooks = await _context.Books.Where(b => b.IsRead).CountAsync();
                var unreadBooks = await _context.Books.Where(b => !b.IsRead).CountAsync();

                // Get recent books
                var recentBooks = await _context.Books
                    .OrderByDescending(b => b.DateAdded)
                    .Take(4)
                    .ToListAsync();

                ViewBag.TotalBooks = totalBooks;
                ViewBag.ReadBooks = readBooks;
                ViewBag.UnreadBooks = unreadBooks;
                ViewBag.RecentBooks = recentBooks;
                ViewBag.Username = HttpContext.Session.GetString("Username");

                return View("Dashboard");
            }

            // Otherwise show landing page
            return View();
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
