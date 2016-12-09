using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoftParking.Models;
using System.Dynamic;

namespace SoftParking.Controllers
{
    public class turnoController : Controller
    {
        private parqueaderoEntities db = new parqueaderoEntities();


        // GET: turno
        public ActionResult Index()
        {
            //incluye  las referencias empleado y caja ala tabla turno  
            var turno=db.turno.Include(s =>s.empleado).Include(s =>s.caja);
            return View(db.turno.ToList());
        }



        // GET: turno/Create
        [HttpGet]
        public ActionResult Create()
        { /// para un dropdown
            var MisCajas = db.caja.ToList();
            SelectList listaCaja = new SelectList(MisCajas, "codi_caja", "nomb_caja", "Estado");
             ViewBag.VistasCaja = listaCaja;
             ViewBag.ListaCaja = db.caja.ToList();

           // ViewBag.ListaMic = listaCaja;
            /// para  enviar una lista  a la vista desde la  base de datos
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codi_turn,codi_empl,codi_caja,fech_inic,fech_fina,mont_inic,desc_turn")] turno turno)
        {
            if (ModelState.IsValid)
            {
                db.turno.Add(turno);
                db.SaveChanges();
                //se creo una variable bandera para que cuando se realice el procedimiento sepa que se efectuo correctamente
                string var = "si";
                TempData["BanderaTurno"] = var;
                return RedirectToAction("Index","servicios");

            }

            ViewBag.codi_caja = new SelectList(db.caja, "codi_caja", "nomb_caja", turno.codi_caja);
            ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", turno.codi_empl); 
            return View(turno);
        }




        // POST: turno/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codi_turn,codi_empl,codi_caja,fech_inic,fech_fina,mont_inic,desc_turn")] turno turno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.codi_caja = new SelectList(db.caja, "codi_caja", "nomb_caja", turno.codi_caja);
            ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", turno.codi_empl);
            return View(turno);
        }

        // GET: turno/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            turno turno = db.turno.Find(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            return View(turno);
        }

        

        // GET: turno/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            turno turno = db.turno.Find(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", turno.empleado.codi_empl);
            ViewBag.codi_caja = new SelectList(db.caja, "codi_caja", "nomb_caja", turno.caja.codi_caja);
            return View(turno);
        }

        // POST: turno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            turno turno = db.turno.Find(id);
            db.turno.Remove(turno);
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
