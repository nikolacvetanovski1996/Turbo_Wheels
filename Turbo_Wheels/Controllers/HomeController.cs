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
                var isLoggedInCookie = Request.Cookies["HasLogged"];
                var userSession = Session["User"];

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
                Response.Cookies["HasLogged"].Value = false.ToString();
                Response.Cookies["NameSurname"].Value = "";
                return View();
            }

            [HttpPost]
            [AllowAnonymous]
            public ActionResult Login(LoggedInViewModel loggedInViewModel)
            {
                if (!ModelState.IsValid)
                    return View(loggedInViewModel);

                var user = db.Users.FirstOrDefault(u => u.Username == loggedInViewModel.Username);
                if (user == null || !Crypto.VerifyHashedPassword(user.Password, loggedInViewModel.Password))
                {
                    ModelState.AddModelError("", "Invalid username or password.");

                    // Clear password field
                    loggedInViewModel.Password = "";

                    return View(loggedInViewModel);
                }
           
                Response.Cookies["IsAdmin"].Value = user.IsAdmin.ToString();
                Response.Cookies["NameSurname"].Value = user.FirstName + " " + user.LastName;
                Session["User"] = user;
                Response.Cookies["HasLogged"].Value = true.ToString();

                return RedirectToAction("Index");
            }

            [AllowAnonymous]
            public ActionResult Logout()
            {
                // Clear session
                Session.Clear();
                Session.Abandon();

                // Clear cookies
                if (Request.Cookies["IsAdmin"] != null)
                {
                    var cookie = new HttpCookie("IsAdmin");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
                if (Request.Cookies["NameSurname"] != null)
                {
                    var cookie = new HttpCookie("NameSurname");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
                if (Request.Cookies["HasLogged"] != null)
                {
                    var cookie = new HttpCookie("HasLogged");
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
                if (Request.Cookies["IsAdmin"] == null)
                {
                    Response.Cookies["IsAdmin"].Value = false.ToString();
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
                user.FirstName = user.FirstName.Trim();
                user.LastName = user.LastName.Trim();
                user.Email = user.Email.Trim().ToLowerInvariant();

                if (!string.IsNullOrWhiteSpace(user.Address))
                    user.Address = user.Address.Trim();

                if (!string.IsNullOrWhiteSpace(user.Phone))
                    user.Phone = user.Phone.Trim();

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
                if (Session["User"] == null)
                    return RedirectToAction("Login");

                var reservation = Session["ReservationViewModel"] as ReservationViewModel;

                if (reservation == null)
                    return View(new ReservationInputViewModel
                    {
                        PickupDate = DateTime.Today,
                        ReturnDate = DateTime.Today.AddDays(1),
                        PickupTime = new TimeSpan(10, 0, 0),
                        ReturnTime = new TimeSpan(15, 0, 0)
                    });

                var model = new ReservationInputViewModel
                {
                    PickupPlace = reservation.PickupPlace,
                    ReturnPlace = reservation.ReturnPlace,
                    PickupDate = reservation.PickupDateTime.Date,
                    PickupTime = reservation.PickupDateTime.TimeOfDay,
                    ReturnDate = reservation.ReturnDateTime.Date,
                    ReturnTime = reservation.ReturnDateTime.TimeOfDay
                };

                return View(model);
            }

            [HttpPost]
            public ActionResult Index(ReservationInputViewModel reservationInputViewModel)
            {
                var pickupDateTime = reservationInputViewModel.PickupDate.Date + reservationInputViewModel.PickupTime;
                var returnDateTime = reservationInputViewModel.ReturnDate.Date + reservationInputViewModel.ReturnTime;

                if (pickupDateTime >= returnDateTime)
                {
                    ModelState.AddModelError("", "Return date/time must be after pickup date/time.");
                    return View(reservationInputViewModel); // Return the same view so user can fix
                }

                var reservationViewModel = new ReservationViewModel
                {
                    PickupPlace = reservationInputViewModel.PickupPlace,
                    ReturnPlace = reservationInputViewModel.ReturnPlace,
                    PickupDateTime = pickupDateTime,
                    ReturnDateTime = returnDateTime
                };
                Session["ReservationViewModel"] = reservationViewModel;
                return RedirectToAction("Step1");
            }

            public ActionResult About ()
            {
                return View();
            }

            public ActionResult Step1 ()
            {
                var reservationViewModel = (ReservationViewModel)Session["ReservationViewModel"];

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

                var vehicle = db.Vehicles.Find(id);
                if (vehicle == null)
                {
                    return HttpNotFound();
                }

                Session["Vehicle"] = vehicle;
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Step2 (CreditCardViewModel creditCardViewModel)
            {
                if (Session["ReservationViewModel"] == null || Session["Vehicle"] == null || Session["User"] == null)
                {
                    return RedirectToAction("Index"); // Session expired, restart booking
                }

                if (!ModelState.IsValid)
                {
                    return View(creditCardViewModel);
                }

                string normalizedCardNumber = null;

                if (creditCardViewModel.PayOnPickup == false)
                {
                    normalizedCardNumber =
                        creditCardViewModel.CreditCardNumber.Replace(" ", "");
                }

                DateTime? expiryDate = null;

                if (creditCardViewModel.PayOnPickup == false)
                {
                    try
                    {
                        int month = creditCardViewModel.ExpireMonth.Value;
                        int year = creditCardViewModel.ExpireYear.Value;

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

                        expiryDate = new DateTime(year, month,
                            DateTime.DaysInMonth(year, month), 23, 59, 59);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ModelState.AddModelError("", "Invalid expiration date.");
                        return View(creditCardViewModel);
                    }
                }
                
                if (creditCardViewModel.PayOnPickup == false &&
                    expiryDate <= DateTime.UtcNow)
                {
                    ModelState.AddModelError("", "Credit card expiration date must be in the future.");
                    return View(creditCardViewModel);
                }

                var reservationViewModel = (ReservationViewModel)Session["ReservationViewModel"];
                var vehicle = (Vehicle)Session["Vehicle"];
                var user = (User)Session["User"];

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
                var conflict = db.Reservations.Any(res =>
                    res.VehicleID == vehicle.VehicleID &&
                    !(res.ReturnDateTime <= reservationViewModel.PickupDateTime || res.PickupDateTime >= reservationViewModel.ReturnDateTime));

                if (conflict)
                {
                    ModelState.AddModelError("", "Selected vehicle is not available in the chosen period.");
                    return View(creditCardViewModel);
                }

                var reservation = new Reservation
                {
                    PickupDateTime = reservationViewModel.PickupDateTime,
                    ReturnDateTime = reservationViewModel.ReturnDateTime,
                    PickupPlace = reservationViewModel.PickupPlace,
                    ReturnPlace = reservationViewModel.ReturnPlace,
                    TotalPrice =
                        (int)Math.Ceiling(
                            (reservationViewModel.ReturnDateTime - reservationViewModel.PickupDateTime).TotalDays)
                        * vehicle.PricePerDay,
                    PaymentMethod = creditCardViewModel.PayOnPickup == true
                        ? PaymentMethod.PayOnPickup
                        : PaymentMethod.CreditCard,
                    UserID = user.UserID,
                    VehicleID = vehicle.VehicleID
                };

                if (creditCardViewModel.PayOnPickup == false)
                {
                    reservation.CreditCardExpiryDate = expiryDate.Value.ToString("MM'/'yy", CultureInfo.InvariantCulture);
                    reservation.CreditCardLast4 =
                        normalizedCardNumber.Length >= 4
                            ? normalizedCardNumber.Substring(normalizedCardNumber.Length - 4)
                            : normalizedCardNumber;
                    reservation.CreditCardOwner = creditCardViewModel.CreditCardOwner;
                }

                db.Reservations.Add(reservation);
                db.SaveChanges();
                Session.Remove("ReservationViewModel");
                Session.Remove("Vehicle");

                return RedirectToAction("Final");
            }

            public ActionResult Final ()
            {
                return View();
            }
        }
    }