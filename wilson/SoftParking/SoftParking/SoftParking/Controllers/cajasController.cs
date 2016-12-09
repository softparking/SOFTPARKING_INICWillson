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
  public class cajasController : Controller
  {
    private parqueaderoEntities db = new parqueaderoEntities();

    // GET: cajas
    public ActionResult Index()
    {
      return View(db.caja.ToList());
    }
    
    // GET: cajas/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: cajas/Create.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "codi_caja,nomb_caja,esta_caja,obse_caja")] caja caja)
    {
      if (ModelState.IsValid)
      {
        db.caja.Add(caja);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(caja);
    }

    // GET: cajas/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      caja caja = db.caja.Find(id);
      if (caja == null)
      {
        return HttpNotFound();
      }
      return View(caja);
    }

    // POST: cajas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "codi_caja,nomb_caja,esta_caja,obse_caja")] caja caja)
    {
      if (ModelState.IsValid)
      {
        db.Entry(caja).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(caja);
    }

    /**
    * Metodo Para Eliminar Caja
    * @param codi_caja Numero Caja
    */
    public string EliminarCaja(int codi_caja)
    {
      caja dele_caja = new caja();
      dele_caja = db.caja.Find(codi_caja);
      if (dele_caja != null)
      {
        db.caja.Remove(dele_caja);
        db.SaveChanges();
        return "Mensaje,Eliminado Satisfactoriamente..!,Location,/Cajas/Index";
      }
      else
      {
        return "Error,No Se Encontro Informacion Alguna Por Favor Verifique..!!";
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
