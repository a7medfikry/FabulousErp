namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPricelvl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_invoice", "Price_lvl", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_sales_invoice", "Price_lvl");
        }
    }
}
