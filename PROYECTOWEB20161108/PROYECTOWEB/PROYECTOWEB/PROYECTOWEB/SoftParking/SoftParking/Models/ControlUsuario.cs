using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoftParking.Models
{
    public class ControlUsuario
    {
        [Key]
        public int UserID { get; set; }
        public String Usuario { get; set; }
        public String Rol { get; set; }
        public DateTime Fecha { get; set; }
        public string iniciaTurno { get; set; }
    }
}