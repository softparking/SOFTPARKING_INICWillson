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
    
    public partial class empleado
    {
        public empleado()
        {
            this.servicio = new HashSet<servicio>();
            this.turno = new HashSet<turno>();
        }
    
        public int codi_empl { get; set; }
        public long iden_empl { get; set; }
        public string nomb_empl { get; set; }
        public string apel_empl { get; set; }
        public string dire_empl { get; set; }
        public string usua_empl { get; set; }
        public string cont_empl { get; set; }
        public string carg_empl { get; set; }
    
        public virtual ICollection<servicio> servicio { get; set; }
        public virtual ICollection<turno> turno { get; set; }
    }
}