namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay39 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Creditor_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table");
            DropForeignKey("dbo.Creditor_setting", "Payment_term_id", "dbo.Payment_term");
            DropForeignKey("dbo.Creditor_setting", "Shipping_method_id", "dbo.Shipping_method");
            DropForeignKey("dbo.Creditor_setting", "Tax_group_id", "dbo.TaxGroup_table");
            DropIndex("dbo.Creditor_setting", new[] { "Payment_term_id" });
            DropIndex("dbo.Creditor_setting", new[] { "Def_Checkbook" });
            DropIndex("dbo.Creditor_setting", new[] { "Shipping_method_id" });
            DropIndex("dbo.Creditor_setting", new[] { "Tax_group_id" });
            AlterColumn("dbo.Creditor_setting", "Vendor_id", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Creditor_setting", "Payment_term_id", c => c.Int());
            AlterColumn("dbo.Creditor_setting", "Def_Checkbook", c => c.Int());
            AlterColumn("dbo.Creditor_setting", "Minimum_order", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Creditor_setting", "Maximum_order", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Creditor_setting", "Shipping_method_id", c => c.Int());
            AlterColumn("dbo.Creditor_setting", "Tax_group_id", c => c.Int());
            AlterColumn("dbo.Creditor_setting", "Inactive", c => c.Boolean());
            AlterColumn("dbo.Creditor_setting", "Customer", c => c.Boolean());
            AlterColumn("dbo.Creditor_setting", "Revaluate", c => c.Boolean());
            CreateIndex("dbo.Creditor_setting", "Payment_term_id");
            CreateIndex("dbo.Creditor_setting", "Def_Checkbook");
            CreateIndex("dbo.Creditor_setting", "Shipping_method_id");
            CreateIndex("dbo.Creditor_setting", "Tax_group_id");
            AddForeignKey("dbo.Creditor_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table", "C_CBSID");
            AddForeignKey("dbo.Creditor_setting", "Payment_term_id", "dbo.Payment_term", "Id");
            AddForeignKey("dbo.Creditor_setting", "Shipping_method_id", "dbo.Shipping_method", "Id");
            AddForeignKey("dbo.Creditor_setting", "Tax_group_id", "dbo.TaxGroup_table", "TG_ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Creditor_setting", "Tax_group_id", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Creditor_setting", "Shipping_method_id", "dbo.Shipping_method");
            DropForeignKey("dbo.Creditor_setting", "Payment_term_id", "dbo.Payment_term");
            DropForeignKey("dbo.Creditor_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table");
            DropIndex("dbo.Creditor_setting", new[] { "Tax_group_id" });
            DropIndex("dbo.Creditor_setting", new[] { "Shipping_method_id" });
            DropIndex("dbo.Creditor_setting", new[] { "Def_Checkbook" });
            DropIndex("dbo.Creditor_setting", new[] { "Payment_term_id" });
            AlterColumn("dbo.Creditor_setting", "Revaluate", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Creditor_setting", "Customer", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Creditor_setting", "Inactive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Creditor_setting", "Tax_group_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Creditor_setting", "Shipping_method_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Creditor_setting", "Maximum_order", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Creditor_setting", "Minimum_order", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Creditor_setting", "Def_Checkbook", c => c.Int(nullable: false));
            AlterColumn("dbo.Creditor_setting", "Payment_term_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Creditor_setting", "Vendor_id", c => c.String(maxLength: 100));
            CreateIndex("dbo.Creditor_setting", "Tax_group_id");
            CreateIndex("dbo.Creditor_setting", "Shipping_method_id");
            CreateIndex("dbo.Creditor_setting", "Def_Checkbook");
            CreateIndex("dbo.Creditor_setting", "Payment_term_id");
            AddForeignKey("dbo.Creditor_setting", "Tax_group_id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: true);
            AddForeignKey("dbo.Creditor_setting", "Shipping_method_id", "dbo.Shipping_method", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Creditor_setting", "Payment_term_id", "dbo.Payment_term", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Creditor_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table", "C_CBSID", cascadeDelete: true);
        }
    }
}
