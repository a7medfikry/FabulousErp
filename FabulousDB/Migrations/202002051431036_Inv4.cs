namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inv4 : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("dbo.Inv_receive_po", t => t.Receive_po_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Table_vat_id, cascadeDelete: false)
                .ForeignKey("dbo.Unit_of_measure", t => t.UOM_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Vat_id, cascadeDelete: false)
                .Index(t => t.Item_id)
                .Index(t => t.UOM_id)
                .Index(t => t.Table_vat_id)
                .Index(t => t.Vat_id)
                .Index(t => t.Deduct_id)
                .Index(t => t.Receive_po_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po_items", "Vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po_items", "UOM_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_receive_po_items", "Table_vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Inv_receive_po_items", "Receive_po_id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_receive_po_items", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_receive_po_items", "Deduct_id", "dbo.C_TaxSetting_table");
            DropIndex("dbo.Inv_receive_po_items", new[] { "Receive_po_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Deduct_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Vat_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Table_vat_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "UOM_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "Item_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropTable("dbo.Inv_receive_po_items");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
