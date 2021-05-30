namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvGroupForien : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_item_group", "Tax_table_type_TG_ID", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_item_group", "Tbl_vat_TG_ID", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_item_group", "Vat_TG_ID", "dbo.TaxGroup_table");
            DropIndex("dbo.Inv_item_group", new[] { "Tax_table_type_TG_ID" });
            DropIndex("dbo.Inv_item_group", new[] { "Tbl_vat_TG_ID" });
            DropIndex("dbo.Inv_item_group", new[] { "Vat_TG_ID" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropColumn("dbo.Inv_item_group", "Tax_table_type_id");
            DropColumn("dbo.Inv_item_group", "Tbl_vat_Id");
            DropColumn("dbo.Inv_item_group", "Vat_id");
            RenameColumn(table: "dbo.Inv_item_group", name: "Tax_table_type_TG_ID", newName: "Tax_table_type_id");
            RenameColumn(table: "dbo.Inv_item_group", name: "Tbl_vat_TG_ID", newName: "Tbl_vat_Id");
            RenameColumn(table: "dbo.Inv_item_group", name: "Vat_TG_ID", newName: "Vat_id");
            AlterColumn("dbo.Inv_item_group", "Tax_table_type_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_item_group", "Tbl_vat_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_item_group", "Vat_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_item_group", "Tax_table_type_id");
            CreateIndex("dbo.Inv_item_group", "Tbl_vat_Id");
            CreateIndex("dbo.Inv_item_group", "Vat_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            AddForeignKey("dbo.Inv_item_group", "Tax_table_type_id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: false);
            AddForeignKey("dbo.Inv_item_group", "Tbl_vat_Id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: false);
            AddForeignKey("dbo.Inv_item_group", "Vat_id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_group", "Vat_id", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_item_group", "Tbl_vat_Id", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_item_group", "Tax_table_type_id", "dbo.TaxGroup_table");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_item_group", new[] { "Vat_id" });
            DropIndex("dbo.Inv_item_group", new[] { "Tbl_vat_Id" });
            DropIndex("dbo.Inv_item_group", new[] { "Tax_table_type_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_item_group", "Vat_id", c => c.Int());
            AlterColumn("dbo.Inv_item_group", "Tbl_vat_Id", c => c.Int());
            AlterColumn("dbo.Inv_item_group", "Tax_table_type_id", c => c.Int());
            RenameColumn(table: "dbo.Inv_item_group", name: "Vat_id", newName: "Vat_TG_ID");
            RenameColumn(table: "dbo.Inv_item_group", name: "Tbl_vat_Id", newName: "Tbl_vat_TG_ID");
            RenameColumn(table: "dbo.Inv_item_group", name: "Tax_table_type_id", newName: "Tax_table_type_TG_ID");
            AddColumn("dbo.Inv_item_group", "Vat_id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_item_group", "Tbl_vat_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_item_group", "Tax_table_type_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_item_group", "Vat_TG_ID");
            CreateIndex("dbo.Inv_item_group", "Tbl_vat_TG_ID");
            CreateIndex("dbo.Inv_item_group", "Tax_table_type_TG_ID");
            AddForeignKey("dbo.Inv_item_group", "Vat_TG_ID", "dbo.TaxGroup_table", "TG_ID");
            AddForeignKey("dbo.Inv_item_group", "Tbl_vat_TG_ID", "dbo.TaxGroup_table", "TG_ID");
            AddForeignKey("dbo.Inv_item_group", "Tax_table_type_TG_ID", "dbo.TaxGroup_table", "TG_ID");
        }
    }
}
