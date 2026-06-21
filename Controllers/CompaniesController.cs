using InternHub.Data;
using InternHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternHub.Controllers
{
    /// <summary>
    /// CompaniesController handles full CRUD for Company records.
    /// Follows the standard MVC pattern: Controller talks to DB, passes data to View.
    /// </summary>
    public class CompaniesController : Controller
    {
        private readonly AppDbContext _context;

        public CompaniesController(AppDbContext context)
        {
            _context = context;
        }

        // ── INDEX: List all companies (with optional search) ──────────────────
        // GET: /Companies
        // GET: /Companies?search=Google
        public async Task<IActionResult> Index(string? search)
        {
            // LINQ: Start with all companies query
            var query = _context.Companies.AsQueryable();

            // LINQ Where() — filter if search term is provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(search) ||
                    c.Industry.ToLower().Contains(search) ||
                    c.Location.ToLower().Contains(search));
            }

            // Pass search term back to view to keep it in the search box
            ViewData["CurrentSearch"] = search;
            ViewData["TotalCount"] = await query.CountAsync();

            var companies = await query.OrderBy(c => c.Name).ToListAsync();
            return View(companies);
        }

        // ── DETAILS: View a single company ────────────────────────────────────
        // GET: /Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Include() performs a JOIN to load related Applications and Interviews
            var company = await _context.Companies
                .Include(c => c.Applications)
                .Include(c => c.Interviews)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return NotFound();

            return View(company);
        }

        // ── CREATE: Show empty form ────────────────────────────────────────────
        // GET: /Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // ── CREATE: Handle form submission ────────────────────────────────────
        // POST: /Companies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]  // Prevents CSRF attacks
        public async Task<IActionResult> Create([Bind("Name,Industry,Location,Website,HREmail")] Company company)
        {
            if (ModelState.IsValid)
            {
                company.CreatedAt = DateTime.Now;
                _context.Add(company);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Company '{company.Name}' added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(company); // Re-show form with validation errors
        }

        // ── EDIT: Show pre-filled form ────────────────────────────────────────
        // GET: /Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return NotFound();
            return View(company);
        }

        // ── EDIT: Handle form submission ───────────────────────────────────────
        // POST: /Companies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Industry,Location,Website,HREmail,CreatedAt")] Company company)
        {
            if (id != company.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Company '{company.Name}' updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Companies.Any(c => c.Id == company.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // ── DELETE: Show confirmation page ─────────────────────────────────────
        // GET: /Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return NotFound();
            return View(company);
        }

        // ── DELETE: Handle confirmed deletion ──────────────────────────────────
        // POST: /Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Company '{company.Name}' deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
