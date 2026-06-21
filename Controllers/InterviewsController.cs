using InternHub.Data;
using InternHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InternHub.Controllers
{
    /// <summary>
    /// InterviewsController handles CRUD for interview rounds.
    /// </summary>
    public class InterviewsController : Controller
    {
        private readonly AppDbContext _context;

        public InterviewsController(AppDbContext context)
        {
            _context = context;
        }

        // ── INDEX ──────────────────────────────────────────────────────────────
        public async Task<IActionResult> Index(string? typeFilter)
        {
            var query = _context.Interviews
                .Include(i => i.Company)
                .AsQueryable();

            // LINQ: Filter by interview type
            if (!string.IsNullOrWhiteSpace(typeFilter) &&
                Enum.TryParse<InterviewType>(typeFilter, out var parsedType))
            {
                query = query.Where(i => i.InterviewType == parsedType);
            }

            ViewData["CurrentType"] = typeFilter;
            ViewData["TypeList"]    = Enum.GetValues<InterviewType>()
                                         .Select(t => new SelectListItem(t.ToString(), t.ToString()));

            var interviews = await query.OrderByDescending(i => i.InterviewDate).ToListAsync();
            return View(interviews);
        }

        // ── DETAILS ────────────────────────────────────────────────────────────
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var interview = await _context.Interviews
                .Include(i => i.Company)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (interview == null) return NotFound();
            return View(interview);
        }

        // ── CREATE: Show form ──────────────────────────────────────────────────
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Name), "Id", "Name");
            return View();
        }

        // ── CREATE: Handle POST ────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CompanyId,Role,InterviewDate,InterviewType,Notes")]
            Interview interview)
        {
            if (ModelState.IsValid)
            {
                interview.CreatedAt = DateTime.Now;
                _context.Add(interview);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Interview added successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Name), "Id", "Name", interview.CompanyId);
            return View(interview);
        }

        // ── EDIT: Show form ────────────────────────────────────────────────────
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var interview = await _context.Interviews.FindAsync(id);
            if (interview == null) return NotFound();
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Name), "Id", "Name", interview.CompanyId);
            return View(interview);
        }

        // ── EDIT: Handle POST ──────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,CompanyId,Role,InterviewDate,InterviewType,Notes,CreatedAt")]
            Interview interview)
        {
            if (id != interview.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interview);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Interview updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Interviews.Any(i => i.Id == interview.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies.OrderBy(c => c.Name), "Id", "Name", interview.CompanyId);
            return View(interview);
        }

        // ── DELETE: Confirm page ───────────────────────────────────────────────
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var interview = await _context.Interviews
                .Include(i => i.Company)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (interview == null) return NotFound();
            return View(interview);
        }

        // ── DELETE: Handle POST ────────────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interview = await _context.Interviews.FindAsync(id);
            if (interview != null)
            {
                _context.Interviews.Remove(interview);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Interview deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
