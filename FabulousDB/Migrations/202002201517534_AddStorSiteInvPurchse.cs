namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStorSiteInvPurchse : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AddColumn("dbo.Inv_purchase_request", "Store_id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_purchase_request", "Site_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_purchase_request", "Store_id");
            CreateIndex("dbo.Inv_purchase_request", "Site_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            AddForeignKey("dbo.Inv_purchase_request", "Site_id", "dbo.Inv_store_site", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Inv_purchase_request", "Store_id", "dbo.Inv_store", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
     
        }
    }
}
