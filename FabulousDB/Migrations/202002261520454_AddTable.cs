namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_sales_invoice_items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_id = c.Int(nullable: false),
                        UOM_id = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Unit_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount_system_currency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Net_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Table_vat_id = c.Int(),
                        Table_vat_amount = c.Decimal(precision: 18, scale: 2),
                        Vat_id = c.Int(),
                        Vat_amount = c.Decimal(precision: 18, scale: 2),
                        Deduct_id = c.Int(),
                        Deduct_amount = c.Decimal(precision: 18, scale: 2),
                        Sales_invoice_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Deduct_id)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_sales_invoice", t => t.Sales_invoice_id)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Table_vat_id)
                .ForeignKey("dbo.Unit_of_measure", t => t.UOM_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Vat_id)
                .Index(t => t.Item_id)
                .Index(t => t.UOM_id)
                .Index(t => t.Table_vat_id)
                .Index(t => t.Vat_id)
                .Index(t => t.Deduct_id)
                .Index(t => t.Sales_invoice_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_invoice_items", "Vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_sales_invoice_items", "UOM_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_sales_invoice_items", "Table_vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_sales_invoice_items", "Sales_invoice_id", "dbo.Inv_sales_invoice");
            DropForeignKey("dbo.Inv_sales_invoice_items", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_sales_invoice_items", "Deduct_id", "dbo.C_TaxSetting_table");
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "Sales_invoice_id" });
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "Deduct_id" });
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "Vat_id" });
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "Table_vat_id" });
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "UOM_id" });
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "Item_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropTable("dbo.Inv_sales_invoice_items");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
