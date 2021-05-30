namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvT9 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            RenameColumn(table: "dbo.Inv_item_gl_accounts", name: "Item_group_id", newName: "Item_id");
            RenameIndex(table: "dbo.Inv_item_gl_accounts", name: "IX_Item_group_id", newName: "IX_Item_id");
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Inv_item_gl_accounts", name: "IX_Item_id", newName: "IX_Item_group_id");
            RenameColumn(table: "dbo.Inv_item_gl_accounts", name: "Item_id", newName: "Item_group_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
