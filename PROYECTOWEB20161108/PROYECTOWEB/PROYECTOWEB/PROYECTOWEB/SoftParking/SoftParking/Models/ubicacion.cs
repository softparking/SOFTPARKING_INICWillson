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
    
    public partial class ubicacion
    {
        public ubicacion()
        {
            this.servicio = new HashSet<servicio>();
        }
    
        public int codi_ubic { get; set; }
        public string tama_ubic { get; set; }
        public string esta_ubic { get; set; }
        public int piso_ubic { get; set; }
        public string dist_ubic { get; set; }
    
        public virtual ICollection<servicio> servicio { get; set; }
    }
}
