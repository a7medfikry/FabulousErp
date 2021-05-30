namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvGroupToItem : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            //DropColumn("dbo.Inv_item", "Item_group_id");
            //RenameColumn(table: "dbo.Inv_item", name: "Inv_item_group_Id", newName: "Item_group_id");
            //RenameIndex(table: "dbo.Inv_item", name: "IX_Inv_item_group_Id", newName: "IX_Item_group_id");
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            //CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
        
        public override void Down()
        {
            //DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            //RenameIndex(table: "dbo.Inv_item", name: "IX_Item_group_id", newName: "IX_Inv_item_group_Id");
            //RenameColumn(table: "dbo.Inv_item", name: "Item_group_id", newName: "Inv_item_group_Id");
            //AddColumn("dbo.Inv_item", "Item_group_id", c => c.Int());
            //CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
