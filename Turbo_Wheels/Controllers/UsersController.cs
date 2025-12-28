using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using Turbo_Wheels.Filters;
using Turbo_Wheels.Models;

namespace Turbo_Wheels.Controllers
{
    [RequireLogin]
    [RequireAdmin]
    public class UsersController : Controller
    {
        private Turbo_WheelsContext db = new Turbo_WheelsContext();

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            bool userExists = db.Users.Any(u => u.Username == user.Username);

            if (userExists)
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(user);
            }

            // Trim and normalize username and email
            user.Username = user.Username.Trim();
            user.Email = user.Email.Trim().ToLowerInvariant();

            // Hash the password before saving
            user.Password = Crypto.HashPassword(user.Password);

            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var existingUser = db.Users.Find(user.UserID);
            if (existingUser == null)
                return HttpNotFound();

            // Update fields (explicitly)
            existingUser.Username = user.Username;
            existingUser.IsAdmin = user.IsAdmin;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email.Trim().ToLowerInvariant();
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;

            // Only update password if a new one was provided
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existingUser.Password = Crypto.HashPassword(user.Password);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            User user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
