﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Turbo_Wheels.Models;

namespace Turbo_Wheels.Controllers
{
    public class VehiclesController : Controller
    {
        private Turbo_WheelsContext db = new Turbo_WheelsContext();

        // GET: Vehicles
        public ActionResult Index()
        {
            return View(db.Vehicles.ToList());
        }

        // GET: Vehicles/Details/5
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

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "vehicleID,type,brand,model,fuel,transmission," +
            "price_per_day,doors,seats,has_airbag,has_ABS,has_ESP,has_GPS,imageURL,ImageFile")] Vehicle vehicle)
        {
            string url = vehicle.brand + "_" + vehicle.model + Path.GetExtension(vehicle.ImageFile.FileName);
            vehicle.imageURL = url;
            string filename = Path.Combine(Server.MapPath("~/images/vehicle_images/"), url);
            vehicle.ImageFile.SaveAs(filename);
            db.Vehicles.Add(vehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Vehicles/Delete/5
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

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            string filePath = Path.Combine(Request.MapPath("~/images/vehicle_images/"), vehicle.imageURL);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            db.Vehicles.Remove(vehicle);
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
