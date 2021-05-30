namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRmPoFromSaes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_sales_invoice", "PO_id", "dbo.Inv_po");
            DropIndex("dbo.Inv_sales_invoice", new[] { "PO_id" });
            DropColumn("dbo.Inv_sales_invoice", "PO_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_sales_invoice", "PO_id", c => c.Int());
            CreateIndex("dbo.Inv_sales_invoice", "PO_id");
            AddForeignKey("dbo.Inv_sales_invoice", "PO_id", "dbo.Inv_po", "Id");
        }
    }
}
