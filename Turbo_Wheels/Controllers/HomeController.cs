using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turbo_Wheels.Models;
using Turbo_Wheels.Models.ViewModels;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;

namespace Turbo_Wheels.Controllers
{
    public class HomeController : Controller
    {
        private Turbo_WheelsContext db = new Turbo_WheelsContext();
        public ActionResult Login()
        {
            Response.Cookies["hasLogged"].Value = false.ToString();
            Response.Cookies["namesurname"].Value = "";
            return View();
        }
        [HttpPost]
        public ActionResult Login(Logged_In loggedin)
        {
            User user = (from item in db.Users where item.username == loggedin.username &&
                         item.password == loggedin.password select item).FirstOrDefault();
            if (user == null)
            {
                return View();
            }
            else
            {
                Response.Cookies["isAdmin"].Value = user.isAdmin.ToString();
                Response.Cookies["namesurname"].Value = user.firstName + " " + user.lastName;
                Session["user"] = user;
                Response.Cookies["hasLogged"].Value = true.ToString();
                return RedirectToAction("Index");
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ReservationVM1 reservationvm1)
        {
            ReservationVM reservationvm = new ReservationVM();
            reservationvm.pickupPlace = reservationvm1.pickupPlace;
            reservationvm.returnPlace = reservationvm1.returnPlace;
            reservationvm.pickupDateTime = reservationvm1.pickupDate.Date + reservationvm1.pickupTime.TimeOfDay;
            reservationvm.returnDateTime = reservationvm1.returnDate.Date + reservationvm1.returnTime.TimeOfDay;
            Session["rvm"] = reservationvm;
            return RedirectToAction("Step1");
        }
        public ActionResult About ()
        {
            return View();
        }
        public ActionResult Step1 ()
        {
            ReservationVM rvm = (ReservationVM)Session["rvm"];
            List<Reservation> reservations = db.Reservations.ToList();
            List<Vehicle> vehicles = db.Vehicles.ToList();
            foreach (var reservation in reservations)
            {
                if (!((reservation.pickupDateTime > rvm.pickupDateTime &&
                    reservation.returnDateTime > rvm.pickupDateTime &&
                    reservation.pickupDateTime > rvm.returnDateTime &&
                    reservation.returnDateTime > rvm.returnDateTime) ||
                    (reservation.pickupDateTime < rvm.pickupDateTime &&
                    reservation.returnDateTime < rvm.pickupDateTime &&
                    reservation.pickupDateTime < rvm.returnDateTime &&
                    reservation.returnDateTime < rvm.returnDateTime)))
                {
                    if (vehicles.Contains(reservation.vehicle))
                    {
                        vehicles.Remove(reservation.vehicle);
                    }
                }
            }
            Session["expiredateIsValid"] = true;
            Session["monthInRange"] = true;
            return View(vehicles);
        }
        public ActionResult Step2 (int? id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            Session["vehicle"] = vehicle;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step2 (CreditCard c)
        {
            if (ModelState.IsValid)
            {
                if (c.month >= 1 && c.month <= 12)
                {
                    Session["monthInRange"] = true;
                    if (DateTime.Parse("01." + c.month + "." + c.year).AddMonths(1).AddSeconds(-1) > DateTime.Now)
                    {
                        Reservation r = new Reservation();
                        ReservationVM rvm = (ReservationVM)Session["rvm"];
                        Vehicle v = (Vehicle)Session["vehicle"];
                        User u = (User)Session["user"];
                        r.pickupDateTime = rvm.pickupDateTime;
                        r.returnDateTime = rvm.returnDateTime;
                        r.pickupPlace = rvm.pickupPlace;
                        r.returnPlace = rvm.returnPlace;
                        r.totalPrice = (int)Math.Ceiling((rvm.returnDateTime - rvm.pickupDateTime).TotalDays)* v.price_per_day;
                        r.CreditCardExpireDate = DateTime.Parse("01." + c.month + "." + c.year).AddMonths(1).AddSeconds(-1);
                        r.CreditCardNumber = c.CreditCardNumber;
                        r.CreditCardOwner = c.CreditCardOwner;
                        r.CVC2_CVV2_Number = c.CVC2_CVV2_Number;
                        r.userID = u.userID;
                        r.vehicleID = v.vehicleID;
                        if (ModelState.IsValid)
                        {
                            db.Reservations.Add(r);
                            db.SaveChanges();
                        }
                        return RedirectToAction("Final");
                    }
                    else
                    {
                        Session["expiredateIsValid"] = false;
                        return View();
                    }
                }
                else
                {
                    Session["monthInRange"] = false;
                    return View();
                }
            }
            return View();
        }
        public ActionResult Final ()
        {
            return View();
        }
    }
}