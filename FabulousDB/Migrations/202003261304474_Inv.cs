namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inv : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Inv_quotation_request", "FK_dbo.Inv_request_for_quotation_dbo.Inv_item_Item_id");
            DropForeignKey("dbo.Inv_quotation_request", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_quotation_receiving", "Vendore_Id", "dbo.Payable_creditor_setting");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_quotation_request", new[] { "Item_id" });
            AddColumn("dbo.Inv_quotation_request", "Recive_po_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_request", "Within_days", c => c.Single(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_id");
            CreateIndex("dbo.Inv_quotation_request", "Recive_po_id");
            AddForeignKey("dbo.Inv_quotation_request", "Recive_po_id", "dbo.Inv_receive_po", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Inv_quotation_receiving", "Vendore_id", "dbo.Payable_creditor_setting", "Id", cascadeDelete: false);
            DropColumn("dbo.Inv_quotation_request", "Item_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_quotation_request", "Item_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Inv_quotation_receiving", "Vendore_id", "dbo.Payable_creditor_setting");
            DropForeignKey("dbo.Inv_quotation_request", "Recive_po_id", "dbo.Inv_receive_po");
            DropIndex("dbo.Inv_quotation_request", new[] { "Recive_po_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_id" });
            AlterColumn("dbo.Inv_quotation_request", "Within_days", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            DropColumn("dbo.Inv_quotation_request", "Recive_po_id");
            CreateIndex("dbo.Inv_quotation_request", "Item_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            AddForeignKey("dbo.Inv_quotation_receiving", "Vendore_Id", "dbo.Payable_creditor_setting", "Id");
            AddForeignKey("dbo.Inv_quotation_request", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
        }
    }
}
