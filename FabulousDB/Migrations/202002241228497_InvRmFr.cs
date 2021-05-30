namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRmFr : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_receive_po", "PO_id", "dbo.Inv_po");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_receive_po", new[] { "PO_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po", "PO_id", c => c.Int());
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_receive_po", "PO_id");
            AddForeignKey("dbo.Inv_receive_po", "PO_id", "dbo.Inv_po", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po", "PO_id", "dbo.Inv_po");
            DropIndex("dbo.Inv_receive_po", new[] { "PO_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_receive_po", "PO_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_receive_po", "PO_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            AddForeignKey("dbo.Inv_receive_po", "PO_id", "dbo.Inv_po", "Id", cascadeDelete: true);
        }
    }
}
