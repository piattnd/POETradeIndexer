namespace POETradeContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class poe_accountDBModel : DbContext
    {
        public poe_accountDBModel()
            : base("name=poe_accountDBModel")
        {
        }

        public virtual DbSet<POE_ACCOUNT> POE_ACCOUNT { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<POE_ACCOUNT>()
                .Property(e => e.ACCOUNT_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<POE_ACCOUNT>()
                .Property(e => e.LAST_CHAR_NAME)
                .IsUnicode(false);
        }
    }
}
