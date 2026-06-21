using InternHub.Models;

namespace InternHub.Data
{
    /// <summary>
    /// DbSeeder populates the database with sample data on first run.
    /// This lets you see the app working immediately without manual data entry.
    /// </summary>
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Only seed if there is no data yet
            if (context.Companies.Any()) return;

            // ── Seed Companies ─────────────────────────────────────────────────
            var companies = new List<Company>
            {
                new Company
                {
                    Name = "Google",
                    Industry = "Technology",
                    Location = "Bengaluru, India",
                    Website = "https://careers.google.com",
                    HREmail = "hr@google.com",
                    CreatedAt = DateTime.Now.AddDays(-30)
                },
                new Company
                {
                    Name = "Microsoft",
                    Industry = "Technology",
                    Location = "Hyderabad, India",
                    Website = "https://careers.microsoft.com",
                    HREmail = "hr@microsoft.com",
                    CreatedAt = DateTime.Now.AddDays(-25)
                },
                new Company
                {
                    Name = "Amazon",
                    Industry = "E-Commerce / Cloud",
                    Location = "Bengaluru, India",
                    Website = "https://amazon.jobs",
                    HREmail = "hr@amazon.com",
                    CreatedAt = DateTime.Now.AddDays(-20)
                },
                new Company
                {
                    Name = "Infosys",
                    Industry = "IT Services",
                    Location = "Pune, India",
                    Website = "https://infosys.com/careers",
                    HREmail = "hr@infosys.com",
                    CreatedAt = DateTime.Now.AddDays(-15)
                },
                new Company
                {
                    Name = "Flipkart",
                    Industry = "E-Commerce",
                    Location = "Bengaluru, India",
                    Website = "https://flipkart.com/careers",
                    HREmail = "hr@flipkart.com",
                    CreatedAt = DateTime.Now.AddDays(-10)
                }
            };

            context.Companies.AddRange(companies);
            context.SaveChanges();

            // ── Seed Applications ──────────────────────────────────────────────
            var applications = new List<Application>
            {
                new Application
                {
                    CompanyId = companies[0].Id,
                    Role = "Software Engineering Intern",
                    Location = "Bengaluru, India",
                    Stipend = 80000,
                    ApplicationDate = DateTime.Now.AddDays(-28),
                    Deadline = DateTime.Now.AddDays(10),
                    Status = ApplicationStatus.InterviewScheduled,
                    Notes = "Applied via campus placement portal. OA completed successfully.",
                    CreatedAt = DateTime.Now.AddDays(-28)
                },
                new Application
                {
                    CompanyId = companies[1].Id,
                    Role = ".NET Developer Intern",
                    Location = "Hyderabad, India",
                    Stipend = 60000,
                    ApplicationDate = DateTime.Now.AddDays(-20),
                    Deadline = DateTime.Now.AddDays(5),
                    Status = ApplicationStatus.OAScheduled,
                    Notes = "Received OA link via email. Need to complete before deadline.",
                    CreatedAt = DateTime.Now.AddDays(-20)
                },
                new Application
                {
                    CompanyId = companies[2].Id,
                    Role = "SDE Intern",
                    Location = "Bengaluru, India",
                    Stipend = 90000,
                    ApplicationDate = DateTime.Now.AddDays(-15),
                    Deadline = DateTime.Now.AddDays(-2),
                    Status = ApplicationStatus.Rejected,
                    Notes = "Did not clear the OA round. Need to revise DSA.",
                    CreatedAt = DateTime.Now.AddDays(-15)
                },
                new Application
                {
                    CompanyId = companies[3].Id,
                    Role = "Full Stack Intern",
                    Location = "Pune, India",
                    Stipend = 25000,
                    ApplicationDate = DateTime.Now.AddDays(-10),
                    Deadline = DateTime.Now.AddDays(15),
                    Status = ApplicationStatus.Applied,
                    Notes = "Applied through Infosys InStep program.",
                    CreatedAt = DateTime.Now.AddDays(-10)
                },
                new Application
                {
                    CompanyId = companies[4].Id,
                    Role = "Backend Engineering Intern",
                    Location = "Bengaluru, India",
                    Stipend = 50000,
                    ApplicationDate = DateTime.Now.AddDays(-5),
                    Deadline = DateTime.Now.AddDays(20),
                    Status = ApplicationStatus.Selected,
                    Notes = "Received offer letter! Joining date: next month.",
                    CreatedAt = DateTime.Now.AddDays(-5)
                }
            };

            context.Applications.AddRange(applications);
            context.SaveChanges();

            // ── Seed Interviews ────────────────────────────────────────────────
            var interviews = new List<Interview>
            {
                new Interview
                {
                    CompanyId = companies[0].Id,
                    Role = "Software Engineering Intern",
                    InterviewDate = DateTime.Now.AddDays(3),
                    InterviewType = InterviewType.Technical,
                    Notes = "Round 1: DSA - Arrays, Trees, Graphs. Prepare LeetCode Medium level.",
                    CreatedAt = DateTime.Now.AddDays(-5)
                },
                new Interview
                {
                    CompanyId = companies[0].Id,
                    Role = "Software Engineering Intern",
                    InterviewDate = DateTime.Now.AddDays(7),
                    InterviewType = InterviewType.HR,
                    Notes = "HR Round: Prepare for behavioural questions (STAR method).",
                    CreatedAt = DateTime.Now.AddDays(-5)
                },
                new Interview
                {
                    CompanyId = companies[4].Id,
                    Role = "Backend Engineering Intern",
                    InterviewDate = DateTime.Now.AddDays(-3),
                    InterviewType = InterviewType.Coding,
                    Notes = "Online coding test - 2 DSA problems. Both solved correctly. Received offer!",
                    CreatedAt = DateTime.Now.AddDays(-10)
                }
            };

            context.Interviews.AddRange(interviews);
            context.SaveChanges();

            // ── Seed Deadlines ─────────────────────────────────────────────────
            var deadlines = new List<Deadline>
            {
                new Deadline
                {
                    Title = "Microsoft OA Deadline",
                    DueDate = DateTime.Now.AddDays(5),
                    Description = "Complete the Online Assessment for Microsoft .NET Intern role before this date.",
                    Priority = Priority.High,
                    CreatedAt = DateTime.Now.AddDays(-2)
                },
                new Deadline
                {
                    Title = "Resume Update",
                    DueDate = DateTime.Now.AddDays(3),
                    Description = "Add new projects and internship experience to resume before applying to new companies.",
                    Priority = Priority.Medium,
                    CreatedAt = DateTime.Now.AddDays(-1)
                },
                new Deadline
                {
                    Title = "LeetCode Practice",
                    DueDate = DateTime.Now.AddDays(14),
                    Description = "Complete 50 medium-level DSA problems before Google interview.",
                    Priority = Priority.High,
                    CreatedAt = DateTime.Now
                },
                new Deadline
                {
                    Title = "Infosys Application Deadline",
                    DueDate = DateTime.Now.AddDays(15),
                    Description = "Final date to apply for Infosys InStep internship program.",
                    Priority = Priority.Medium,
                    CreatedAt = DateTime.Now
                },
                new Deadline
                {
                    Title = "Update LinkedIn Profile",
                    DueDate = DateTime.Now.AddDays(7),
                    Description = "Add recent projects, skills, and endorsements to LinkedIn.",
                    Priority = Priority.Low,
                    CreatedAt = DateTime.Now
                }
            };

            context.Deadlines.AddRange(deadlines);
            context.SaveChanges();
        }
    }
}
