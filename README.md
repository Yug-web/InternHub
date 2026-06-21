# 🎓 InternHub — Internship Application Tracker

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?logo=dotnet)
![Entity Framework](https://img.shields.io/badge/Entity_Framework_Core-8.0-512BD4?logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-CC2927?logo=microsoftsqlserver)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap)
![License](https://img.shields.io/badge/License-MIT-green)

> A professional **ASP.NET Core 8 MVC** web application for tracking internship applications, interviews, and deadlines. Built as a portfolio project demonstrating real-world .NET development skills.

---

## 📋 Project Overview

InternHub helps students stay organized during their internship search by providing a centralized tracker for:

- 🏢 **Companies** — Manage target companies with contact info
- 📄 **Applications** — Track applications with status pipeline
- 🎥 **Interviews** — Log technical, HR, and coding rounds
- ⏰ **Deadlines** — Never miss OA dates or submission deadlines
- 📊 **Dashboard** — At-a-glance statistics and success rate

---

## ✨ Features

| Feature | Description |
|---|---|
| Full CRUD | Create, Read, Update, Delete for all 4 modules |
| Status Pipeline | Applied → OA → Interview → Selected / Rejected |
| Search & Filter | Search by company/role, filter by status/type/priority |
| Dashboard Stats | Total apps, success rate, active count, upcoming deadlines |
| Smart Alerts | Days-remaining counter, overdue detection, TODAY highlight |
| Responsive UI | Mobile-friendly Bootstrap 5 dark theme |
| Data Seeding | Auto-seeds sample data on first run |
| Cascade Delete | Deleting a company removes all related records |

---

## 🛠️ Technology Stack

| Technology | Version | Purpose |
|---|---|---|
| **ASP.NET Core MVC** | 8.0 | Web framework (Controllers, Views, Routing) |
| **Entity Framework Core** | 8.0 | ORM — database access using C# |
| **SQL Server LocalDB** | 2022 | Database (MSSQL) |
| **LINQ** | — | Querying database with C# |
| **Razor Views** | — | Server-side HTML templating (.cshtml) |
| **Bootstrap 5.3** | 5.3.2 | Responsive UI framework |
| **Bootstrap Icons** | 1.11 | Icon library |
| **C# 12** | — | Primary programming language |

---

## 🏛️ MVC Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Browser / Client                         │
└───────────────────────────┬─────────────────────────────────┘
                            │  HTTP Request (URL)
                            ▼
┌─────────────────────────────────────────────────────────────┐
│               ASP.NET Core Router                           │
│         {controller}/{action}/{id?}                         │
└───────────────────────────┬─────────────────────────────────┘
                            │
                            ▼
┌─────────────────── CONTROLLERS ─────────────────────────────┐
│  HomeController          → Dashboard with LINQ stats        │
│  CompaniesController     → Company CRUD + search            │
│  ApplicationsController  → Application CRUD + filter        │
│  InterviewsController    → Interview CRUD + type filter      │
│  DeadlinesController     → Deadline CRUD + priority filter  │
└───────────┬───────────────────────────┬─────────────────────┘
            │ queries via EF Core        │ returns to
            ▼                           ▼
┌─────── DATA LAYER ──────┐   ┌────── VIEWS (.cshtml) ───────┐
│  AppDbContext           │   │  Razor templates             │
│  Entity Framework Core  │   │  Bootstrap 5 components      │
│  LINQ Queries           │   │  Tag Helpers                 │
│  SQL Server LocalDB     │   │  Form Validation             │
└─────────────────────────┘   └──────────────────────────────┘
```

---

## 🗃️ Database Design

### Entity Relationship Diagram

```
Companies (PK: Id)
├── Id         INT          PRIMARY KEY  AUTO-INCREMENT
├── Name       NVARCHAR(100) NOT NULL    [INDEX]
├── Industry   NVARCHAR(50)  NOT NULL
├── Location   NVARCHAR(100) NOT NULL
├── Website    NVARCHAR(200) NULL
├── HREmail    NVARCHAR(100) NULL
└── CreatedAt  DATETIME2    NOT NULL

Applications (PK: Id, FK: CompanyId → Companies.Id CASCADE)
├── Id              INT          PRIMARY KEY
├── CompanyId       INT          FOREIGN KEY → Companies.Id
├── Role            NVARCHAR(100) NOT NULL
├── Location        NVARCHAR(100) NOT NULL
├── Stipend         INT          NULL
├── ApplicationDate DATETIME2    NOT NULL   [INDEX]
├── Deadline        DATETIME2    NULL
├── Status          NVARCHAR(50) NOT NULL   [INDEX: Applied/OA/Interview/Selected/Rejected]
├── Notes           NVARCHAR(500) NULL
└── CreatedAt       DATETIME2    NOT NULL

Interviews (PK: Id, FK: CompanyId → Companies.Id CASCADE)
├── Id            INT          PRIMARY KEY
├── CompanyId     INT          FOREIGN KEY → Companies.Id
├── Role          NVARCHAR(100) NOT NULL
├── InterviewDate DATETIME2    NOT NULL
├── InterviewType NVARCHAR(50) NOT NULL   [Technical / HR / Coding]
├── Notes         NVARCHAR(1000) NULL
└── CreatedAt     DATETIME2    NOT NULL

Deadlines (PK: Id)
├── Id          INT          PRIMARY KEY
├── Title       NVARCHAR(100) NOT NULL
├── DueDate     DATETIME2    NOT NULL    [INDEX]
├── Description NVARCHAR(500) NULL
├── Priority    NVARCHAR(20) NOT NULL    [High / Medium / Low]
└── CreatedAt   DATETIME2    NOT NULL
```

### Relationships
- `Company` → `Applications`: **One-to-Many**, Cascade Delete
- `Company` → `Interviews`: **One-to-Many**, Cascade Delete

---

## ⚙️ Installation & Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- SQL Server Express LocalDB (comes with Visual Studio) or SQL Server

### Quick Start

```bash
# 1. Clone the repository
git clone https://github.com/YOUR_USERNAME/InternHub.git
cd InternHub

# 2. Restore NuGet packages
dotnet restore

# 3. Run the application (auto-creates DB, runs migrations, seeds data)
dotnet run
```

Open your browser at **`https://localhost:5001`** or **`http://localhost:5000`**

### Connection String (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=InternHubDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

> **For full SQL Server:** Replace `(localdb)\\MSSQLLocalDB` with `your-server-name`

---

## 📁 Project Structure

```
InternHub/
├── Controllers/                    ← Handle requests, call DB, return Views
│   ├── HomeController.cs           ← Dashboard (LINQ stats aggregation)
│   ├── CompaniesController.cs      ← Company CRUD + search
│   ├── ApplicationsController.cs   ← Application CRUD + search + status filter
│   ├── InterviewsController.cs     ← Interview CRUD + type filter
│   └── DeadlinesController.cs      ← Deadline CRUD + priority filter
│
├── Models/                         ← C# classes → SQL Server tables
│   ├── Company.cs
│   ├── Application.cs
│   ├── Interview.cs
│   ├── Deadline.cs
│   ├── Enums.cs                    ← ApplicationStatus, InterviewType, Priority
│   └── ViewModels/
│       └── DashboardViewModel.cs   ← Aggregated stats for Dashboard
│
├── Data/                           ← Database layer
│   ├── AppDbContext.cs             ← EF Core DbContext (relationships, indexes)
│   └── DbSeeder.cs                 ← Seeds sample data on first run
│
├── Views/                          ← Razor HTML templates
│   ├── Shared/
│   │   ├── _Layout.cshtml          ← Master layout (navbar, footer, toasts)
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── Home/Index.cshtml           ← Dashboard
│   ├── Companies/                  ← Index, Create, Edit, Details, Delete
│   ├── Applications/               ← Index, Create, Edit, Details, Delete
│   ├── Interviews/                 ← Index, Create, Edit, Details, Delete
│   └── Deadlines/                  ← Index, Create, Edit, Details, Delete
│
├── Migrations/                     ← EF Core migration history
├── wwwroot/
│   ├── css/site.css                ← Dark navy custom theme
│   └── js/site.js                  ← Nav highlighting, counter animations
│
├── appsettings.json                ← Configuration (connection string)
├── Program.cs                      ← App startup, DI, middleware pipeline
└── InternHub.csproj                ← Project file (NuGet packages)
```

---

## 📸 Screenshots

> Add screenshots of your running application here:

| Page | Screenshot |
|---|---|
| Dashboard | *(Add screenshot)* |
| Applications List | *(Add screenshot)* |
| Add Application | *(Add screenshot)* |
| Companies List | *(Add screenshot)* |
| Deadline Tracker | *(Add screenshot)* |
| Interview Tracker | *(Add screenshot)* |

**To take screenshots:**
1. Run the app with `dotnet run`
2. Open `http://localhost:5000` in Chrome
3. Press `F12` → Device toolbar (mobile view) or `Win + Shift + S` for snip

---

## 🔧 Useful Commands

```bash
# Run the application
dotnet run

# Build without running
dotnet build

# Add a new EF Core migration (after changing models)
dotnet ef migrations add YourMigrationName

# Apply migrations to database
dotnet ef database update

# Drop and recreate database
dotnet ef database drop --force
dotnet ef database update

# Publish for deployment
dotnet publish -c Release -o ./publish
```

---

## 🔄 Application Status Flow

```
Applied ──→ OA Scheduled ──→ OA Completed ──→ Interview Scheduled ──→ Selected ✅
                                                                    └──→ Rejected ❌
```

---

## 🚀 Future Improvements

- [ ] **ASP.NET Core Identity** — User accounts (each student sees their own data)
- [ ] **Email Notifications** — Deadline reminders via IEmailSender
- [ ] **Chart.js Statistics** — Pie/bar charts for application breakdown
- [ ] **Resume Upload** — Attach PDF resume per application (IFormFile)
- [ ] **CSV Export** — Export applications to Excel via CsvHelper
- [ ] **Pagination** — Page through large application lists
- [ ] **Notes Rich Text** — Markdown support for interview notes

---

## 🎤 Key Talking Points (Interview Ready)

- **MVC Pattern**: Separation of concerns — Models (data shape), Controllers (logic), Views (UI)
- **EF Core Code-First**: Wrote C# classes → EF generated SQL Server schema via migrations
- **LINQ**: Used `Where()`, `Include()`, `OrderByDescending()`, `CountAsync()`, `Take()` throughout
- **Dependency Injection**: AppDbContext injected into all controllers via constructor
- **Data Annotations**: `[Required]`, `[StringLength]`, `[EmailAddress]` for model-level validation
- **Relationships**: One-to-Many with Cascade Delete (Company → Applications, Interviews)
- **Async/Await**: All controller actions are async for non-blocking DB operations
- **Database Indexes**: Added on Name, Status, ApplicationDate, DueDate for query performance

---

## 📝 License

MIT License — free to use for portfolio and learning purposes.

---

*Built with ❤️ using **ASP.NET Core 8 MVC** | .NET Developer Portfolio Project*
