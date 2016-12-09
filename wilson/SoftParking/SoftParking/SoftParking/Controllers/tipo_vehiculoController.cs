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
  public class tipo_vehiculoController : Controller
  {
    private parqueaderoEntities db = new parqueaderoEntities();

    // GET: tipo_vehiculo
    public ActionResult Index()
    {      
      return View(db.tipo_vehiculo.ToList());
    }

    // GET: tipo_vehiculo/Create
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "tipo_vehi,nomb_vehi,tari_vehi")] tipo_vehiculo tipo_vehiculo)
    {
      if (ModelState.IsValid)
      {
        db.tipo_vehiculo.Add(tipo_vehiculo);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(tipo_vehiculo);
    }

    // GET: tipo_vehiculo/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      tipo_vehiculo tipo_vehiculo = db.tipo_vehiculo.Find(id);
      if (tipo_vehiculo == null)
      {
        return HttpNotFound();
      }
      return View(tipo_vehiculo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "tipo_vehi,nomb_vehi,tari_vehi")] tipo_vehiculo tipo_vehiculo)
    {
      if (ModelState.IsValid)
      {
        db.Entry(tipo_vehiculo).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(tipo_vehiculo);
    }

    /**
    * Metodo Para Realizar Eliminacion Tipo Vehiculo
    * @param tipo_vehi Codigo Tipo Vehiculo A Eliminar
    */
    public string EliminarTipoV(int tipo_vehi)
    {
      tipo_vehiculo dele_tipo = new tipo_vehiculo();
      dele_tipo = db.tipo_vehiculo.Find(tipo_vehi);
      if(dele_tipo != null)
      {
        db.tipo_vehiculo.Remove(dele_tipo);
        db.SaveChanges();
        return "Mensaje,Eliminado Satisfactoriamente..!,Location,/Tipo_vehiculo/Index";
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
