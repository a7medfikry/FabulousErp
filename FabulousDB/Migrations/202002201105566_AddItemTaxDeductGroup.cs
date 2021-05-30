namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItemTaxDeductGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Inv_item_deduct_tax", "FK_dbo.Inv_item_deduct_tax_dbo.Inv_item_group_item_group_id");
            DropIndex("dbo.Inv_item_deduct_tax", new[] { "item_group_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            CreateTable(
                "dbo.Inv_item_group_deduct_tax",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deduct_id = c.Int(nullable: false),
                        item_group_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Deduct_id, cascadeDelete: true)
                .Index(t => t.Deduct_id)
                .Index(t => t.item_group_id);
            
            AddColumn("dbo.Inv_item", "Tax_type_id", c => c.Int());
            AddColumn("dbo.Inv_item", "Tax_table_type_id", c => c.Int());
            AddColumn("dbo.Inv_item", "Tbl_vat_Id", c => c.Int());
            AddColumn("dbo.Inv_item", "Vat_id", c => c.Int());
            AddColumn("dbo.Inv_item_deduct_tax", "item_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_item", "Tax_type_id");
            CreateIndex("dbo.Inv_item", "Tax_table_type_id");
            CreateIndex("dbo.Inv_item", "Tbl_vat_Id");
            CreateIndex("dbo.Inv_item", "Vat_id");
            CreateIndex("dbo.Inv_item_deduct_tax", "item_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            AddForeignKey("dbo.Inv_item_deduct_tax", "item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item", "Tax_table_type_id", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.Inv_item", "Tax_type_id", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.Inv_item", "Tbl_vat_Id", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.Inv_item", "Vat_id", "dbo.C_TaxSetting_table", "CT_ID");
            DropColumn("dbo.Inv_item_deduct_tax", "item_group_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_item_deduct_tax", "item_group_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Inv_item", "Vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_item", "Tbl_vat_Id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_item", "Tax_type_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_item", "Tax_table_type_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_item_group_deduct_tax", "Deduct_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_item_deduct_tax", "item_id", "dbo.Inv_item");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_item_group_deduct_tax", new[] { "item_group_id" });
            DropIndex("dbo.Inv_item_group_deduct_tax", new[] { "Deduct_id" });
            DropIndex("dbo.Inv_item_deduct_tax", new[] { "item_id" });
            DropIndex("dbo.Inv_item", new[] { "Vat_id" });
            DropIndex("dbo.Inv_item", new[] { "Tbl_vat_Id" });
            DropIndex("dbo.Inv_item", new[] { "Tax_table_type_id" });
            DropIndex("dbo.Inv_item", new[] { "Tax_type_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_item_deduct_tax", "item_id");
            DropColumn("dbo.Inv_item", "Vat_id");
            DropColumn("dbo.Inv_item", "Tbl_vat_Id");
            DropColumn("dbo.Inv_item", "Tax_table_type_id");
            DropColumn("dbo.Inv_item", "Tax_type_id");
            DropTable("dbo.Inv_item_group_deduct_tax");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_item_deduct_tax", "item_group_id");
        }
    }
}
