# Brewery API - Documentation

##  Overview

This is a RESTful API built using ASP.NET Core 8. It consumes data from the Open Brewery DB API, caches it, persists it to a SQLite database using Entity Framework Core, and exposes brewery information through a versioned, secure API. Designed with SOLID principles in mind, it also includes logging, JWT authentication, and scalability features.

##  Features

* SQLite + EF Core database integration
* In-memory caching (10 min expiry)
* Search with autocomplete
* Sort by name, city, or distance
* JWT authentication
* API versioning (`/api/v1/...`)   
* Centralized error handling
* Logging via built-in ILogger

## Project Structure

BreweryApi/
├── Controllers/              # API endpoints
├── Services/                 # Business logic (e.g., BreweryService)
├── Models/                   # DTOs and EF Entities
├── Data/                     # AppDbContext, DB seed logic
├── Middleware/               # Error handling, logging middleware
├── Program.cs                # App startup and middleware setup
└── appsettings.json          # Configs for DB, JWT, etc.


##  Design Decisions

| Area               | Approach                                                                      |
| ------------------ | ----------------------------------------------------------------------------- |
| **Architecture**   | Follows SOLID principles. Separation of concerns via services and interfaces. |
| **Caching**        | Uses IMemoryCache with a 10-minute sliding expiration.                        |
| **Database**       | SQLite for local development; EF Core for ORM mapping.                        |
| **Security**       | JWT Bearer tokens using ASP.NET Core authentication middleware.               |
| **Error Handling** | Try/catch blocks with ILogger and global middleware.                          |


##  Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* [SQLite](https://www.sqlite.org/download.html) (for database inspection)

##  Run the API

### Step 1: Clone the repo

```bash
git clone https://github.com/yourusername/BreweryApi.git
cd BreweryApi
```

### Step 2: Restore and build

```bash
dotnet restore
dotnet build
```

### Step 3: Apply EF Core migrations

```bash
dotnet ef database update
```

### Step 4: Run the API

```bash
dotnet run
```

API will be accessible at:

* `https://localhost:7204`

---

##  API Testing

### Swagger

* Navigate to `https://localhost:7204/swagger`
* Try endpoints like:

  * `POST /api/v1/breweries/GetBreweries`
  * `GET /api/v1/Auth/login`
* Use "Authorize" to input JWT token

### Postman Example:
GET /api/v1/breweries?search=ale&sortBy=city
Authorization: Bearer <your_token>

### Sample Token
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

##  Database

SQLite database file: `Brewery.db`

sqlite3 Brewery.db.tables
SELECT * FROM Breweries;
