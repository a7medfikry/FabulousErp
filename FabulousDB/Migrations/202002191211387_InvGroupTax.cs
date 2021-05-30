namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvGroupTax : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_item_group", "Sales_tax_group_id", "dbo.TaxGroup_table");
            DropIndex("dbo.Inv_item_group", new[] { "Sales_tax_group_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            RenameColumn(table: "dbo.Inv_item_group", name: "Purchase_tax_group_id", newName: "Tax_table_type_TG_ID");
            RenameColumn(table: "dbo.Inv_item_group", name: "Sales_tax_group_id", newName: "Tax_type_id");
            RenameIndex(table: "dbo.Inv_item_group", name: "IX_Purchase_tax_group_id", newName: "IX_Tax_table_type_TG_ID");
            CreateTable(
                "dbo.Inv_item_deduct_tax",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deduct_id = c.Int(nullable: false),
                        item_group_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaxGroup_table", t => t.Deduct_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item_group", t => t.item_group_id, cascadeDelete: false)
                .Index(t => t.Deduct_id)
                .Index(t => t.item_group_id);
            
            AddColumn("dbo.Inv_item_group", "Item_type", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_item_group", "Tax_table_type_id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_item_group", "Tbl_vat_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_item_group", "Vat_id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_item_group", "Tbl_vat_TG_ID", c => c.Int());
            AddColumn("dbo.Inv_item_group", "Vat_TG_ID", c => c.Int());
            AlterColumn("dbo.Inv_item_group", "Tax_type_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_item_group", "Tax_type_id");
            CreateIndex("dbo.Inv_item_group", "Tbl_vat_TG_ID");
            CreateIndex("dbo.Inv_item_group", "Vat_TG_ID");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            AddForeignKey("dbo.Inv_item_group", "Tbl_vat_TG_ID", "dbo.TaxGroup_table", "TG_ID");
            AddForeignKey("dbo.Inv_item_group", "Vat_TG_ID", "dbo.TaxGroup_table", "TG_ID");
            AddForeignKey("dbo.Inv_item_group", "Tax_type_id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_group", "Tax_type_id", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_item_group", "Vat_TG_ID", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_item_group", "Tbl_vat_TG_ID", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_item_deduct_tax", "item_group_id", "dbo.Inv_item_group");
            DropForeignKey("dbo.Inv_item_deduct_tax", "Deduct_id", "dbo.TaxGroup_table");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_item_deduct_tax", new[] { "item_group_id" });
            DropIndex("dbo.Inv_item_deduct_tax", new[] { "Deduct_id" });
            DropIndex("dbo.Inv_item_group", new[] { "Vat_TG_ID" });
            DropIndex("dbo.Inv_item_group", new[] { "Tbl_vat_TG_ID" });
            DropIndex("dbo.Inv_item_group", new[] { "Tax_type_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_item_group", "Tax_type_id", c => c.Int());
            DropColumn("dbo.Inv_item_group", "Vat_TG_ID");
            DropColumn("dbo.Inv_item_group", "Tbl_vat_TG_ID");
            DropColumn("dbo.Inv_item_group", "Vat_id");
            DropColumn("dbo.Inv_item_group", "Tbl_vat_Id");
            DropColumn("dbo.Inv_item_group", "Tax_table_type_id");
            DropColumn("dbo.Inv_item_group", "Item_type");
            DropTable("dbo.Inv_item_deduct_tax");
            RenameIndex(table: "dbo.Inv_item_group", name: "IX_Tax_table_type_TG_ID", newName: "IX_Purchase_tax_group_id");
            RenameColumn(table: "dbo.Inv_item_group", name: "Tax_type_id", newName: "Sales_tax_group_id");
            RenameColumn(table: "dbo.Inv_item_group", name: "Tax_table_type_TG_ID", newName: "Purchase_tax_group_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_item_group", "Sales_tax_group_id");
            AddForeignKey("dbo.Inv_item_group", "Sales_tax_group_id", "dbo.TaxGroup_table", "TG_ID");
        }
    }
}
