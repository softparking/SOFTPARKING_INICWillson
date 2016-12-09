using System;
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

                Session["ControlUsuario"] = new ControlUsuario() { UserID = query2, Usuario = query3, Rol = query4 ,Fecha=fechainicio,iniciaTurno="no"};

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


       

    }
}