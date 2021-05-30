namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPosEdit : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Inv_sales_receivs_pos", name: "Po_id", newName: "Receive_po_id");
            RenameIndex(table: "dbo.Inv_sales_receivs_pos", name: "IX_Po_id", newName: "IX_Receive_po_id");
            AddColumn("dbo.Inv_sales_receivs_pos", "Item_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_sales_receivs_pos", "Item_id");
            AddForeignKey("dbo.Inv_sales_receivs_pos", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_receivs_pos", "Item_id", "dbo.Inv_item");
            DropIndex("dbo.Inv_sales_receivs_pos", new[] { "Item_id" });
            DropColumn("dbo.Inv_sales_receivs_pos", "Item_id");
            RenameIndex(table: "dbo.Inv_sales_receivs_pos", name: "IX_Receive_po_id", newName: "IX_Po_id");
            RenameColumn(table: "dbo.Inv_sales_receivs_pos", name: "Receive_po_id", newName: "Po_id");
        }
    }
}
