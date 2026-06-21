using InternHub.Data;
using InternHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternHub.Controllers
{
    /// <summary>
    /// DeadlinesController handles CRUD for deadline tracking.
    /// </summary>
    public class DeadlinesController : Controller
    {
        private readonly AppDbContext _context;

        public DeadlinesController(AppDbContext context)
        {
            _context = context;
        }

        // ── INDEX ──────────────────────────────────────────────────────────────
        public async Task<IActionResult> Index(string? priorityFilter)
        {
            var query = _context.Deadlines.AsQueryable();

            // LINQ: Filter by priority
            if (!string.IsNullOrWhiteSpace(priorityFilter) &&
                Enum.TryParse<Priority>(priorityFilter, out var parsedPriority))
            {
                query = query.Where(d => d.Priority == parsedPriority);
            }

            ViewData["CurrentPriority"] = priorityFilter;

            // LINQ: Order by soonest deadline first
            var deadlines = await query.OrderBy(d => d.DueDate).ToListAsync();
            return View(deadlines);
        }

        // ── DETAILS ────────────────────────────────────────────────────────────
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var deadline = await _context.Deadlines.FindAsync(id);
            if (deadline == null) return NotFound();
            return View(deadline);
        }

        // ── CREATE: Show form ──────────────────────────────────────────────────
        public IActionResult Create()
        {
            return View();
        }

        // ── CREATE: Handle POST ────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,DueDate,Description,Priority")]
            Deadline deadline)
        {
            if (ModelState.IsValid)
            {
                deadline.CreatedAt = DateTime.Now;
                _context.Add(deadline);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Deadline added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(deadline);
        }

        // ── EDIT: Show form ────────────────────────────────────────────────────
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var deadline = await _context.Deadlines.FindAsync(id);
            if (deadline == null) return NotFound();
            return View(deadline);
        }

        // ── EDIT: Handle POST ──────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Title,DueDate,Description,Priority,CreatedAt")]
            Deadline deadline)
        {
            if (id != deadline.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deadline);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Deadline updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Deadlines.Any(d => d.Id == deadline.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(deadline);
        }

        // ── DELETE: Confirm page ───────────────────────────────────────────────
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var deadline = await _context.Deadlines.FindAsync(id);
            if (deadline == null) return NotFound();
            return View(deadline);
        }

        // ── DELETE: Handle POST ────────────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deadline = await _context.Deadlines.FindAsync(id);
            if (deadline != null)
            {
                _context.Deadlines.Remove(deadline);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Deadline deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
