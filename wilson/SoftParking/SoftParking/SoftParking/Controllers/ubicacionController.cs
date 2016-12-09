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

    [HttpPost]
    [ValidateAntiForgeryToken]
    //public ActionResult Create([Bind(Include = "codi_ubic,tama_ubic,esta_ubic,piso_ubic,dist_ubic")] ubicacion ubicacion)
    public ActionResult Create([Bind(Include = "codi_ubic,tama_ubic,esta_ubic,piso_ubic,dist_ubic")] ubicacion ubicacionRow, int posicion)
    {
      //if (ModelState.IsValid)
      //{
      //  db.ubicacion.Add(ubicacion);
      //  db.SaveChanges();
      //  return RedirectToAction("Index");
      //}
      //return View(ubicacion);

      int contador = 0;
      int numero = posicion;
      int[] dato = new int[posicion];
      if (ModelState.IsValid)
      {
        if (posicion == 1)
        {
          db.ubicacion.Add(ubicacionRow);
          db.SaveChanges();
          return RedirectToAction("Index");
        }
        else if (numero != 1)
        {
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
            ViewBag.MAXIMO = numero;
          }
          db.SaveChanges();
          if (contador == numero)
          {
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "codi_ubic,tama_ubic,esta_ubic,piso_ubic,dist_ubic")] ubicacion ubicacion)
    {
      if (ModelState.IsValid)
      {
        db.Database.BeginTransaction();
        var modi_ubic = "UPDATE ubicacion SET tama_ubic ='"+ubicacion.tama_ubic+"', esta_ubic='"+ubicacion.esta_ubic+"', piso_ubic='"+ubicacion.piso_ubic+"', dist_ubic='"+ubicacion.dist_ubic+"' WHERE codi_ubic='"+ubicacion.codi_ubic+"'";
        db.Database.ExecuteSqlCommand(modi_ubic);
        db.Database.CurrentTransaction.Commit();                
        return RedirectToAction("Index");
      }
      return View(ubicacion);
    }

    /**
    * Metodo Para Eliminar Ubicacion 
    * @param codi_ubic Codigo Ubicacion Relacionada
    */
    public string EliminarUbicacion(int codi_ubic)
    {
      ubicacion dele_ubic = new ubicacion();
      dele_ubic = db.ubicacion.Find(codi_ubic);
      db.ubicacion.Remove(dele_ubic);
      db.SaveChanges();
      return "Mensaje,Ubicacion Eliminada Satisfactoriamente..!!,Location,/Ubicacion";
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
