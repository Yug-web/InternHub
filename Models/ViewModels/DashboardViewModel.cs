namespace InternHub.Models.ViewModels
{
    /// <summary>
    /// ViewModel for the Dashboard page.
    /// Aggregates statistics and recent data for display.
    /// ViewModels carry shaped data from Controller → View.
    /// </summary>
    public class DashboardViewModel
    {
        // ── Stat Card Counts ───────────────────────────────────────────────────
        public int TotalApplications       { get; set; }
        public int AppliedCount            { get; set; }
        public int InterviewScheduledCount { get; set; }
        public int SelectedCount           { get; set; }
        public int RejectedCount           { get; set; }
        public int TotalCompanies          { get; set; }
        public int UpcomingDeadlinesCount  { get; set; }
        public int TotalInterviews         { get; set; }

        // ── Computed Stats ─────────────────────────────────────────────────────
        /// <summary>Success rate = Selected / Total (only when Total > 0)</summary>
        public double SuccessRate =>
            TotalApplications > 0
                ? Math.Round((double)SelectedCount / TotalApplications * 100, 1)
                : 0;

        /// <summary>Active applications = not Rejected and not Selected</summary>
        public int ActiveCount =>
            TotalApplications - RejectedCount - SelectedCount;

        // ── Recent Data for Dashboard Tables ──────────────────────────────────
        /// <summary>Last 5 applications added (most recent first)</summary>
        public List<Application> RecentApplications { get; set; } = new();

        /// <summary>Next 5 upcoming deadlines (soonest first)</summary>
        public List<Deadline> UpcomingDeadlines { get; set; } = new();

        /// <summary>Last 3 interviews logged</summary>
        public List<Interview> RecentInterviews { get; set; } = new();
    }
}
