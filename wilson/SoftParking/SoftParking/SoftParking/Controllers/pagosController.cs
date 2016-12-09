using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoftParking.Models;
using SoftParking.Clases;

namespace SoftParking.Controllers
{
  public class pagosController : Controller
  {
    private parqueaderoEntities db = new parqueaderoEntities();
    private parqueaderoEntities dbac = new parqueaderoEntities();
    // GET: pagos
    public ActionResult Index()
    {
      return View(db.pago.ToList());
    }


    //vista de pagos 
    public ActionResult Create(int? codServ, string Placa)
    {
      ViewBag.estado = Placa;
      if (Placa != null)
      {
        try
        {
          CalcularPrecioXminuto calcularT = new CalcularPrecioXminuto();
          int IVA = 16;
          string tiempoT = "SELECT  timediff(timestamp(CONCAT(fech_sali, ' ', hora_sali)), timestamp(CONCAT(fech_serv, ' ', hora_entr))) as diferencia from servicio where plac_vehi='" + Placa + "' and codi_serv='" + codServ + "'  and   fech_sali is not null and hora_sali is not null";

          var comprobacion = db.Database.SqlQuery<string>(tiempoT).FirstOrDefault();
          // el resultado lo transformo a tiempo

          TimeSpan tiempoTotal = TimeSpan.Parse(comprobacion);
          //valor por minuto 
          //obtenemos el valor del tipo de vehiculo 
          string TarifaVehi = "select  tari_vehi from  tipo_vehiculo  join vehiculo on tipo_vehiculo.tipo_vehi =vehiculo.tipo_vehi where plac_vehi='" + Placa + "'";
          var consultaTarifa = db.Database.SqlQuery<string>(TarifaVehi).FirstOrDefault();
          double precio = double.Parse(consultaTarifa);
          double valorTiempo = calcularT.ValorMinuto(tiempoTotal, precio);
          double porcentajeIva = (valorTiempo * IVA) / 100;
          double ValorAPagar = (porcentajeIva + valorTiempo);
          /*----*facturacion*----*/

          ViewBag.PlacasPago = Placa;
          ViewBag.CodiServicio = codServ;
          ViewBag.TiempoTotal = comprobacion;
          ViewBag.valorTiempo = valorTiempo;
          ViewBag.porcentajeIva = porcentajeIva;
          ViewBag.ValorAPagar = ValorAPagar;

          /*----*Datos Adicionales*----*/
          //TIPOVEHICULO
          var consultaVehi = db.Database.SqlQuery<string>("select nomb_vehi from tipo_vehiculo join vehiculo on tipo_vehiculo.tipo_vehi = vehiculo.tipo_vehi where vehiculo.plac_vehi='" + Placa + "'").FirstOrDefault();
          double precioMin = precio / 60;
          ViewBag.TipoVehiculo = consultaVehi;
          ViewBag.PrecioVehi = precio;
          ViewBag.PrecioMin = precioMin;
          ViewBag.ConsultaTarifa = consultaTarifa;

        }
        catch (Exception e)
        {


        }
      }
      else
      {

      }
      return View();
    }

    // Generacion de pago  cliente ocasional 
    public ActionResult Gpagos()
    {
      return View();

    }


    [HttpPost]
    public ActionResult Gpagos(int? consecutivo, TimeSpan tiempoTotal, string ValorTiemp, DateTime fecha, int numFacServ, string metodo_pago, double ivaPago, double totalAPagar, string placa)
    {

      pago TablaPago = new pago();
      detalle_pago TabladetaleP = new detalle_pago();

      try
      {
        //insertar pagos           


        TablaPago.codi_pago = consecutivo.GetValueOrDefault();
        TablaPago.nume_fact = numFacServ;
        TablaPago.tipo_pago = metodo_pago;
        TablaPago.fech_pago = fecha;
        TablaPago.iva_pago = (int)ivaPago;
        TablaPago.tota_pago = totalAPagar;
        db.pago.Add(TablaPago);
        db.SaveChanges();

        string CodigoPago = db.Database.SqlQuery<string>("select codi_pago from pago where nume_fact = '" + numFacServ + "'").FirstOrDefault();
        int codigosservTpago = int.Parse(CodigoPago);

        TabladetaleP.idet_pago = consecutivo.GetValueOrDefault();
        TabladetaleP.nume_fact = numFacServ;
        TabladetaleP.tiem_tota = tiempoTotal;
        TabladetaleP.tota_deta = int.Parse(ValorTiemp);
        TabladetaleP.codi_serv = numFacServ;
        TabladetaleP.codi_pago = codigosservTpago;
        db.detalle_pago.Add(TabladetaleP);
        db.SaveChanges();

        ViewBag.ttota = tiempoTotal;
        ViewBag.valtiem = ValorTiemp;
        ViewBag.fechTa = fecha;
        ViewBag.numserv = numFacServ;
        ViewBag.metpag = metodo_pago.ToUpper();
        ViewBag.eliva = ivaPago;
        ViewBag.totapag = totalAPagar;
        ViewBag.Placa = placa;


      }
      catch (Exception e)
      {

      }
      return View();
    }
    
    public ActionResult facturaIngreso(string placaVehiculo)
    {
      if (placaVehiculo != null)
      {
        try
        {
          var codigodeServicio = dbac.Database.SqlQuery<string>("select codi_serv from servicio  where plac_vehi ='" + placaVehiculo.ToUpper() + "' and fech_sali is null  and   hora_sali is NULL").FirstOrDefault();
          int numero = int.Parse(codigodeServicio);
          var CodVahia = dbac.Database.SqlQuery<string>("select codi_ubic from servicio where plac_vehi='" + placaVehiculo + "' and fech_sali is null and hora_sali is null").FirstOrDefault();

          string Piso = "select piso_ubic from   ubicacion where codi_ubic = '" + CodVahia + "'";
          string distintivo = "select dist_ubic from   ubicacion where codi_ubic = '" + CodVahia + "'";

          var CODUBICAPISO = dbac.Database.SqlQuery<string>(Piso).FirstOrDefault();
          var CODUBICALETRA = dbac.Database.SqlQuery<string>(distintivo).FirstOrDefault();

          ViewBag.idServicio = numero;
          ViewBag.placa = placaVehiculo.ToUpper();
          ViewBag.pisoUbicacion = CODUBICAPISO;
          ViewBag.distintivo = CODUBICALETRA;
          ViewBag.CodVahia = CodVahia;
          ViewBag.fechaIng = DateTime.Now;
        }
        catch (Exception e)
        {
          return HttpNotFound();
        }
      }
      return View();
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
