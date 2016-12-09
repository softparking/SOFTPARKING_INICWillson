using System;
using System.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoftParking.Models;

namespace SoftParking.Controllers
{
  public class LogeoController : Controller
  {
    public int variableUsuario=0;        

    private parqueaderoEntities db = new parqueaderoEntities();

    public ActionResult login()
    {

        return View();
    }

    [HttpPost]
    public ActionResult login(String usuario, String password)
    {
        var resultado = Validar(usuario, password);

        if (resultado == true)
        {
            return RedirectToAction("Index", "Home");
        }
        else {

            ViewBag.fallo = "USUARIO O CONTRASEÑA INCORRECTOS";                
            return View();
            ////return RedirectToAction("Error", "Logeo");
        }
          
    }


// funcion    que me  permite  comprobar el  usuario y contraseña 

    private bool Validar(String Usuario, String Password)
    {

        /* empleado empleadoUs =  db.empleado.Single(p => p.usua_empl == Usuario);
          empleado empleadoCon = db.empleado.Single(p => p.cont_empl == Password);
        */
        var query = (from u in db.empleado
                      where u.usua_empl == Usuario && u.cont_empl == Password
                      select u).FirstOrDefault();

        //si la consulta query  es correcta creamos la sesion a ese usuario

        if (query != null) {
            //consulta  para obtener  el id de usuario el nombre  y el rol  si lo encuentra  se asigna y crea una nueva sesion
            var query2 = (from u in db.empleado
                          where u.usua_empl == Usuario && u.cont_empl == Password
                          select u.codi_empl).FirstOrDefault();
            var query3 = (from u in db.empleado
                          where u.usua_empl == Usuario && u.cont_empl == Password
                          select u.usua_empl).FirstOrDefault();

            var query4 = (from u in db.empleado
                          where u.usua_empl == Usuario && u.cont_empl == Password
                          select u.carg_empl).FirstOrDefault();

            //fecha inicio de login 
            var fechainicio= @DateTime.Now.ToLocalTime();

            variableUsuario = query2;


            Session["ControlUsuario"] = new ControlUsuario() { UserID = query2, Usuario = query3, Rol = query4 ,Fecha=fechainicio};

            /* Session["UserID"] = query2;
              Session["Usuario"] = query3;
            */
            return true;
        }

        else { 
            return false;
        }            
    }

    // funcion cierre de sesion de usuario
    public ActionResult logout() {
        Session.Clear();
        return RedirectToAction("Index","Home");
    }

    public string ValidaUsuario(String usuario,String contraseña)
    {      
      try
      {
        string carg_empl = "";        
        int codi_empl = 0;
        var cons_logi = from tabl in db.empleado
                        where tabl.usua_empl.Equals(usuario)
                        && tabl.cont_empl.Equals(contraseña)
                        select new { tabl.carg_empl , tabl.codi_empl};        
        foreach(var item in cons_logi)
        {          
          carg_empl = item.carg_empl;
          codi_empl = item.codi_empl;
        }

        if (carg_empl == "OPERARIO")
        {          
          var html = "";
          html += "Confirmar,";
          html += " <div class='container-fluid'>";
          html += "   <div class = 'row'>";
          html += "   <div class = 'col-md-3'></div>";          
          html += "     <div class = 'col-md-6'>";
          html += "       <div class ='alert alert-success text-center'><b>Iniciar Turno</b></div>";
          html += "       <div><input type='hidden' id ='hide_codi_empl' value='"+codi_empl+"'></div>";
          html += "       <div class = 'form-group'>";
          html += "         <label>Numero Caja:</label>";                  
          html += "         <select id='comb_nume_caja' class='form-control'>";
          html += "           <option value = ''>-Seleccione-</option>";

          /*Consultando Cajas Disponibles*/
          try
          {
            var info_caja = from tabl in db.caja
                            where tabl.esta_caja.Equals("ACT")
                            select new { tabl.codi_caja, tabl.nomb_caja };
            foreach (var item in info_caja)
            {
              html += "       <option value='"+item.codi_caja+"'>"+item.nomb_caja+"</option>";
            }
          }
          catch
          {
            html += "         <option>Sin Cajas Disponibles</option>";
          }         
          html += "         </select>";          
          html += "       </div>";
          html += "       <div class = 'form-group'>";
          html += "         <label>Monto Recibido</label>";
          html += "         <input type = 'number' class='form-control' id='txt_mont_caja' placeholder='Monto Inicial'>";          
          html += "       </div>";
          html += "       <div class ='form-group'>";
          html += "         <label>Observacion Turno</label>";
          html += "         <textarea id='txt_obse_turn' class ='form-control'></textarea>";
          html += "       </div>";
          html += "     </div>";
          html += "   <div class = 'col-md-3'></div>";
          html += "   </div>";
          html += " </div>;IniciarTurno()";
          return html;
        }
        else if (carg_empl == "ADMINISTRADOR")
        {
          Validar(usuario, contraseña);
          return "Location,/Home/Index";
        }
        else
        {
          return "Error,Usuario / Contraseña Incorrectos Por Favor Verifique..!!";
        }
      }
      catch
      {
        return "Error,Usuario / Contraseña Incorrectos Por Favor Verifique..!!";
      }           
    }

    public string IniciarTurno(int codi_caja, int mont_caja,int codi_empl,string desc_turn,string codi_usua,string clav_usua)
    {
      try
      {
        //turno inic_turno = new turno();        
        //inic_turno.caja.codi_caja = codi_caja;        
        //inic_turno.mont_inic = mont_caja;
        //inic_turno.empleado.codi_empl = codi_empl;
        //inic_turno.fech_inic = fech_actu;
        //inic_turno.desc_turn = desc_turn;
        //db.turno.Add(inic_turno);
        //db.SaveChanges();
        int nuev_turn;
        try
        {
          var codigo = from turno in db.turno
                       select turno.codi_turn;
          nuev_turn = codigo.Max();
        }
        catch
        {
          nuev_turn = 0;
        }
       
        nuev_turn++;
        var insertar = "INSERT INTO turno (codi_turn, codi_empl, codi_caja, fech_inic, mont_inic, desc_turn)";
        insertar += "VALUES('"+nuev_turn+"','"+codi_empl+"','"+codi_caja+"',now(),'"+mont_caja+"','"+desc_turn+"');";
        db.Database.BeginTransaction();
        db.Database.ExecuteSqlCommand(insertar);        
        db.Database.CurrentTransaction.Commit();
        Validar(codi_usua, clav_usua);
        return "Location,/Home/Index";        
      }
      catch(Exception e)
      {
        db.Database.CurrentTransaction.Rollback();
        return "Error,Ocurrio Un Error En El Inicio De Turno Por Favor Verifique..!! ("+e+")";        
      }
    }
  }
}