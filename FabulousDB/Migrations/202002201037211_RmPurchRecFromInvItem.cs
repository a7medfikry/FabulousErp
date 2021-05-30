namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RmPurchRecFromInvItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_item", "Purchase_tax_group_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_item", "Sales_tax_group_id", "dbo.C_TaxSetting_table");
           
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_item", "Purchase_tax_group_id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_item", "Sales_tax_group_id", c => c.Int(nullable: false));
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_item", "Purchase_tax_group_id");
            CreateIndex("dbo.Inv_item", "Sales_tax_group_id");
            AddForeignKey("dbo.Inv_item", "Sales_tax_group_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item", "Purchase_tax_group_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
        }
    }
}
