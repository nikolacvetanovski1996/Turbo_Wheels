# Turbo Wheels

A car rental management web application built with ASP.NET MVC 5 and Entity Framework 6, featuring authentication, multi-step reservations, payment options, and an administrative dashboard.

> **Note:** This project started as my university thesis, then I significantly improved it until it became portfolio-quality and a practical app for managing car rentals.

## Features

- User registration and login system
- Vehicle listings with availability filtering
- Multi-step reservation process with client-side and server-side validation
- Credit card and pay-on-pickup payment options
- Reservation conflict detection to prevent overlapping bookings
- Admin controls for managing vehicles and users
- Admin controls for viewing and deleting reservations
- Session- and cookie-based authentication
- Reservation state persistence across reservation steps

## Technologies

- ASP.NET MVC 5
- Entity Framework 6 (Code First)
- SQL Server LocalDB
- Bootstrap 5
- jQuery
- jQuery UI Datepicker
- Font Awesome

## Setup Instructions

1. Clone the repository.
2. Update the connection string in `web.config` to point to your local SQL Server instance.
3. In Visual Studio, **perform a Clean Solution** (Build > Clean Solution). This step resolves any package or compiler file issues without manual NuGet restore.  
4. Build and run the project in Visual Studio (2019 or later).
5. The database will be created automatically on first run (if it doesn't already exist).
6. Access the application via `http://localhost:<port>/`.

## Troubleshooting

### Build errors related to missing compiler files (`roslyn\csc.exe`)

If you encounter errors about missing files like `roslyn\csc.exe` when building or running the project, try this:

1. In Visual Studio, go to **Build > Clean Solution**.  
2. After cleaning, build the solution again.

This cleans out temporary build artifacts and forces Visual Studio to re-extract necessary compiler files automatically, resolving the issue without manual NuGet restores.

---

If the problem persists after cleaning and rebuilding, double-check that all NuGet packages are properly restored by right-clicking the solution and selecting **Restore NuGet Packages**.

## Notes

- Custom 403, 404, and 500 error pages.
- Passwords are securely hashed before storage.
- A default administrator account is automatically seeded on first run if no administrator exists:
  - By default:
    - **Username:** admin
    - **Password:** admin123
  - These values are configurable via `Web.config`.
- Uses cookie- and session-based authentication.

## Admin Demo

The administrative interface is intentionally not publicly available.

For serious evaluation, temporary administrator access can be provided by:

- restoring a database backup to a sandbox environment, or
- providing a database backup that can be run locally.

If you would like to evaluate the administrative functionality, feel free to contact me. Temporary access can be provided upon request.