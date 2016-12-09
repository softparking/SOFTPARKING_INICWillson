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

    /**
    * Metodo Para Eliminar Privilegio
    * @param codi_priv Numero Privilegio
    */
    public string EliminarPriv(int codi_priv)
    {
      privilegio_cliente dele_priv = new privilegio_cliente();
      dele_priv = db.privilegio_cliente.Find(codi_priv);
      if (dele_priv != null)
      {
        db.privilegio_cliente.Remove(dele_priv);
        db.SaveChanges();
        return "Mensaje,Eliminado Satisfactoriamente..!,Location,/Privilegio_cliente/Index";
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
