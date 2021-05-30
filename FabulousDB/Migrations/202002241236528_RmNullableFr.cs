namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RmNullableFr : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_receive_po_items", "Deduct_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po_items", "Receive_po_id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_receive_po_items", "Table_vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po_items", "Vat_id", "dbo.C_TaxSetting_table");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Table_vat_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Vat_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Deduct_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Receive_po_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po_items", "Table_vat_id", c => c.Int());
            AlterColumn("dbo.Inv_receive_po_items", "Table_vat_amount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Inv_receive_po_items", "Vat_id", c => c.Int());
            AlterColumn("dbo.Inv_receive_po_items", "Vat_amount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Inv_receive_po_items", "Deduct_id", c => c.Int());
            AlterColumn("dbo.Inv_receive_po_items", "Deduct_amount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Inv_receive_po_items", "Receive_po_id", c => c.Int());
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_receive_po_items", "Table_vat_id");
            CreateIndex("dbo.Inv_receive_po_items", "Vat_id");
            CreateIndex("dbo.Inv_receive_po_items", "Deduct_id");
            CreateIndex("dbo.Inv_receive_po_items", "Receive_po_id");
            AddForeignKey("dbo.Inv_receive_po_items", "Deduct_id", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.Inv_receive_po_items", "Receive_po_id", "dbo.Inv_receive_po", "Id");
            AddForeignKey("dbo.Inv_receive_po_items", "Table_vat_id", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.Inv_receive_po_items", "Vat_id", "dbo.C_TaxSetting_table", "CT_ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po_items", "Vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po_items", "Table_vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po_items", "Receive_po_id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_receive_po_items", "Deduct_id", "dbo.C_TaxSetting_table");
            DropIndex("dbo.Inv_receive_po_items", new[] { "Receive_po_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Deduct_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Vat_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Table_vat_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_receive_po_items", "Receive_po_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po_items", "Deduct_amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Inv_receive_po_items", "Deduct_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po_items", "Vat_amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Inv_receive_po_items", "Vat_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po_items", "Table_vat_amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Inv_receive_po_items", "Table_vat_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_receive_po_items", "Receive_po_id");
            CreateIndex("dbo.Inv_receive_po_items", "Deduct_id");
            CreateIndex("dbo.Inv_receive_po_items", "Vat_id");
            CreateIndex("dbo.Inv_receive_po_items", "Table_vat_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            AddForeignKey("dbo.Inv_receive_po_items", "Vat_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po_items", "Table_vat_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po_items", "Receive_po_id", "dbo.Inv_receive_po", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po_items", "Deduct_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
        }
    }
}
