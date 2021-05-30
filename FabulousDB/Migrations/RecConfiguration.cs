namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class RecConfiguration : DbMigrationsConfiguration<FabulousErp.Receivable.Models.ReceivableContext>
    {
        public RecConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FabulousErp.Receivable.Models.ReceivableContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
