namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToTax : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tax", "Final_cost_price", c => c.Decimal(nullable: true, precision: 18, scale: 2,defaultValue:0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tax", "Final_cost_price");
        }
    }
}
