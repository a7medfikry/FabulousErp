namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePSInvItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Inv_item", "FK_dbo.Inv_item_dbo.TaxGroup_table_Purchase_tax_group_id");
            DropForeignKey("Inv_item", "FK_dbo.Inv_item_dbo.TaxGroup_table_Sales_tax_group_id");
            DropIndex("dbo.Inv_item", new[] { "Sales_tax_group_id" });
            DropIndex("dbo.Inv_item", new[] { "Purchase_tax_group_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            DropColumn("dbo.Inv_item", "Sales_tax_group_id");
            DropColumn("dbo.Inv_item", "Purchase_tax_group_id");
        }
        
        public override void Down()
        {
        }
    }
}
