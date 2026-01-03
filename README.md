# Turbo Wheels

A straightforward car rental management web app built with ASP.NET MVC and Entity Framework.

> **Note:** This project started as my university thesis, then I significantly improved it until it became portfolio-quality and a practical app for managing car rentals.

## Features

- User registration and login system
- Vehicle listings with availability filtering
- Multi-step booking process with validation
- Admin controls for managing vehicles and users (full CRUD)
- Admin controls for managing reservations (view and delete only)
- Session and cookie based user management

## Technologies

- ASP.NET MVC 5
- Entity Framework 6 (Code First)
- SQL Server LocalDB
- Bootstrap 5 for responsive, mobile-friendly UI

## Setup Instructions

1. Clone the repository.
2. Update the connection string in `web.config` to point to your local SQL Server instance.
3. In Visual Studio, **perform a Clean Solution** (Build > Clean Solution). This step resolves any package or compiler file issues without manual NuGet restore.  
4. Build and run the project in Visual Studio (2019 or later).
5. The database will be created automatically on first run (if it doesn't already exist).
6. Access the app via `http://localhost:<port>/`.

## Troubleshooting

### Build errors related to missing compiler files (`roslyn\csc.exe`)

If you encounter errors about missing files like `roslyn\csc.exe` when building or running the project, try this:

1. In Visual Studio, go to **Build > Clean Solution**.  
2. After cleaning, build the solution again.

This cleans out temporary build artifacts and forces Visual Studio to re-extract necessary compiler files automatically, resolving the issue without manual NuGet restores.

---

If the problem persists after cleaning and rebuilding, double-check that all NuGet packages are properly restored by right-clicking the solution and selecting **Restore NuGet Packages**.

## Notes

- Basic error handling in place (default MVC error pages).
- Seed admin user added on first run:
  - **Username:** admin
  - **Password:** admin123
- Uses cookie/session for simple authentication. 
