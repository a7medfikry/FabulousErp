namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invetory_po : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            CreateTable(
                "dbo.Inv_po",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Po_type = c.Int(nullable: false),
                        Pr_no_id = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Vendore_id = c.Int(nullable: false),
                        Currency_id = c.String(maxLength: 50),
                        System_rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Transaction_rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Difference = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Inv_purchase_request", t => t.Pr_no_id, cascadeDelete: false)
                .ForeignKey("dbo.Payable_creditor_setting", t => t.Vendore_id, cascadeDelete: false)
                .Index(t => t.Pr_no_id)
                .Index(t => t.Vendore_id)
                .Index(t => t.Currency_id);
            
            CreateTable(
                "dbo.Inv_po_items",
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
                        Table_vat_id = c.Int(nullable: false),
                        Table_vat_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Vat_id = c.Int(nullable: false),
                        Vat_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deduct_id = c.Int(nullable: false),
                        Deduct_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Po_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Deduct_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_po", t => t.Po_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Table_vat_id, cascadeDelete: false)
                .ForeignKey("dbo.Unit_of_measure", t => t.UOM_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Vat_id, cascadeDelete: false)
                .Index(t => t.Item_id)
                .Index(t => t.UOM_id)
                .Index(t => t.Table_vat_id)
                .Index(t => t.Vat_id)
                .Index(t => t.Deduct_id)
                .Index(t => t.Po_id);
            
            CreateTable(
                "dbo.Inv_receive_po",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GR_no = c.Int(nullable: false),
                        Doc_type = c.Int(nullable: false),
                        PO_id = c.Int(nullable: false),
                        Store_id = c.Int(nullable: false),
                        Site_id = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Vendore_id = c.Int(nullable: false),
                        Batch_id = c.Int(nullable: false),
                        Buyer = c.String(),
                        Vendore_inv_number = c.String(maxLength: 200),
                        Currency_id = c.String(maxLength: 50),
                        Inv_receive_po_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Inv_receive_po", t => t.Inv_receive_po_Id)
                .ForeignKey("dbo.Inv_po", t => t.PO_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store_site", t => t.Site_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store", t => t.Store_id, cascadeDelete: false)
                .ForeignKey("dbo.Payable_creditor_setting", t => t.Vendore_id, cascadeDelete: false)
                .Index(t => t.PO_id)
                .Index(t => t.Store_id)
                .Index(t => t.Site_id)
                .Index(t => t.Vendore_id)
                .Index(t => t.Currency_id)
                .Index(t => t.Inv_receive_po_Id);
            
            CreateTable(
                "dbo.Inv_receive_po_items",
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
                        Table_vat_id = c.Int(nullable: false),
                        Table_vat_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Vat_id = c.Int(nullable: false),
                        Vat_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deduct_id = c.Int(nullable: false),
                        Deduct_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Receive_po_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Deduct_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_receive_po", t => t.Receive_po_id)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Table_vat_id, cascadeDelete: false)
                .ForeignKey("dbo.Unit_of_measure", t => t.UOM_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Vat_id, cascadeDelete: false)
                .Index(t => t.Item_id)
                .Index(t => t.UOM_id)
                .Index(t => t.Table_vat_id)
                .Index(t => t.Vat_id)
                .Index(t => t.Deduct_id)
                .Index(t => t.Receive_po_id);
            
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po_items", "Vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po_items", "UOM_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_receive_po_items", "Table_vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po_items", "receive_po_Id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_receive_po_items", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_receive_po_items", "Deduct_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po", "Vendore_id", "dbo.Payable_creditor_setting");
            DropForeignKey("dbo.Inv_receive_po", "Store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_receive_po", "Site_id", "dbo.Inv_store_site");
            DropForeignKey("dbo.Inv_receive_po", "PO_id", "dbo.Inv_po");
            DropForeignKey("dbo.Inv_receive_po", "Inv_receive_po_Id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_receive_po", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropForeignKey("dbo.Inv_po", "Vendore_id", "dbo.Payable_creditor_setting");
            DropForeignKey("dbo.Inv_po", "Pr_no_id", "dbo.Inv_purchase_request");
            DropForeignKey("dbo.Inv_po_items", "Vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_po_items", "UOM_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_po_items", "Table_vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_po_items", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_po_items", "Po_id", "dbo.Inv_po");
            DropForeignKey("dbo.Inv_po_items", "Deduct_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_po", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropIndex("dbo.Inv_receive_po_items", new[] { "receive_po_Id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Deduct_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Vat_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Table_vat_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "UOM_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Item_id" });
            DropIndex("dbo.Inv_receive_po", new[] { "Inv_receive_po_Id" });
            DropIndex("dbo.Inv_receive_po", new[] { "Currency_id" });
            DropIndex("dbo.Inv_receive_po", new[] { "Vendore_id" });
            DropIndex("dbo.Inv_receive_po", new[] { "Site_id" });
            DropIndex("dbo.Inv_receive_po", new[] { "Store_id" });
            DropIndex("dbo.Inv_receive_po", new[] { "PO_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_po_items", new[] { "Po_id" });
            DropIndex("dbo.Inv_po_items", new[] { "Deduct_id" });
            DropIndex("dbo.Inv_po_items", new[] { "Vat_id" });
            DropIndex("dbo.Inv_po_items", new[] { "Table_vat_id" });
            DropIndex("dbo.Inv_po_items", new[] { "UOM_id" });
            DropIndex("dbo.Inv_po_items", new[] { "Item_id" });
            DropIndex("dbo.Inv_po", new[] { "Currency_id" });
            DropIndex("dbo.Inv_po", new[] { "Vendore_id" });
            DropIndex("dbo.Inv_po", new[] { "Pr_no_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropTable("dbo.Inv_receive_po_items");
            DropTable("dbo.Inv_receive_po");
            DropTable("dbo.Inv_po_items");
            DropTable("dbo.Inv_po");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
