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
    public class BooksController : Controller
    {
        private readonly BookDbContext _context;

        public BooksController(BookDbContext context)
        {
            _context = context;
        }

        // Check if user is logged in
        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["AuthorSortParam"] = sortOrder == "author" ? "author_desc" : "author";
            ViewData["GenreSortParam"] = sortOrder == "genre" ? "genre_desc" : "genre";
            ViewData["ReadSortParam"] = sortOrder == "read" ? "read_desc" : "read";
            ViewData["CurrentFilter"] = searchString;

            var books = from b in _context.Books
                        select b;

            // Apply search filter
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) ||
                                         b.Author.Contains(searchString) ||
                                         b.Genre.Contains(searchString));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "author":
                    books = books.OrderBy(b => b.Author);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(b => b.Author);
                    break;
                case "genre":
                    books = books.OrderBy(b => b.Genre);
                    break;
                case "genre_desc":
                    books = books.OrderByDescending(b => b.Genre);
                    break;
                case "read":
                    books = books.OrderBy(b => b.IsRead);
                    break;
                case "read_desc":
                    books = books.OrderByDescending(b => b.IsRead);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Genre,PublicationYear,ISBN,IsRead,Notes")] Book book)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                book.DateAdded = DateTime.Now;
                if (book.IsRead)
                {
                    book.DateRead = DateTime.Now;
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Genre,PublicationYear,ISBN,IsRead,Notes,DateAdded")] Book book)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing book to preserve DateAdded
                    var existingBook = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
                    if (existingBook == null)
                    {
                        return NotFound();
                    }

                    // Update DateRead based on IsRead status
                    if (book.IsRead && !existingBook.IsRead)
                    {
                        book.DateRead = DateTime.Now;
                    }
                    else if (!book.IsRead)
                    {
                        book.DateRead = null;
                    }
                    else
                    {
                        book.DateRead = existingBook.DateRead;
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/MarkAsRead/5
        public async Task<IActionResult> MarkAsRead(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.IsRead = true;
            book.DateRead = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Books/MarkAsUnread/5
        public async Task<IActionResult> MarkAsUnread(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.IsRead = false;
            book.DateRead = null;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}