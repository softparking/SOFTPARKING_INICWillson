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

    // GET: lista_negra/Create
    public ActionResult Create()
    {
      ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "plac_vehi");
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "esta_list,plac_vehi,obse_list")] lista_negra lista_negra)
    {
      if (ModelState.IsValid)
      {
        db.Database.BeginTransaction();
        var inse_list = "INSERT INTO lista_negra (esta_list, plac_vehi, obse_list) VALUES ('"+lista_negra.esta_list+"','"+lista_negra.plac_vehi+"','"+lista_negra.obse_list+"')";
        db.Database.ExecuteSqlCommand(inse_list);
        db.Database.CurrentTransaction.Commit();        
        return RedirectToAction("Index");
      }
      return RedirectToAction("Index");
    }

    /**
    * Metodo Para Cambiar Estado Lista
    * @param Plac_vehi Placa Vehiculo
    */
    public string EstadoLista(string Plac_vehi)
    {

      var esta_vehi = from ln in db.lista_negra
                      where ln.vehiculo.plac_vehi == Plac_vehi
                      select ln.esta_list;
      try
      {
        string nuev_esta;
        var estado = esta_vehi.First();
        if (estado == "ACT")
        {
          nuev_esta = "INA";
        }
        else if (estado == "INA")
        {
          nuev_esta = "ACT";
        }
        else
        {
          return "Error,Estado No Valido Por Favor Verifique..!!";
        }

        /*Modificando Registro*/
        var modi_esta = "UPDATE lista_negra SET esta_list = '" + nuev_esta + "' WHERE plac_vehi = '" + Plac_vehi + "'";
        db.Database.BeginTransaction();
        db.Database.ExecuteSqlCommand(modi_esta);
        db.Database.CurrentTransaction.Commit();
        return "Mensaje,Estado Actualizado Satisfactoriamente..!!,Location,/Lista_Negra";
      }
      catch
      {
        return "Error,Placa No Encontrada Intente Nuevamente..!!";
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
