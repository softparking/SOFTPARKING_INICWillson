//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SoftParking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class servicio
    {
        public servicio()
        {
            this.detalle_pago = new HashSet<detalle_pago>();
        }
    
        public int codi_serv { get; set; }
        public System.DateTime fech_serv { get; set; }
        public System.DateTime hora_entr { get; set; }
        public System.DateTime hora_sali { get; set; }
        public int codi_empl { get; set; }
        public string plac_vehi { get; set; }
        public int codi_ubic { get; set; }
    
        public virtual ICollection<detalle_pago> detalle_pago { get; set; }
        public virtual empleado empleado { get; set; }
        public virtual vehiculo vehiculo { get; set; }
        public virtual ubicacion ubicacion { get; set; }
    }
}
