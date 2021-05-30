namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovForiegn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Inv_sales_invoice_items", "FK_dbo.Inv_sales_invoice_items_dbo.Inv_receive_po_Inv_po_receive");
        }
        
        public override void Down()
        {
        }
    }
}
