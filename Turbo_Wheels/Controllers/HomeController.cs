using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Turbo_Wheels.Filters;
using Turbo_Wheels.Models;
using Turbo_Wheels.Models.ViewModels;

namespace Turbo_Wheels.Controllers
{
    [RequireLogin]
    public class HomeController : Controller
    {
        private Turbo_WheelsContext db = new Turbo_WheelsContext();

        public ActionResult RootRedirect()
        {
            var isLoggedInCookie = Request.Cookies["hasLogged"];
            var isAdminCookie = Request.Cookies["isAdmin"];
            var userSession = Session["user"];

            // Basic check: if session user exists or cookies say logged in, redirect to Index
            if (userSession != null ||
                (isLoggedInCookie != null && isLoggedInCookie.Value == "True"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [NoCache]
        [AllowAnonymous]
        public ActionResult Login()
        {
            Response.Cookies["hasLogged"].Value = false.ToString();
            Response.Cookies["nameSurname"].Value = "";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoggedInViewModel loggedInViewModel)
        {
            if (!ModelState.IsValid)
                return View(loggedInViewModel);

            User user = db.Users.FirstOrDefault(u => u.Username == loggedInViewModel.Username);
            if (user == null || !Crypto.VerifyHashedPassword(user.Password, loggedInViewModel.Password))
            {
                ModelState.AddModelError("", "Invalid username or password.");

                // Clear password field
                loggedInViewModel.Password = "";

                return View(loggedInViewModel);
            }
           
            Response.Cookies["isAdmin"].Value = user.IsAdmin.ToString();
            Response.Cookies["nameSurname"].Value = user.FirstName + " " + user.LastName;
            Session["user"] = user;
            Response.Cookies["hasLogged"].Value = true.ToString();

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            // Clear session
            Session.Clear();
            Session.Abandon();

            // Clear cookies
            if (Request.Cookies["isAdmin"] != null)
            {
                var cookie = new HttpCookie("isAdmin");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            if (Request.Cookies["nameSurname"] != null)
            {
                var cookie = new HttpCookie("nameSurname");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            if (Request.Cookies["hasLogged"] != null)
            {
                var cookie = new HttpCookie("hasLogged");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            // Redirect to Login page
            return RedirectToAction("Login");
        }

        [NoCache]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.Cookies["isAdmin"] == null)
            {
                Response.Cookies["isAdmin"].Value = false.ToString();
            }
            return View();
        }

        [NoCache]
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var userExists = db.Users.Any(u => u.Username == user.Username);
            if (userExists)
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(user);
            }

            // Trim and normalize username and email
            user.Username = user.Username.Trim();
            user.Email = user.Email.Trim().ToLowerInvariant();

            // New users are not admins by default
            user.IsAdmin = false;

            // Hash the password before saving
            user.Password = Crypto.HashPassword(user.Password);

            db.Users.Add(user);
            db.SaveChanges();

            return RedirectToAction("Login", "Home");
        }

        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(ReservationInputViewModel reservationInputViewModel)
        {
            var pickupDateTime = reservationInputViewModel.PickupDate.Date + reservationInputViewModel.PickupTime;
            var returnDateTime = reservationInputViewModel.ReturnDate.Date + reservationInputViewModel.ReturnTime;

            if (pickupDateTime >= returnDateTime)
            {
                ModelState.AddModelError("", "Return date/time must be after pickup date/time.");
                return View(); // Return the same view so user can fix
            }

            ReservationViewModel reservationViewModel = new ReservationViewModel();
            reservationViewModel.PickupPlace = reservationInputViewModel.PickupPlace;
            reservationViewModel.ReturnPlace = reservationInputViewModel.ReturnPlace;
            reservationViewModel.PickupDateTime = pickupDateTime;
            reservationViewModel.ReturnDateTime = returnDateTime;
            Session["rvm"] = reservationViewModel;
            return RedirectToAction("Step1");
        }

        public ActionResult About ()
        {
            return View();
        }

        public ActionResult Step1 ()
        {
            ReservationViewModel reservationViewModel = (ReservationViewModel)Session["rvm"];

            if (reservationViewModel == null)
            {
                return RedirectToAction("Index");
            }

            // Find vehicle IDs that conflict
            var busyVehicleIds = db.Reservations
                .Where(res =>
                    !(res.ReturnDateTime <= reservationViewModel.PickupDateTime ||
                      res.PickupDateTime >= reservationViewModel.ReturnDateTime))
                .Select(r => r.VehicleID)
                .Distinct()
                .ToList();

            var availableVehicles = db.Vehicles
                .Where(v => !busyVehicleIds.Contains(v.VehicleID))
                .ToList();

            return View(availableVehicles);
        }

        public ActionResult Step2 (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }

            Session["vehicle"] = vehicle;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step2 (CreditCardViewModel creditCardViewModel)
        {
            if (Session["rvm"] == null || Session["vehicle"] == null || Session["user"] == null)
            {
                return RedirectToAction("Index"); // Session expired, restart booking
            }

            // Strip spaces and possibly other non-digit chars before validation
            if (creditCardViewModel.CreditCardNumber != null)
            {
                creditCardViewModel.CreditCardNumber = creditCardViewModel.CreditCardNumber.Replace(" ", "");

                // Clear the existing model state error for CreditCardNumber, if any
                ModelState.Remove(nameof(creditCardViewModel.CreditCardNumber));
            }

            if (!ModelState.IsValid)
            {
                return View(creditCardViewModel);
            }

            DateTime expiryDate;
            
            try
            {
                int year = creditCardViewModel.ExpireYear;
                if (year < 100)
                {
                    int currentCentury = DateTime.UtcNow.Year / 100 * 100;
                    int fullYear = currentCentury + year;

                    if (fullYear < DateTime.UtcNow.Year)
                    {
                        fullYear += 100; // Next century if expired or past 
                    }
                    year = fullYear;

                }

                expiryDate = new DateTime(year, creditCardViewModel.ExpireMonth,
                    DateTime.DaysInMonth(year, creditCardViewModel.ExpireMonth), 23, 59, 59);
            }
            catch
            {
                ModelState.AddModelError("", "Invalid expiration date.");
                return View(creditCardViewModel);
            }

            if (expiryDate <= DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Credit card expiration date must be in the future.");
                return View(creditCardViewModel);
            }

            ReservationViewModel reservationViewModel = (ReservationViewModel)Session["rvm"];
            Vehicle vehicle = (Vehicle)Session["vehicle"];
            User user = (User)Session["user"];

            if (reservationViewModel == null || vehicle == null || user == null)
            {
                return RedirectToAction("Index");
            }

            // Validate pickup < return again
            if (reservationViewModel.PickupDateTime >= reservationViewModel.ReturnDateTime)
            {
                ModelState.AddModelError("", "Return date/time must be after pickup date/time.");
                return View(creditCardViewModel);
            }

            // Check vehicle availability for overlap
            bool conflict = db.Reservations.Any(res =>
                res.VehicleID == vehicle.VehicleID &&
                !(res.ReturnDateTime <= reservationViewModel.PickupDateTime || res.PickupDateTime >= reservationViewModel.ReturnDateTime));

            if (conflict)
            {
                ModelState.AddModelError("", "Selected vehicle is not available in the chosen period.");
                return View(creditCardViewModel);
            }

            Reservation r = new Reservation
            {
                PickupDateTime = reservationViewModel.PickupDateTime,
                ReturnDateTime = reservationViewModel.ReturnDateTime,
                PickupPlace = reservationViewModel.PickupPlace,
                ReturnPlace = reservationViewModel.ReturnPlace,
                TotalPrice = (int)Math.Ceiling((reservationViewModel.ReturnDateTime - reservationViewModel.PickupDateTime).TotalDays) * vehicle.PricePerDay,
                CreditCardExpiryDate = expiryDate.ToString("MM'/'yy", CultureInfo.InvariantCulture),
                CreditCardLast4 = creditCardViewModel.CreditCardNumber.Length >= 4
                    ? creditCardViewModel.CreditCardNumber.Substring(creditCardViewModel.CreditCardNumber.Length - 4)
                    : creditCardViewModel.CreditCardNumber,
                CreditCardOwner = creditCardViewModel.CreditCardOwner,
                UserID = user.UserID,
                VehicleID = vehicle.VehicleID
            };

            db.Reservations.Add(r);
            db.SaveChanges();
            Session.Remove("rvm");
            Session.Remove("vehicle");

            return RedirectToAction("Final");
        }

        public ActionResult Final ()
        {
            return View();
        }
    }
}