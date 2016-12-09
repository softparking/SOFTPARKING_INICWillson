using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SoftParking.Controllers
{
  public class HomeController : Controller
  {
    public string fname ="";
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Contactenos()
    {
      ViewBag.Message = "Contactenos";
      return View();
    }

    [System.Web.Http.HttpPost]
    public string Email(string cont_emai)
    {
      SmtpClient server = new SmtpClient("smtp.gmail.com", 587);
      server.Credentials = new System.Net.NetworkCredential("softparkingcs@gmail.com", "sistemas");
      server.EnableSsl = true;
      string output = null;
      try
      {
        MailMessage mnsj = new MailMessage();
        mnsj.Subject = "Contacto Softpaking";

        mnsj.To.Add(new MailAddress("wdaguilar6@misena.edu.co"));
        //mnsj.To.Add(new MailAddress("faamaris@misena.edu.co"));
        //mnsj.To.Add(new MailAddress("oardila6@misena.edu.co"));
        //mnsj.To.Add(new MailAddress("jarodriguez742@misena.edu.co"));

        mnsj.From = new MailAddress("softparkingcs@gmail.com", "SoftParking");

        /* Si deseamos Adjuntar algún archivo*/
        //mnsj.Attachments.Add(new Attachment("~/App_Data/Uploads/" + fname));

        /*Contenido*/
        var contenido = cont_emai;      
        var fecha = @DateTime.Now.ToLongDateString().ToString();
        contenido += "<br><br>" + fecha;        
        mnsj.IsBodyHtml = true;
        mnsj.Body = contenido;

        /* Enviar */
        server.Send(mnsj);
        output = "Mensaje,Correo Enviado Satisfactoriamente..!";
      }
      catch (Exception e)
      {
        output = "Error,Ocurrio Un Error En El Envio Por Favor Intente De Nuevo O Mas Tarde..!! (" + e + ")";
      }
      return output;
    }  

    public ActionResult QuienesSomos()
    {
      ViewBag.Message = "Quienes Somos";

      return View();
    }
    public ActionResult NuestraEmpresa()
    {
      ViewBag.Message = " Nuestra Empresa";

      return View();
    }

    public ActionResult Vision()
    {
      ViewBag.Message = "Vision";

      return View();
    }

    public ActionResult Mision()
    {
      ViewBag.Message = "Mision";

      return View();
    }
  }
}