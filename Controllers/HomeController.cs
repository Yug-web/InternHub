using InternHub.Data;
using InternHub.Models;
using InternHub.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternHub.Controllers
{
    /// <summary>
    /// HomeController handles the Dashboard page.
    /// Uses LINQ to aggregate statistics from SQL Server via Entity Framework Core.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor Injection — AppDbContext is provided by ASP.NET Core's DI container
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: / or /Home/Index
        /// Builds DashboardViewModel with aggregate stats and recent data.
        /// Each CountAsync() → single SQL COUNT query
        /// Include() → SQL JOIN with Companies table
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                // ── LINQ Aggregate Queries (each = 1 SQL COUNT statement) ────────
                TotalApplications       = await _context.Applications.CountAsync(),
                AppliedCount            = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Applied),
                InterviewScheduledCount = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.InterviewScheduled),
                SelectedCount           = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Selected),
                RejectedCount           = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Rejected),
                TotalCompanies          = await _context.Companies.CountAsync(),
                UpcomingDeadlinesCount  = await _context.Deadlines.CountAsync(d => d.DueDate >= DateTime.Now),
                TotalInterviews         = await _context.Interviews.CountAsync(),

                // ── Recent 5 Applications (newest first) with Company JOIN ────────
                RecentApplications = await _context.Applications
                    .Include(a => a.Company)
                    .OrderByDescending(a => a.CreatedAt)
                    .Take(5)
                    .ToListAsync(),

                // ── Next 5 Upcoming Deadlines (soonest first) ────────────────────
                UpcomingDeadlines = await _context.Deadlines
                    .Where(d => d.DueDate >= DateTime.Now.Date)
                    .OrderBy(d => d.DueDate)
                    .Take(5)
                    .ToListAsync(),

                // ── Last 3 Interviews logged with Company JOIN ────────────────────
                RecentInterviews = await _context.Interviews
                    .Include(i => i.Company)
                    .OrderByDescending(i => i.InterviewDate)
                    .Take(3)
                    .ToListAsync()
            };

            return View(viewModel);
        }
    }
}
