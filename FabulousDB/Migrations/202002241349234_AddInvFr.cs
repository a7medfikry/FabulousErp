namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvFr : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_receive_po", new[] { "Inv_receive_po_Id" });
            //DropIndex("dbo.Inv_receive_po_items", new[] { "Receive_po_id" });
            //DropForeignKey("dbo.Inv_receive_po_items", "FK_dbo.Inv_receive_po_items_dbo.Inv_receive_po_Receive_po_id");
            //DropColumn("dbo.Inv_receive_po_items", "Receive_po_id");
            //RenameColumn(table: "dbo.Inv_receive_po_items", name: "Inv_receive_po_Id", newName: "Receive_po_id");
            AddColumn("dbo.Inv_receive_po", "Payable_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
           // DropColumn("dbo.Inv_receive_po", "Inv_receive_po_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_receive_po", "Inv_receive_po_Id", c => c.Int());
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_receive_po", "Payable_id");
            RenameColumn(table: "dbo.Inv_receive_po_items", name: "Receive_po_id", newName: "Inv_receive_po_Id");
            AddColumn("dbo.Inv_receive_po_items", "Receive_po_id", c => c.Int());
            CreateIndex("dbo.Inv_receive_po", "Inv_receive_po_Id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
