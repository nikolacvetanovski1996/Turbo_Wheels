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
- Bootstrap 5 for responsive UI  

## Setup Instructions

1. Clone the repository.  
2. Update the connection string in `web.config` to point to your local SQL Server instance.  
3. Build and run the project in Visual Studio (2019 or later).  
4. The database will be created automatically on first run (if it doesn't already exist).  
5. Access the app via `http://localhost:<port>/`.  

## Notes

- Basic error handling in place (default MVC error pages).  
- Seed admin user added on first run.  
- Uses cookie/session for simple authentication.  
