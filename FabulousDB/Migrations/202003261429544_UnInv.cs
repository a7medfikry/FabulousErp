namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnInv : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_quotation_request", "Recive_po_id", "dbo.Inv_receive_po");
            DropIndex("dbo.Inv_quotation_request", new[] { "Recive_po_id" });
            AddColumn("dbo.Inv_quotation_request", "Po_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_request", "Po_id");
            AddForeignKey("dbo.Inv_quotation_request", "Po_id", "dbo.Inv_purchase_request", "Id", cascadeDelete: false);
            DropColumn("dbo.Inv_quotation_request", "Recive_po_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_quotation_request", "Recive_po_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Inv_quotation_request", "Po_id", "dbo.Inv_purchase_request");
            DropIndex("dbo.Inv_quotation_request", new[] { "Po_id" });
            DropColumn("dbo.Inv_quotation_request", "Po_id");
            CreateIndex("dbo.Inv_quotation_request", "Recive_po_id");
            AddForeignKey("dbo.Inv_quotation_request", "Recive_po_id", "dbo.Inv_receive_po", "Id", cascadeDelete: true);
        }
    }
}
