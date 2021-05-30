namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowAdjust : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fiscal_year_area", "Allow_adjust", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fiscal_year_area", "Allow_adjust");
        }
    }
}
