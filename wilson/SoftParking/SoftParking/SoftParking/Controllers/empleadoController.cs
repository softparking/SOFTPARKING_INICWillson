using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoftParking.Models;

namespace SoftParking.Controllers
{
  public class empleadoController : Controller
  {
    private parqueaderoEntities db = new parqueaderoEntities();

    // GET: empleado
    public async Task<ActionResult> Index()
    {
      return View(await db.empleado.ToListAsync());
    }

    // GET: empleado/Create
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "codi_empl,iden_empl,nomb_empl,apel_empl,dire_empl,usua_empl,cont_empl,carg_empl")] empleado empleado)
    {
      if (ModelState.IsValid)
      {
        db.empleado.Add(empleado);
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      return View(empleado);
    }

    // GET: empleado/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      empleado empleado = await db.empleado.FindAsync(id);
      if (empleado == null)
      {
        return HttpNotFound();
      }
      return View(empleado);
    }

    // POST: empleado/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "codi_empl,iden_empl,nomb_empl,apel_empl,dire_empl,usua_empl,cont_empl,carg_empl")] empleado empleado)
    {
      if (ModelState.IsValid)
      {
        db.Entry(empleado).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      return View(empleado);
    }

    /**
    * Metodo Para Eliminar Empleado
    * @param codi_empl Codigo Empleado
    */
    public string EliminarEmpleado(int codi_empl)
    {
      empleado dele_empl = new empleado();
      dele_empl = db.empleado.Find(codi_empl);
      if(dele_empl != null)
      {
        db.empleado.Remove(dele_empl);
        db.SaveChanges();
        return "Mensaje,Eliminado Satisfactoriamente..!,Location,/Empleado/Index";
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
