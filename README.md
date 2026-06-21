# InternHub 🎓

InternHub is a simple, easy-to-use ASP.NET Core MVC web application I built to help students (like myself) keep track of their internship applications. 

When applying to dozens of companies, it's easy to lose track of where you applied, who you interviewed with, and what deadlines are coming up. I built this tool to solve that problem while getting hands-on experience with the .NET ecosystem.

**Live Demo:** [http://internhub.somee.com](http://internhub.somee.com)

---

### What it does

- **Company Tracker:** Add and manage companies you're interested in.
- **Application Status:** Track whether you've just applied, are interviewing, got an offer, or were rejected.
- **Interview Logs:** Keep notes on when your interviews are happening and what type of interview it is (Technical, HR, etc.).
- **Deadlines:** Never miss an application or assessment deadline. 

### Tech Stack

I kept the architecture straightforward to focus on mastering the core concepts of ASP.NET and relational databases:

- **Framework:** ASP.NET Core 8.0 MVC
- **Database:** SQL Server (Entity Framework Core)
- **Frontend:** HTML, Razor Views, and Bootstrap 5
- **Data Access:** LINQ and Repository-like patterns directly in the controllers.

### Local Setup

If you want to pull this down and run it on your own machine:

1. Clone the repo:
   ```bash
   git clone https://github.com/Yug-web/InternHub.git
   ```
2. Make sure you have the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download) installed.
3. Open a terminal in the project folder and run:
   ```bash
   dotnet ef database update
   ```
   *(This will create the SQL Server database using EF Core)*
4. Run the app:
   ```bash
   dotnet run
   ```
5. Open your browser and go to `http://localhost:5000`

### Database Design

The database has 4 main tables that are linked together using Entity Framework Core foreign keys:
- **Companies:** The core table. If a company is deleted, all related applications and interviews are deleted with it (Cascade Delete).
- **Applications:** Linked to a Company. Tracks the role and status.
- **Interviews:** Linked to an Application. Tracks rounds and dates.
- **Deadlines:** Linked to a Company. Tracks upcoming due dates.

---
