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
    
    public partial class POE_REQUIREMENT
    {
        public long ID { get; set; }
        public Nullable<long> ITEM_ID { get; set; }
        public Nullable<sbyte> DISPLAY_MODE { get; set; }
        public Nullable<sbyte> TYPE { get; set; }
        public Nullable<sbyte> PROGRESS { get; set; }
        public Nullable<sbyte> IS_ADDITIONAL_REQUIREMENT { get; set; }
        public Nullable<int> REQUIREMENT_VALUE { get; set; }
        public Nullable<long> REQUIREMENT_TYPE_ID { get; set; }
        public string REQUIREMENT_VALUE_STRING { get; set; }
        public Nullable<sbyte> IS_NEXT_LEVEL_REQUIREMENT { get; set; }
    
        public virtual POE_ITEM POE_ITEM { get; set; }
        public virtual POE_REQUIREMENT_TYPE POE_REQUIREMENT_TYPE { get; set; }
    }
}
