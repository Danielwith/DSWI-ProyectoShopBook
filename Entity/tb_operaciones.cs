//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShopBook.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_operaciones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_operaciones()
        {
            this.tb_rol_operacion = new HashSet<tb_rol_operacion>();
        }
    
        public int idOperacion { get; set; }
        public string acceso { get; set; }
        public int idModulo { get; set; }
    
        public virtual tb_modulo tb_modulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_rol_operacion> tb_rol_operacion { get; set; }
    }
}
