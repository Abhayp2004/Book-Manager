using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SimpleBookManager.Data;
using SimpleBookManager.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBookManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(BookDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Store user info in session
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("Username", user.Username);
                    
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", "Invalid username or password");
            }
            
            return View(model);
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if username already exists
                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(model);
                }

                // Create new user
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, // In a real app, this would be hashed
                    FullName = model.FullName,
                    Email = model.Email,
                    RegistrationDate = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Log in the user
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();
            
            return RedirectToAction("Index", "Home");
        }
    }
}