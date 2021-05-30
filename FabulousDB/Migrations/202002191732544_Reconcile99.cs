namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reconcile99 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_item", "Item_id", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Inv_item", "Description", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_item", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.Inv_item", "Item_id", c => c.String(maxLength: 200));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
