using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Turbo_Wheels.Filters;
using Turbo_Wheels.Models;

namespace Turbo_Wheels.Controllers
{
    [RequireLogin]
    [RequireAdmin]
    public class VehiclesController : Controller
    {
        private Turbo_WheelsContext db = new Turbo_WheelsContext();
        private string ImageFolder => Server.MapPath("~/images/vehicle_images/");


        public ActionResult Index()
        {
            return View(db.Vehicles.ToList());
        }

        public ActionResult Details(int? id)
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
            return View(vehicle);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vehicle vehicle)
        {
            ModelState.Remove("ImageURL");

            if (vehicle.ImageFile == null || vehicle.ImageFile.ContentLength == 0)
            {
                ModelState.AddModelError("ImageFile", "Vehicle image is required.");
            }

            if (ModelState.IsValid)
            {
                var ext = Path.GetExtension(vehicle.ImageFile.FileName);
                var url = $"{Guid.NewGuid():N}_{vehicle.Brand}_{vehicle.Model}{ext}";
                vehicle.ImageURL = url;

                SaveImage(vehicle.ImageFile, url);

                db.Vehicles.Add(vehicle);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        public ActionResult Edit(int id)
        {
            var vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
                return HttpNotFound();

            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vehicle vehicle)
        {
            ModelState.Remove("ImageURL");
            ModelState.Remove("ImageFile");

            if (ModelState.IsValid)
            {
                var existingVehicle = db.Vehicles.Find(vehicle.VehicleID);
                if (existingVehicle == null)
                    return HttpNotFound();

                // Update all editable fields except ImageURL first
                existingVehicle.Type = vehicle.Type;
                existingVehicle.Brand = vehicle.Brand;
                existingVehicle.Model = vehicle.Model;
                existingVehicle.Fuel = vehicle.Fuel;
                existingVehicle.Transmission = vehicle.Transmission;
                existingVehicle.PricePerDay = vehicle.PricePerDay;
                existingVehicle.Doors = vehicle.Doors;
                existingVehicle.Seats = vehicle.Seats;
                existingVehicle.HasAirbag = vehicle.HasAirbag;
                existingVehicle.HasAbs = vehicle.HasAbs;
                existingVehicle.HasEsp = vehicle.HasEsp;
                existingVehicle.HasGps = vehicle.HasGps;

                // Handle image file upload if provided
                if (vehicle.ImageFile != null && vehicle.ImageFile.ContentLength > 0)
                {
                     DeleteImage(existingVehicle.ImageURL);

                    // Save new file with unique filename
                    var ext = Path.GetExtension(vehicle.ImageFile.FileName);
                    var url = $"{Guid.NewGuid():N}_{vehicle.Brand}_{vehicle.Model}{ext}";
                    existingVehicle.ImageURL = url;

                    SaveImage(vehicle.ImageFile, url);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        public ActionResult Delete(int? id)
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

            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);

            if (vehicle == null)
            {
                return HttpNotFound();
            }

            DeleteImage(vehicle.ImageURL);

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private void SaveImage(System.Web.HttpPostedFileBase file, string fileName)
        {
            if (file == null || string.IsNullOrEmpty(fileName))
                return;

            var path = Path.Combine(ImageFolder, fileName);
            file.SaveAs(path);
        }

        private void DeleteImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var path = Path.Combine(ImageFolder, fileName);
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch { }
            }

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
