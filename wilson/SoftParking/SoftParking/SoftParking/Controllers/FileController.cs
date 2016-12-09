using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SoftParking.Controllers
{
  public class FileController : ApiController
  {

    public IHttpActionResult Post()
    {
      var request = HttpContext.Current.Request;
      if (request.Files.Count > 0)
      {
        foreach (string file in request.Files)
        {
          var postedFile = request.Files[file];
          var filePath = HttpContext.Current.Server.MapPath(string.Format("~/Uploads/{0}", postedFile.FileName));
          postedFile.SaveAs(filePath);
        }
        return Ok(true);
      }
      else
      {
        return BadRequest();
      }
    }
  }
}
