namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletInvAndGroupForeign : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_item_group", "Tax_table_type_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_item_group", "Tax_type_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_item", "Tax_table_type_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_item", "Tax_type_id", "dbo.C_TaxSetting_table");
            DropIndex("dbo.Inv_item", new[] { "Tax_type_id" });
            DropIndex("dbo.Inv_item", new[] { "Tax_table_type_id" });
            DropIndex("dbo.Inv_item_group", new[] { "Tax_type_id" });
            DropIndex("dbo.Inv_item_group", new[] { "Tax_table_type_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_item_group", "Tax_table_type_id");
            CreateIndex("dbo.Inv_item_group", "Tax_type_id");
            CreateIndex("dbo.Inv_item", "Tax_table_type_id");
            CreateIndex("dbo.Inv_item", "Tax_type_id");
            AddForeignKey("dbo.Inv_item", "Tax_type_id", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.Inv_item", "Tax_table_type_id", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.Inv_item_group", "Tax_type_id", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.Inv_item_group", "Tax_table_type_id", "dbo.C_TaxSetting_table", "CT_ID");
        }
    }
}
