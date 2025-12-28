using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Turbo_Wheels.Filters;
using Turbo_Wheels.Models;

namespace Turbo_Wheels.Controllers
{
    [RequireLogin]
    [RequireAdmin]
    public class ReservationsController : Controller
    {
        private Turbo_WheelsContext db = new Turbo_WheelsContext();

        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.User).Include(r => r.Vehicle);
            return View(reservations.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var reservation = db.Reservations
                                .Include(r => r.User)
                                .Include(r => r.Vehicle)
                                .SingleOrDefault(r => r.ReservationID == id);

            if (reservation == null)
            {
                return HttpNotFound();
            }

            return View(reservation);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var reservation = db.Reservations
                               .Include(r => r.User)
                               .Include(r => r.Vehicle)
                               .SingleOrDefault(r => r.ReservationID == id);

            if (reservation == null)
            {
                return HttpNotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Reservation reservation = db.Reservations.Find(id);

            if (reservation == null)
            {
                return HttpNotFound();
            }

            db.Reservations.Remove(reservation);
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
