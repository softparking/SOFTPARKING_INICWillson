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
    
    public partial class vehiculo
    {
        public vehiculo()
        {
            this.servicio = new HashSet<servicio>();
            this.lista_negra = new HashSet<lista_negra>();
        }
    
        public string plac_vehi { get; set; }
        public string colo_vehi { get; set; }
        public string obse_vehi { get; set; }
        public string marc_vehi { get; set; }
        public int tipo_vehi { get; set; }
        public int codi_clie { get; set; }
    
        public virtual cliente cliente { get; set; }
        public virtual ICollection<servicio> servicio { get; set; }
        public virtual tipo_vehiculo tipo_vehiculo { get; set; }
        public virtual ICollection<lista_negra> lista_negra { get; set; }
    }
}
