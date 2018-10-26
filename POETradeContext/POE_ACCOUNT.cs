namespace POETradeContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("poe_trader.POE_ACCOUNT")]
    public partial class POE_ACCOUNT
    {
        public long ID { get; set; }

        [StringLength(60)]
        public string ACCOUNT_NAME { get; set; }

        [StringLength(60)]
        public string LAST_CHAR_NAME { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? LAST_ITEM_ADDED { get; set; }
    }
}
