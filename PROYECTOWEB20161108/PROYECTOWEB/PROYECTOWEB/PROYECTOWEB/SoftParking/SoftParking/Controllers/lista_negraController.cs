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
    public class lista_negraController : Controller
    {
        private parqueaderoEntities db = new parqueaderoEntities();

        // GET: lista_negra
        public ActionResult Index()
        {
            var lista_negra = db.lista_negra.Include(l => l.vehiculo);
            return View(lista_negra.ToList());
        }

        // GET: lista_negra/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lista_negra lista_negra = db.lista_negra.Find(id);
            if (lista_negra == null)
            {
                return HttpNotFound();
            }
            return View(lista_negra);
        }

        // GET: lista_negra/Create
        public ActionResult Create()
        {
            ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi");
            return View();
        }

        // POST: lista_negra/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "esta_list,obse_list,plac_vehi")] lista_negra lista_negra)
        {
            if (ModelState.IsValid)
            {
                db.lista_negra.Add(lista_negra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", lista_negra.plac_vehi);
            return View(lista_negra);
        }

        // GET: lista_negra/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lista_negra lista_negra = db.lista_negra.Find(id);
            if (lista_negra == null)
            {
                return HttpNotFound();
            }
            ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", lista_negra.plac_vehi);
            return View(lista_negra);
        }

        // POST: lista_negra/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "esta_list,obse_list,plac_vehi")] lista_negra lista_negra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lista_negra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", lista_negra.plac_vehi);
            return View(lista_negra);
        }

        // GET: lista_negra/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lista_negra lista_negra = db.lista_negra.Find(id);
            if (lista_negra == null)
            {
                return HttpNotFound();
            }
            return View(lista_negra);
        }

        // POST: lista_negra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            lista_negra lista_negra = db.lista_negra.Find(id);
            db.lista_negra.Remove(lista_negra);
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
