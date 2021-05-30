namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeForeignKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Inv_gorup_gl_accounts", name: "Returne_id", newName: "Inventory_Returne_id");
            RenameIndex(table: "dbo.Inv_gorup_gl_accounts", name: "IX_Returne_id", newName: "IX_Inventory_Returne_id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Inv_gorup_gl_accounts", name: "IX_Inventory_Returne_id", newName: "IX_Returne_id");
            RenameColumn(table: "dbo.Inv_gorup_gl_accounts", name: "Inventory_Returne_id", newName: "Returne_id");
        }
    }
}
