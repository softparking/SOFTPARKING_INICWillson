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
    
    public partial class pago
    {
        public pago()
        {
            this.detalle_pago = new HashSet<detalle_pago>();
        }
    
        public int codi_pago { get; set; }
        public int nume_fact { get; set; }
        public string tipo_pago { get; set; }
        public System.DateTime fech_pago { get; set; }
        public Nullable<int> iva_pago { get; set; }
        public double tota_pago { get; set; }
    
        public virtual ICollection<detalle_pago> detalle_pago { get; set; }
    }
}
