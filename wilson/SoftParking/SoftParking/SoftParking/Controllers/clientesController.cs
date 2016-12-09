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
  public class clientesController : Controller
  {
    private parqueaderoEntities db = new parqueaderoEntities();

    // GET: clientes
    public ActionResult Index()
    {
      var cliente = db.cliente.Include(c => c.privilegio_cliente);
      return View(cliente.ToList());
    }

    // GET: clientes/Create
    public ActionResult Create()
    {
      ViewBag.codi_priv = new SelectList(db.privilegio_cliente, "codi_priv", "tipo_priv");
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "codi_clie,iden_clie,nomb_clie,apel_clie,codi_priv")] cliente cliente)
    {
      if (ModelState.IsValid)
      {
        db.cliente.Add(cliente);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      ViewBag.codi_priv = new SelectList(db.privilegio_cliente, "codi_priv", "tipo_priv", cliente.codi_priv);
      return View(cliente);
    }

    // GET: clientes/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      cliente cliente = db.cliente.Find(id);
      if (cliente == null)
      {
        return HttpNotFound();
      }
      ViewBag.codi_priv = new SelectList(db.privilegio_cliente, "codi_priv", "tipo_priv", cliente.codi_priv);
      return View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "codi_clie,iden_clie,nomb_clie,apel_clie,codi_priv")] cliente cliente)
    {
      if (ModelState.IsValid)
      {
        db.Entry(cliente).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      ViewBag.codi_priv = new SelectList(db.privilegio_cliente, "codi_priv", "tipo_priv", cliente.codi_priv);
      return View(cliente);
    }

    /**
    * Metodo Para Eliminar Cliente
    * @param codi_clie Codigo Cliente
    */
    public string EliminarCliente(int codi_clie)
    {
      try
      {
        db.Database.BeginTransaction();
        /*Eliminando Vehiculo Asociado*/
        var elim_vehi = "DELETE FROM vehiculo WHERE codi_clie = '" + codi_clie + "'";
        db.Database.ExecuteSqlCommand(elim_vehi);

        cliente dele_clie = new cliente();
        dele_clie = db.cliente.Find(codi_clie);
        db.cliente.Remove(dele_clie);
        db.SaveChanges();
        db.Database.CurrentTransaction.Commit();
        return "Mensaje,Eliminado Correctamente..!!,Location,/Clientes";
      }
      catch (Exception e)
      {
        return "Error,Ocurrio un Error Eliminando El Registros Intente Mas Tarde.!! (" + e + ")";
      }
    }

    /**
    * Metodo Para Mostrar Vehiculos Sin Cliente Registrado
    * @param codi_clie Codigo Cliente
    */
    public string MostrarVehiculos(int iden_clie)
    {
      string html = "";
      var cons_vehi = from vehi in db.vehiculo
                      where vehi.cliente.iden_clie.Equals(iden_clie)
                      select new { vehi.plac_vehi, vehi.marc_vehi, vehi.tipo_vehiculo.nomb_vehi, vehi.colo_vehi };
      try
      {
        cons_vehi.First();
        html += "<div class = 'container-fluid'>";
        html += " <div class='panel panel-info'>";
        html += "   <div class='panel-heading text-center'>";
        if(iden_clie == 99)
        {
          html += "     <h4>Vehiculos Disponibles</h4>";
        }
        else
        {
          html += "     <h4>Vehiculos Asignados</h4>";
        }        
        html += "   </div>";
        html += "   <div class='panel-body'>";
        html += "     <table class='table  table-responsive table-bordered table-striped table-hover'>";
        html += "       <tr>";
        html += "         <th>PLACA</th>";
        html += "         <th>MARCA</th>";
        html += "         <th>TIPO</th>";
        html += "         <th>COLOR</th>";
        if(iden_clie == 99)
        {
          html += "         <th>ASIGNACIÓN</th>";
        }        
        html += "       </tr>";
        foreach (var item in cons_vehi)
        {
          html += "     <tr>";
          html += "       <td>" + item.plac_vehi + "</td>";
          html += "       <td>" + item.marc_vehi + "</td>";
          html += "       <td>" + item.nomb_vehi + "</td>";
          html += "       <td>" + item.colo_vehi + "</td>";
          if (iden_clie == 99)
          {
            html += "       <td align='center'>";
            html += "         <button Onclick=\"AsignaVehiculo('" + item.plac_vehi + "')\" class='btn btn-success'>";
            html += "          <span class='glyphicon glyphicon-floppy-save'></span>";
            html += "          </button>";
            html += "       </td>";
          }
          html += "     </tr>";
        }
        html += "     </table>";
        html += "   </div>";
        html += " </div>";
        html += "</div>";
        return "Mensaje," + html;
      }
      catch
      {
        return "Error,No Se Encuentran Vehiculos Registrados Por Favor Verifique..!!";
      }
    }

    /**
    * Metodo Para Guardar Cliente Asignando Vehiculo
    * @param proc Procedimiento A Realizar
    * @param iden_clie Identificacion
    * @param nomb_clie Nombre
    * @param apel_clie Apellido
    * @param codi_priv Privilegio Cliente
    * @param plac_vehi Placa Vehiculo Asignar
    * @param colo_vehi Color 
    * @param obse_vehi Observacion
    * @param marc_vehi Marca Vehiculo
    * @param tipo_vehi Tipo Vehiculo
    * @param codi_clie Codigo Cliente
    */
    public string GuardarCliente(int proc, int iden_clie, string nomb_clie, string apel_clie, int codi_priv, string plac_vehi, string colo_vehi, string obse_vehi, string marc_vehi, int tipo_vehi, int codi_clie)
    {
      db.Database.BeginTransaction();
      /*Inserta Cliente*/
      cliente nuev_clie = new cliente();
      nuev_clie.iden_clie = iden_clie;
      nuev_clie.nomb_clie = nomb_clie;
      nuev_clie.apel_clie = apel_clie;
      nuev_clie.codi_priv = codi_priv;
      db.cliente.Add(nuev_clie);

      /*Actualizacion Vehiculo*/
      if (proc == 1)
      {
        db.SaveChanges();
        var asig_vehi = "UPDATE vehiculo SET codi_clie = (SELECT MAX(codi_clie) FROM cliente) WHERE plac_vehi = '" + plac_vehi + "'";
        db.Database.ExecuteSqlCommand(asig_vehi);
        db.Database.CurrentTransaction.Commit();
        return "Mensaje,Cliente Guardado Satisfactoriamente..!!,Location,/Clientes/Index";
      }
      else
      {
        /*Insertando Vehiculo*/
        var resp_inse = InsertarAsignarVehiculo(plac_vehi, colo_vehi, obse_vehi, marc_vehi, tipo_vehi, codi_clie);
        if (resp_inse == "KO")
        {
          db.Database.CurrentTransaction.Commit();
          return "Mensaje,Cliente Guardado y Vehiculo Creado Satisfactoriamente..!!,Location,/Clientes/Index";
        }
        else
        {
          return "Error,Ocurrio Un Error En La Creacion Del Cliente Intente De Nuevo O Mas Tarde..!!";
        }
      }
    }

    /**
    * Metodo Para Insertar Y Asignar Nuevo Vehiculo
    * @param plac_vehi Placa Nuevo Vehiculo
    * @param colo_vehi Color 
    * @param obse_vehi Observacion
    * @param marc_vehi Marca Vehiculo
    * @param tipo_vehi Tipo Vehiculo
    * @param codi_clie Codigo Cliente
    */
    public string InsertarAsignarVehiculo(string plac_vehi, string colo_vehi, string obse_vehi, string marc_vehi, int tipo_vehi, int codi_clie)
    {
      vehiculo nuev_vehi = new vehiculo();
      nuev_vehi.plac_vehi = plac_vehi;
      nuev_vehi.colo_vehi = colo_vehi;
      nuev_vehi.obse_vehi = obse_vehi;
      nuev_vehi.marc_vehi = marc_vehi;
      nuev_vehi.tipo_vehi = tipo_vehi;
      nuev_vehi.codi_clie = codi_clie;
      db.vehiculo.Add(nuev_vehi);
      db.SaveChanges();
      return "KO";
    }

    /**
    * Metodo Para Ingresar Informacion Vehiculo
    */
    public string IngresarVehiculo()
    {
      string html = "";
      html += "<div class='container-fluid'>";
      html += "  <div class='panel panel-info'>";
      html += "    <div class='panel-heading text-center'>";
      html += "      <h4>Registrar Vehiculo</h4>";
      html += "    </div>";
      html += "    <div class='panel-body' align='center'>";
      html += "      <div class='form-inline'>";
      html += "        <label>PLACA</label>";
      html += "        <input class='form-control' id='plac_vehi' placeholder='Placa Vehiculo' type='text'>";
      html += "      </div>";
      html += "      <br>";
      html += "      <div class='form-inline'>";
      html += "        <label>COLOR</label>";
      html += "        <input class='form-control' id='colo_vehi' placeholder='Color Vehiculo' type='text'>";
      html += "      </div>";
      html += "      <br>";
      html += "      <div class='form-inline'>";
      html += "        <label>MARCA</label>";
      html += "        <input class='form-control' id='marc_vehi' placeholder='Marca Vehiculo' type='text'>";
      html += "      </div>";
      html += "      <br>";
      html += "      <div class='form-inline'>";
      html += "        <label>TIPO</label>";
      html += "        <select class='form-control' id='tipo_vehi'>";
      html += "          <option>- Seleccione -</option>";
      /*Consultando Tipos Vehiculo*/
      var cons_tipo = from tipo in db.tipo_vehiculo
                      select new { tipo.tipo_vehi, tipo.nomb_vehi };
      foreach (var item in cons_tipo)
      {
        html += "          <option value = '" + item.tipo_vehi + "'>" + item.nomb_vehi + "</option>";
      }
      html += "        </select>";
      html += "      </div>";
      html += "      <br>";
      html += "      <div class='form-group'>";
      html += "        <label>OBSERVACION</label>";
      html += "        <textarea class='form-control' id='obse_vehi'></textarea>";
      html += "      </div>";
      html += "      <div class='form-group'>";
      html += "        <button class='btn btn-success' Onclick=\"InsertarVehiculo()\">";
      html += "          <span class='glyphicon glyphicon-floppy-save'></span>";
      html += "        </button>";
      html += "      </div>";
      html += "    </div>";
      html += "  </div>";
      html += "</div>";
      return "Mensaje," + html;
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
