using InternHub.Data;
using InternHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InternHub.Controllers
{
    /// <summary>
    /// ApplicationsController handles CRUD for internship applications.
    /// Supports search by company/role and filter by status.
    /// </summary>
    public class ApplicationsController : Controller
    {
        private readonly AppDbContext _context;

        public ApplicationsController(AppDbContext context)
        {
            _context = context;
        }

        // ── INDEX: List with search + status filter ────────────────────────────
        // GET: /Applications
        // GET: /Applications?search=Google&statusFilter=Applied
        public async Task<IActionResult> Index(string? search, string? statusFilter)
        {
            var query = _context.Applications
                .Include(a => a.Company)    // Eager load Company for each Application
                .AsQueryable();

            // LINQ: Filter by search keyword
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(a =>
                    a.Role.ToLower().Contains(search) ||
                    a.Company!.Name.ToLower().Contains(search) ||
                    a.Location.ToLower().Contains(search));
            }

            // LINQ: Filter by status enum
            if (!string.IsNullOrWhiteSpace(statusFilter) &&
                Enum.TryParse<ApplicationStatus>(statusFilter, out var parsedStatus))
            {
                query = query.Where(a => a.Status == parsedStatus);
            }

            // Pass filter values back to view
            ViewData["CurrentSearch"]      = search;
            ViewData["CurrentStatus"]      = statusFilter;
            ViewData["StatusList"]         = Enum.GetValues<ApplicationStatus>()
                                                 .Select(s => new SelectListItem(s.ToString(), s.ToString()));

            var applications = await query.OrderByDescending(a => a.ApplicationDate).ToListAsync();
            return View(applications);
        }

        // ── DETAILS ────────────────────────────────────────────────────────────
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var application = await _context.Applications
                .Include(a => a.Company)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null) return NotFound();
            return View(application);
        }

        // ── CREATE: Show form ──────────────────────────────────────────────────
        public IActionResult Create()
        {
            // Populate dropdown with all companies
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Name), "Id", "Name");
            return View();
        }

        // ── CREATE: Handle POST ────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CompanyId,Role,Location,Stipend,ApplicationDate,Deadline,Status,Notes")]
            Application application)
        {
            if (ModelState.IsValid)
            {
                application.CreatedAt = DateTime.Now;
                _context.Add(application);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Application added successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Name), "Id", "Name", application.CompanyId);
            return View(application);
        }

        // ── EDIT: Show form ────────────────────────────────────────────────────
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var application = await _context.Applications.FindAsync(id);
            if (application == null) return NotFound();
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Name), "Id", "Name", application.CompanyId);
            return View(application);
        }

        // ── EDIT: Handle POST ──────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,CompanyId,Role,Location,Stipend,ApplicationDate,Deadline,Status,Notes,CreatedAt")]
            Application application)
        {
            if (id != application.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Application updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Applications.Any(a => a.Id == application.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Name), "Id", "Name", application.CompanyId);
            return View(application);
        }

        // ── DELETE: Confirm page ───────────────────────────────────────────────
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var application = await _context.Applications
                .Include(a => a.Company)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (application == null) return NotFound();
            return View(application);
        }

        // ── DELETE: Handle POST ────────────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application != null)
            {
                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Application deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
