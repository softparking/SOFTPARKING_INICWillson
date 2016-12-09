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
    public class serviciosController : Controller
    {
        private parqueaderoEntities db = new parqueaderoEntities();

        // GET: servicios
        public ActionResult Index()
        {
            var servicio = db.servicio.Include(s => s.empleado).Include(s => s.vehiculo).Include(s => s.ubicacion);
            return View(servicio.ToList());
        }

        // GET: servicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            servicio servicio = db.servicio.Find(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        // GET: servicios/Create
        public ActionResult Create()
        {
            ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl");
            ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi");
            ViewBag.codi_ubic = new SelectList(db.ubicacion, "codi_ubic", "tama_ubic");
            ViewBag.tipo_vehi = new SelectList(db.tipo_vehiculo, "tipo_vehi", "nomb_vehi");

            //SENTENCIAS QUERYS PARA REALIZAR LA CONSULTA 
            var consulta = "select count(*) from ubicacion";
            var disp = "select count(*) from ubicacion where  esta_ubic='DISPONIBLE'";
            var ocup = "select count(*) from ubicacion where  esta_ubic='OCUPADO'";
            var valor = db.Database.SqlQuery<string>(consulta).FirstOrDefault();
            ViewBag.vahias = valor;
            ViewBag.disponible = db.Database.SqlQuery<string>(disp).FirstOrDefault();
            ViewBag.ocupado = db.Database.SqlQuery<string>(ocup).FirstOrDefault();


            return View();
        }

        // POST: servicios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codi_serv,fech_serv,hora_entr,hora_sali,codi_empl,plac_vehi,codi_ubic")]servicio servicio, string plac_vehi, int tipo_vehi)
        {

            try
            {
                vehiculo vehi = new vehiculo();
                vehi.plac_vehi = plac_vehi;
                vehi.tipo_vehi = tipo_vehi;
                db.vehiculo.Add(vehi);
                db.SaveChanges();

            }
            catch (System.Data.SqlClient.SqlException ex)
            {


            }

            if (ModelState.IsValid)
            {
                db.servicio.Add(servicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", servicio.codi_empl);
            ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", servicio.plac_vehi);
            ViewBag.codi_ubic = new SelectList(db.ubicacion, "codi_ubic", "tama_ubic", servicio.codi_ubic);
            return View(servicio);

        }

        //GENERAR SALIDA DEL VEHICULO 
        [HttpPost]
        public ActionResult Salida()
        {


            return View();
        }



        // GET: servicios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            servicio servicio = db.servicio.Find(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", servicio.codi_empl);
            ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", servicio.plac_vehi);
            ViewBag.codi_ubic = new SelectList(db.ubicacion, "codi_ubic", "tama_ubic", servicio.codi_ubic);
            return View(servicio);
        }

        // POST: servicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codi_serv,fech_serv,hora_entr,hora_sali,codi_empl,plac_vehi,codi_ubic")] servicio servicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", servicio.codi_empl);
            ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", servicio.plac_vehi);
            ViewBag.codi_ubic = new SelectList(db.ubicacion, "codi_ubic", "tama_ubic", servicio.codi_ubic);
            return View(servicio);
        }

        // GET: servicios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            servicio servicio = db.servicio.Find(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        // POST: servicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            servicio servicio = db.servicio.Find(id);
            db.servicio.Remove(servicio);
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
