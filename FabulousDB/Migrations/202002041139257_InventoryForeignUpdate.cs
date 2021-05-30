namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoryForeignUpdate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AddColumn("dbo.Inv_item", "Valudation_method", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_item_group", "Valudation_method", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");

            DropForeignKey("Inv_quotation_receiving", "Pr_no_id", "Inv_purchase");
            DropForeignKey("Inv_quotation_request", "Pr_no_id", "Inv_purchase");

            AddForeignKey("Inv_quotation_receiving", "Pr_no_id", "Inv_purchase_request", "Id");
            AddForeignKey("Inv_quotation_request", "Pr_no_id", "Inv_purchase_request", "Id");
        }
        public override void Down()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_item_group", "Valudation_method");
            DropColumn("dbo.Inv_item", "Valudation_method");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
