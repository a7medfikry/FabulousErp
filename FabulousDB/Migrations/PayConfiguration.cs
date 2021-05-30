namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class PayConfiguration : DbMigrationsConfiguration<FabulousErp.Payable.Models.PayableContext>
    {
        public PayConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FabulousErp.Payable.Models.PayableContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
