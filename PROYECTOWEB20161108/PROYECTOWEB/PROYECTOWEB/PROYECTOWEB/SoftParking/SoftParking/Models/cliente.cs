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
    
    public partial class cliente
    {
        public cliente()
        {
            this.vehiculo = new HashSet<vehiculo>();
        }
    
        public int codi_clie { get; set; }
        public int iden_clie { get; set; }
        public string nomb_clie { get; set; }
        public string apel_clie { get; set; }
        public int codi_priv { get; set; }
    
        public virtual privilegio_cliente privilegio_cliente { get; set; }
        public virtual ICollection<vehiculo> vehiculo { get; set; }
    }
}
