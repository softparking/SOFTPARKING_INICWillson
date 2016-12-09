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
    public class privilegio_clienteController : Controller
    {
        private parqueaderoEntities db = new parqueaderoEntities();

        // GET: privilegio_cliente
        public ActionResult Index()
        {
            return View(db.privilegio_cliente.ToList());
        }
        
        // GET: privilegio_cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: privilegio_cliente/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codi_priv,tipo_priv,tari_priv,esta_priv,fech_priv")] privilegio_cliente privilegio_cliente)
        {
            if (ModelState.IsValid)
            {
                db.privilegio_cliente.Add(privilegio_cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(privilegio_cliente);
        }

        // GET: privilegio_cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            privilegio_cliente privilegio_cliente = db.privilegio_cliente.Find(id);
            if (privilegio_cliente == null)
            {
                return HttpNotFound();
            }
            return View(privilegio_cliente);
        }

        // POST: privilegio_cliente/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codi_priv,tipo_priv,tari_priv,esta_priv,fech_priv")] privilegio_cliente privilegio_cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(privilegio_cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(privilegio_cliente);
        }

        // GET: privilegio_cliente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            privilegio_cliente privilegio_cliente = db.privilegio_cliente.Find(id);
            if (privilegio_cliente == null)
            {
                return HttpNotFound();
            }
            return View(privilegio_cliente);
        }

        // POST: privilegio_cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            privilegio_cliente privilegio_cliente = db.privilegio_cliente.Find(id);
            db.privilegio_cliente.Remove(privilegio_cliente);
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
