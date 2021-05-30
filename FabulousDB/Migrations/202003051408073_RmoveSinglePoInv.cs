namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RmoveSinglePoInv : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_sales_invoice", "Inv_po_Id", "dbo.Inv_receive_po");
            DropIndex("dbo.Inv_sales_invoice", new[] { "Inv_po_Id" });
            DropColumn("dbo.Inv_sales_invoice", "Inv_po_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_sales_invoice", "Inv_po_Id", c => c.Int(nullable: false));
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_sales_invoice", "Inv_po_Id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            AddForeignKey("dbo.Inv_sales_invoice", "Inv_po_Id", "dbo.Inv_receive_po", "Id", cascadeDelete: true);
        }
    }
}
