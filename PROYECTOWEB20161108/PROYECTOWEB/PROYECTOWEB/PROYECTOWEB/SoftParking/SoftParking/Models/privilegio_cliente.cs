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
    
    public partial class privilegio_cliente
    {
        public privilegio_cliente()
        {
            this.cliente = new HashSet<cliente>();
        }
    
        public int codi_priv { get; set; }
        public string tipo_priv { get; set; }
        public int tari_priv { get; set; }
        public string esta_priv { get; set; }
        public System.DateTime fech_priv { get; set; }
    
        public virtual ICollection<cliente> cliente { get; set; }
    }
}
