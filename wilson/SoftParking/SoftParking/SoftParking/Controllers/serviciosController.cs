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

      var tiempo = db.Database.SqlQuery<List<string>>("SELECT  timediff(timestamp(CONCAT(fech_sali, ' ', hora_sali)), timestamp(CONCAT(fech_serv, ' ', hora_entr))) as diferencia from servicio where fech_sali is not null and hora_sali is not null").FirstOrDefault();

      return View(db.servicio.ToList());
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
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AcceptVerbs(HttpVerbs.Post)]
    //el parametro int? es  para indicarle  que es un tipo de dato opcional como la tabla  es un autoincremental  no es necesario asignarle valores 
    public ActionResult Create(int? codi_serv, string fech_serv, string hora_entr, int codi_empl, string plac_vehi, int tipo_vehi, int? AuxConsecutivo)
    {  //busca un espacio disponibe 

      var dipo = db.Database.SqlQuery<string>("select count(*) from ubicacion where  esta_ubic='DISPONIBLE'").FirstOrDefault();

      //VErifica  el doble ingreso del  vehiculo en la area  de  servicio 
      var placaDupli = db.Database.SqlQuery<string>("select plac_vehi from servicio  where plac_vehi='" + plac_vehi + "' and  fech_sali is null and hora_sali is null;").FirstOrDefault();
      //instancia de la clase servicio
      servicio serv = new servicio();
      int esp = int.Parse(dipo);

      if (esp != 0)
      {
        try
        {
          // select al cliente generico   alojado en la base de datos 
          var ClienteGenerico = db.Database.SqlQuery<List<string>>("SELECT * FROM cliente WHERE iden_clie='99'").FirstOrDefault();
          //selecciona el primer registro del consecutivo de la base de datos  para ingresar el primer espacio disponible  
          var tam_Vahias = db.Database.SqlQuery<string>("select codi_ubic from ubicacion  where esta_ubic='DISPONIBLE'  ORDER BY codi_ubic ASC limit 1").FirstOrDefault();
          //si las placas  del cliente  no existe en la base de datos el sistema creara uno generico si existe salta este paso 
          var clienteGenerico = db.Database.SqlQuery<List<string>>("SELECT plac_vehi from vehiculo join cliente  on vehiculo.codi_clie=cliente.codi_clie  where vehiculo.plac_vehi='" + plac_vehi.ToUpper() + "' AND cliente.iden_clie='99'").FirstOrDefault();
          //consulta para hallar un cliente registrado
          var clienteRegistro = db.Database.SqlQuery<List<string>>("SELECT plac_vehi from vehiculo join cliente  on vehiculo.codi_clie=cliente.codi_clie  where vehiculo.plac_vehi='" + plac_vehi.ToUpper() + "' AND cliente.iden_clie!='99' and nomb_clie is not null and apel_clie is not null").FirstOrDefault();
          if (ClienteGenerico == null)
          {
            string QUERYINSERT = "insert into cliente (codi_clie,iden_clie,nomb_clie,apel_clie,codi_priv) values (NULL,99,NULL,NULL,'1')";
            db.Database.ExecuteSqlCommand(QUERYINSERT);
            ViewData["ALERTA"] = "NO EXISTE USUARIO GENERICO  EN LA BASE DE DATOS  SE GENERO USUARIO CON CODIGO 99 ";
            return RedirectToAction("Create", "Servicios");
          }
          /********      validacion         *********
          ********    usuario generico   *********/
          else
          {   //si tam_Vahias es nulo=(encontrar espacios disponibles) y la placa de veiculo es nula  le asigna por defecto el usuario generico 99 a la nueva placa   
            if (tam_Vahias != null && clienteGenerico == null && clienteRegistro == null)
            {   //otengo directamente el id del cliente  generico o cliente OCASIONAL para asignarlo 
              string idClient = "select codi_clie from cliente where iden_clie='99'";
              var Idclinet = db.Database.SqlQuery<string>(idClient).FirstOrDefault();
              // realizo un  casting a la consulta tam_Vahias y  lo transformo a un  entero  para luego asignarle  el id de la ubicacion  al servicio a la variable serv.codi_ubic
              int numVahihas = int.Parse(tam_Vahias);
              // insercion de datos a par de las clases de las entidades 
              vehiculo vehi = new vehiculo();
              vehi.plac_vehi = plac_vehi.ToUpper();
              vehi.colo_vehi = null;
              vehi.obse_vehi = null;
              vehi.marc_vehi = null;
              vehi.tipo_vehi = tipo_vehi;
              vehi.codi_clie = int.Parse(Idclinet);
              db.vehiculo.Add(vehi);
              db.SaveChanges();

              //actualizo el estado de la bahia  OCUPADO
              db.Database.ExecuteSqlCommand("UPDATE ubicacion SET esta_ubic = 'OCUPADO' WHERE ubicacion.codi_ubic ='" + numVahihas + "'");

              //crea el nuevo servicio 

              serv.codi_serv = codi_serv.GetValueOrDefault();
              serv.fech_serv = DateTime.Today;
              serv.hora_entr = TimeSpan.ParseExact(hora_entr, "hh\\:mm\\:ss", null);
              serv.codi_empl = codi_empl;
              serv.plac_vehi = plac_vehi.ToUpper();
              serv.codi_ubic = numVahihas;
              db.servicio.Add(serv);
              db.SaveChanges();

              TempData["MSGEXITOSO"] = "EL VEHICULO FUE REGISTRADO SATISFACTORIAMENTE";
              ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", serv.codi_empl);
              ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", serv.plac_vehi);
              ViewBag.codi_ubic = new SelectList(db.ubicacion, "codi_ubic", "tama_ubic", serv.codi_ubic);

              //bandera para generar el ticket
              TempData["EviarTicket"] = plac_vehi;

              return RedirectToAction("Create");


            }
            //esta validacion es cuando  la placa del cliente generico ya habia sido registrada con anteriorida en la dba
            else if (tam_Vahias != null && clienteGenerico != null && placaDupli == null && clienteRegistro == null)
            {

              int numVahihas = int.Parse(tam_Vahias);
              //actualizo el estado de la bahia  OCUPADO
              db.Database.ExecuteSqlCommand("UPDATE ubicacion SET esta_ubic = 'OCUPADO' WHERE ubicacion.codi_ubic ='" + numVahihas + "'");

              //crea el nuevo servicio 

              serv.codi_serv = codi_serv.GetValueOrDefault();
              serv.fech_serv = DateTime.Today;
              serv.hora_entr = TimeSpan.ParseExact(hora_entr, "hh\\:mm\\:ss", null);
              serv.codi_empl = codi_empl;
              serv.plac_vehi = plac_vehi.ToUpper();
              serv.codi_ubic = numVahihas;//inserta el id  de la vahia al servicio
              db.servicio.Add(serv);
              db.SaveChanges();

              TempData["MSGEXITOSO"] = "EL VEHICULO FUE REGISTRADO SATISFACTORIAMENTE";
              ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", serv.codi_empl);
              ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", serv.plac_vehi);
              ViewBag.codi_ubic = new SelectList(db.ubicacion, "codi_ubic", "tama_ubic", serv.codi_ubic);

              //bandera para generar el ticket
              TempData["EviarTicket"] = plac_vehi;
              return RedirectToAction("Create");
            }
            else if (tam_Vahias != null && clienteGenerico != null && placaDupli != null)
            {

              TempData["Dobleingreso"] = "EL VEHICULO YA SE ENCUENTRA EN UNA VAHIA ";
              ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", serv.codi_empl);
              ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", serv.plac_vehi);
              ViewBag.codi_ubic = new SelectList(db.ubicacion, "codi_ubic", "tama_ubic", serv.codi_ubic);

              return RedirectToAction("Create");
            }
            /********      validacion         *********
             ********    usuario registrado   *********/

            //valida que el cliente registrado no se haya insertado anteriormente 
            else if (tam_Vahias != null && clienteRegistro != null && placaDupli == null && clienteGenerico == null)
            {

              int numVahihas = int.Parse(tam_Vahias);
              //actualizo el estado de la bahia  OCUPADO
              db.Database.ExecuteSqlCommand("UPDATE ubicacion SET esta_ubic = 'OCUPADO' WHERE ubicacion.codi_ubic ='" + numVahihas + "'");

              //crea el nuevo servicio 

              serv.codi_serv = codi_serv.GetValueOrDefault();
              serv.fech_serv = DateTime.Today;
              serv.hora_entr = TimeSpan.ParseExact(hora_entr, "hh\\:mm\\:ss", null);
              serv.codi_empl = codi_empl;
              serv.plac_vehi = plac_vehi.ToUpper();
              serv.codi_ubic = numVahihas;//inserta el id  de la vahia al servicio
              db.servicio.Add(serv);
              db.SaveChanges();

              TempData["MSGEXITOSO"] = "EL VEHICULO FUE REGISTRADO SATISFACTORIAMENTE";
              ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", serv.codi_empl);
              ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", serv.plac_vehi);
              ViewBag.codi_ubic = new SelectList(db.ubicacion, "codi_ubic", "tama_ubic", serv.codi_ubic);

              //bandera para generar el ticket
              TempData["EviarTicket"] = plac_vehi.ToUpper();
              return RedirectToAction("Create");
            }
            else if (tam_Vahias != null && clienteRegistro != null && placaDupli != null)
            {

              TempData["Dobleingreso"] = "EL VEHICULO YA SE ENCUENTRA EN UNA VAHIA ";
              ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", serv.codi_empl);
              ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", serv.plac_vehi);
              ViewBag.codi_ubic = new SelectList(db.ubicacion, "codi_ubic", "tama_ubic", serv.codi_ubic);

              return RedirectToAction("Create");
            }
          }
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          return HttpNotFound();
        }
      }//fin de hayESP
       /********      validacion          *********
        ********    espacios disponibles   *********/
      else if (esp == 0)
      {

        TempData["SinEspacio"] = "YA NO PUEDES  INGRESAR MAS VEHICULOS DEBIDO A QUE SE ENCUENTRA LLENO";
      }

      ViewBag.codi_empl = new SelectList(db.empleado, "codi_empl", "nomb_empl", serv.codi_empl);
      ViewBag.plac_vehi = new SelectList(db.vehiculo, "plac_vehi", "colo_vehi", serv.plac_vehi);
      ViewBag.codi_ubic = new SelectList(db.ubicacion, "codi_ubic", "tama_ubic", serv.codi_ubic);
      ViewBag.tipo_vehi = new SelectList(db.tipo_vehiculo, "tipo_vehi", "nomb_vehi");
      var consulta = "select count(*) from ubicacion";
      var dispo = "select count(*) from ubicacion where  esta_ubic='DISPONIBLE'";
      var ocup = "select count(*) from ubicacion where  esta_ubic='OCUPADO'";
      var valore = db.Database.SqlQuery<string>(consulta).FirstOrDefault();
      ViewBag.vahias = valore;
      ViewBag.disponible = db.Database.SqlQuery<string>(dispo).FirstOrDefault();
      ViewBag.ocupado = db.Database.SqlQuery<string>(ocup).FirstOrDefault();
      return View();

    }

    //GENERAR SALIDA DEL VEHICULO 
    [HttpPost]
    public ActionResult Salida(string plac_vehi, string hora_entr)
    {   //a fecha de salida le asigno la fecha actual  convertida string con el formato "yyyyMMdd"
      string fechaSalida = DateTime.Today.ToString("yyyyMMdd");
      var horaSalida = TimeSpan.ParseExact(hora_entr, "hh\\:mm\\:ss", null);

      string consultaData = "select codi_serv from servicio  where plac_vehi ='" + plac_vehi.ToUpper() + "' and fech_sali is null  and   hora_sali is NULL";

      var comprobacion = db.Database.SqlQuery<string>(consultaData).FirstOrDefault();
      if (comprobacion != null)
      {
        try
        {
          //esta consulta actualiza tanto la tabla de servicio como la tabla ubicacion añadiendo los campos nulos de fecha y hora de salida y cambiando el estado a Disponible de la tabla ubicacion 
          string queryUpd = "UPDATE servicio join ubicacion ON servicio.codi_ubic = ubicacion.codi_ubic SET servicio.fech_sali = '" + fechaSalida + "', servicio.hora_sali = '" + horaSalida + "', ubicacion.esta_ubic = 'DISPONIBLE'" +
                            "WHERE servicio.plac_vehi = '" + plac_vehi.ToUpper() + "' and servicio.fech_sali is NULL and servicio.hora_sali is NULL";
          db.Database.ExecuteSqlCommand(queryUpd);

          //envio los valores basicos al controlador pagos asigno el  codigo de servicio  
          int codigoServ = int.Parse(comprobacion);
          return RedirectToAction("Create", "pagos", new { codServ = codigoServ, Placa = plac_vehi });
        }
        catch (System.Data.SqlClient.SqlException ex)
        {

          return HttpNotFound();
        }

      }
      else
      {
        string info = "DEBES REGISTRAR LA PLACA : !" + plac_vehi.ToUpper() + "! EN UNA VAHIA DISPONIBLE  ";
        TempData["ServErr"] = info;

      }

      return RedirectToAction("Create", "servicios");
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
