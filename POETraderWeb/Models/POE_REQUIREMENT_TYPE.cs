//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POETraderWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class POE_REQUIREMENT_TYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public POE_REQUIREMENT_TYPE()
        {
            this.POE_REQUIREMENT = new HashSet<POE_REQUIREMENT>();
        }
    
        public long ID { get; set; }
        public string NAME { get; set; }
        public string TYPE_SYMBOL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POE_REQUIREMENT> POE_REQUIREMENT { get; set; }
    }
}
