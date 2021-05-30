namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInvReturnId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Inv_item_gl_accounts", name: "Returne_id", newName: "Inventory_returne_id");
            RenameIndex(table: "dbo.Inv_item_gl_accounts", name: "IX_Returne_id", newName: "IX_Inventory_returne_id");
        }
        public override void Down()
        {
            RenameIndex(table: "dbo.Inv_item_gl_accounts", name: "IX_Inventory_returne_id", newName: "IX_Returne_id");
            RenameColumn(table: "dbo.Inv_item_gl_accounts", name: "Inventory_returne_id", newName: "Returne_id");
        }
    }
}
