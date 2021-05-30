namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFrighToSales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_invoice_items", "Fright", c => c.Decimal(nullable: false, precision: 18, scale: 2,defaultValue:0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_sales_invoice_items", "Fright");
        }
    }
}
