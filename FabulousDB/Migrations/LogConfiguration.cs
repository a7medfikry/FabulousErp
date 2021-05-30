namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class LogConfiguration : DbMigrationsConfiguration<FabulousDB.DB_Context.LogContext>
    {
        public LogConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }
        protected override void Seed(FabulousDB.DB_Context.LogContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
