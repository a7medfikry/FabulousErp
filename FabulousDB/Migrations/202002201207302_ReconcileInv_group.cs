namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReconcileInv_group : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Inv_item_group", "FK_dbo.Inv_item_group_dbo.TaxGroup_table_Purchase_tax_group_id");
            DropForeignKey("Inv_item_group", "FK_dbo.Inv_item_group_dbo.TaxGroup_table_Tax_table_type_id");
            DropForeignKey("Inv_item_group", "FK_dbo.Inv_item_group_dbo.TaxGroup_table_Tax_type_id");
            DropForeignKey("Inv_item_group", "FK_dbo.Inv_item_group_dbo.TaxGroup_table_Tbl_vat_Id");
            DropForeignKey("Inv_item_group", "FK_dbo.Inv_item_group_dbo.TaxGroup_table_Vat_id");

            AddForeignKey("Inv_item_group", "Vat_id", "C_TaxSetting_table", "CT_ID");
            AddForeignKey("Inv_item_group", "Tbl_vat_Id", "C_TaxSetting_table", "CT_ID");
            AddForeignKey("Inv_item_group", "Tax_table_type_id", "C_TaxSetting_table", "CT_ID");
            AddForeignKey("Inv_item_group", "Tax_type_id", "C_TaxSetting_table", "CT_ID");
           
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item", "Inv_item_group_Id", "dbo.Inv_item_group");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_item", new[] { "Inv_item_group_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_item", "Inv_item_group_Id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_item", "Item_group_id");
            AddForeignKey("dbo.Inv_item", "Item_group_id", "dbo.Inv_item_group", "Id");
        }
    }
}
