namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RedoLastForeign : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_receive_po_items", "Table_vat_id");
            CreateIndex("dbo.Inv_receive_po_items", "Vat_id");
            CreateIndex("dbo.Inv_receive_po_items", "Deduct_id");
            AddForeignKey("dbo.Inv_receive_po_items", "Deduct_id", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.Inv_receive_po_items", "Table_vat_id", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.Inv_receive_po_items", "Vat_id", "dbo.C_TaxSetting_table", "CT_ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po_items", "Vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po_items", "Table_vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po_items", "Deduct_id", "dbo.C_TaxSetting_table");
            DropIndex("dbo.Inv_receive_po_items", new[] { "Deduct_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Vat_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Table_vat_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
