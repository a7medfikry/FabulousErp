namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTaxFix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tax", "Final_cost_price");
            AddColumn("dbo.Tax", "Final_cost_price", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));

        }

        public override void Down()
        {
        }
    }
}
