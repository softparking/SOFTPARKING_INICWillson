using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoftParking.Models;

namespace SoftParking.Controllers
{
    public class vehiculoController : Controller
    {
        private parqueaderoEntities db = new parqueaderoEntities();

        // GET: vehiculo
        public ActionResult Index()
        {
            var vehiculo = db.vehiculo.Include(v => v.cliente).Include(v => v.tipo_vehiculo);
            return View(vehiculo.ToList());
        }

        // GET: vehiculo/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehiculo vehiculo = db.vehiculo.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // GET: vehiculo/Create
        public ActionResult Create()
        {
            ViewBag.codi_clie = new SelectList(db.cliente, "codi_clie", "nomb_clie");
            ViewBag.tipo_vehi = new SelectList(db.tipo_vehiculo, "tipo_vehi", "nomb_vehi");
            return View();
        }

        // POST: vehiculo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "plac_vehi,colo_vehi,obse_vehi,marc_vehi,tipo_vehi,codi_clie")] vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                db.vehiculo.Add(vehiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.codi_clie = new SelectList(db.cliente, "codi_clie", "nomb_clie", vehiculo.codi_clie);
            ViewBag.tipo_vehi = new SelectList(db.tipo_vehiculo, "tipo_vehi", "nomb_vehi", vehiculo.tipo_vehi);
            return View(vehiculo);
        }

        // GET: vehiculo/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehiculo vehiculo = db.vehiculo.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            ViewBag.codi_clie = new SelectList(db.cliente, "codi_clie", "nomb_clie", vehiculo.codi_clie);
            ViewBag.tipo_vehi = new SelectList(db.tipo_vehiculo, "tipo_vehi", "nomb_vehi", vehiculo.tipo_vehi);
            return View(vehiculo);
        }

        // POST: vehiculo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "plac_vehi,colo_vehi,obse_vehi,marc_vehi,tipo_vehi,codi_clie")] vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.codi_clie = new SelectList(db.cliente, "codi_clie", "nomb_clie", vehiculo.codi_clie);
            ViewBag.tipo_vehi = new SelectList(db.tipo_vehiculo, "tipo_vehi", "nomb_vehi", vehiculo.tipo_vehi);
            return View(vehiculo);
        }

        // GET: vehiculo/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vehiculo vehiculo = db.vehiculo.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // POST: vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            vehiculo vehiculo = db.vehiculo.Find(id);
            db.vehiculo.Remove(vehiculo);
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
