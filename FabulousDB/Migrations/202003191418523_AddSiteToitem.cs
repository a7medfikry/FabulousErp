namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSiteToitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po_items", "Site_id", c => c.Int());
            CreateIndex("dbo.Inv_receive_po_items", "Site_id");
            AddForeignKey("dbo.Inv_receive_po_items", "Site_id", "dbo.Inv_store_site", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po_items", "Site_id", "dbo.Inv_store_site");
            DropIndex("dbo.Inv_receive_po_items", new[] { "Site_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_receive_po_items", "Site_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
