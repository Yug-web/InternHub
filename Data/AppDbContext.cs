using InternHub.Models;
using Microsoft.EntityFrameworkCore;

namespace InternHub.Data
{
    /// <summary>
    /// AppDbContext is the main Entity Framework Core Database Context.
    /// It acts as the bridge between the C# application and SQL Server.
    /// Each DbSet<T> maps to a database table.
    /// </summary>
    public class AppDbContext : DbContext
    {
        // Constructor — receives options (connection string) via Dependency Injection
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ── DbSets (Database Tables) ───────────────────────────────────────────
        public DbSet<Company>     Companies    { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Interview>   Interviews   { get; set; }
        public DbSet<Deadline>    Deadlines    { get; set; }

        /// <summary>
        /// Configure table relationships, constraints, indexes, and enum storage.
        /// This runs when EF Core builds or updates the database schema.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Relationships ──────────────────────────────────────────────────

            // Company → Applications: One-to-Many with Cascade Delete
            // Deleting a Company automatically deletes all its Applications
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Company)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Company → Interviews: One-to-Many with Cascade Delete
            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Company)
                .WithMany(c => c.Interviews)
                .HasForeignKey(i => i.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Store Enums as Strings for DB Readability ──────────────────────
            modelBuilder.Entity<Application>()
                .Property(a => a.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Interview>()
                .Property(i => i.InterviewType)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Deadline>()
                .Property(d => d.Priority)
                .HasConversion<string>()
                .HasMaxLength(20);

            // ── Database Indexes for Performance ───────────────────────────────
            // Index on Company.Name — speeds up company search queries
            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Name)
                .HasDatabaseName("IX_Companies_Name");

            // Index on Application.Status — speeds up status filter queries
            modelBuilder.Entity<Application>()
                .HasIndex(a => a.Status)
                .HasDatabaseName("IX_Applications_Status");

            // Index on Application.ApplicationDate — speeds up date-based sorting
            modelBuilder.Entity<Application>()
                .HasIndex(a => a.ApplicationDate)
                .HasDatabaseName("IX_Applications_ApplicationDate");

            // Index on Deadline.DueDate — speeds up upcoming deadlines queries
            modelBuilder.Entity<Deadline>()
                .HasIndex(d => d.DueDate)
                .HasDatabaseName("IX_Deadlines_DueDate");
        }
    }
}
