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
    public class ubicacionController : Controller
    {
        private parqueaderoEntities db = new parqueaderoEntities();

        // GET: ubicacion
        public ActionResult Index()
        {
            return View(db.ubicacion.ToList());
        }

        // GET: ubicacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ubicacion ubicacion = db.ubicacion.Find(id);
            if (ubicacion == null)
            {
                return HttpNotFound();
            }
            return View(ubicacion);
        }

        // GET: ubicacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ubicacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codi_ubic,tama_ubic,esta_ubic,piso_ubic,dist_ubic")] ubicacion ubicacionRow, int posicion)
        {
            int contador =0;
            int numero = posicion;
            int[] dato = new int[posicion];
            

            if (ModelState.IsValid)
            {  if (posicion == 1)
                {
                    db.ubicacion.Add(ubicacionRow);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (numero != 1) {



                    ubicacion ubic;

                        foreach (int element in dato)
                    {
                       
                        contador = contador + 1;
                        ubic = new ubicacion();
                        ubic.codi_ubic = ubicacionRow.codi_ubic;
                        ubic.tama_ubic = ubicacionRow.tama_ubic;
                        ubic.esta_ubic = ubicacionRow.esta_ubic;
                        ubic.piso_ubic = ubicacionRow.piso_ubic;
                        ubic.dist_ubic = ubicacionRow.dist_ubic;
                        db.ubicacion.Add(ubic);

                        ViewBag.PROGRESO = contador;
                        ViewBag.MAXIMO =numero;
                    }
                    db.SaveChanges();
                   
                   



                    if (contador == numero) {
                        //db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
               

            }

            return View(ubicacionRow);
        }

        // GET: ubicacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ubicacion ubicacion = db.ubicacion.Find(id);
            if (ubicacion == null)
            {
                return HttpNotFound();
            }
            return View(ubicacion);
        }

        // POST: ubicacionPRUEBA/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codi_ubic,tama_ubic,esta_ubic,piso_ubic,dist_ubic")] ubicacion ubicacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ubicacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ubicacion);
        }

        // GET: ubicacionPRUEBA/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ubicacion ubicacion = db.ubicacion.Find(id);
            if (ubicacion == null)
            {
                return HttpNotFound();
            }
            return View(ubicacion);
        }

        // POST: ubicacionPRUEBA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ubicacion ubicacion = db.ubicacion.Find(id);
            db.ubicacion.Remove(ubicacion);
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
