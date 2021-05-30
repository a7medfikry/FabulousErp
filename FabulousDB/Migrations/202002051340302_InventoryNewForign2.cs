namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoryNewForign2 : DbMigration
    {
        public override void Up()
        {


            AlterColumn("dbo.Inv_receive_po", "Doc_type", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po", "Batch_id", c => c.Int(nullable: false));

        }

        public override void Down()
        {
            DropIndex("dbo.Inv_receive_po_items", new[] { "receive_po_Id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_receive_po_items", "Receive_po_id", c => c.Int());
            AlterColumn("dbo.Inv_receive_po_items", "receive_po_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po", "Batch_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po", "Doc_type", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_receive_po_items", "receive_po_Id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
