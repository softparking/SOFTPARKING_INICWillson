using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftParking.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contactenos()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult QuienesSomos()
        {
            ViewBag.Message = "quienes somos.";

            return View();
        }
        public ActionResult NuestraEmpresa()
        {
            ViewBag.Message = " Nestra empresa.";

            return View();
        }

        public ActionResult Historia()
        {
            ViewBag.Message = "Nuestra Historia";

            return View();
        }

        public ActionResult Politicas()
        {
            ViewBag.Message = "Nuestra Historia";

            return View();
        }


    }
}