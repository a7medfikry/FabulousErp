namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay37 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payable_Group_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table");
            DropForeignKey("dbo.Payable_Group_setting", "Payment_terms", "dbo.Payment_term");
            DropForeignKey("dbo.Payable_Group_setting", "Shipping_method_id", "dbo.Shipping_method");
            DropForeignKey("dbo.Payable_Group_setting", "Tax_group_id", "dbo.TaxGroup_table");
            DropIndex("dbo.Payable_Group_setting", new[] { "Payment_terms" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Def_Checkbook" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Shipping_method_id" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Tax_group_id" });
            AlterColumn("dbo.Payable_Group_setting", "Payment_terms", c => c.Int());
            AlterColumn("dbo.Payable_Group_setting", "Def_Checkbook", c => c.Int());
            AlterColumn("dbo.Payable_Group_setting", "Shipping_method_id", c => c.Int());
            AlterColumn("dbo.Payable_Group_setting", "Tax_group_id", c => c.Int());
            CreateIndex("dbo.Payable_Group_setting", "Payment_terms");
            CreateIndex("dbo.Payable_Group_setting", "Def_Checkbook");
            CreateIndex("dbo.Payable_Group_setting", "Shipping_method_id");
            CreateIndex("dbo.Payable_Group_setting", "Tax_group_id");
            AddForeignKey("dbo.Payable_Group_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table", "C_CBSID");
            AddForeignKey("dbo.Payable_Group_setting", "Payment_terms", "dbo.Payment_term", "Id");
            AddForeignKey("dbo.Payable_Group_setting", "Shipping_method_id", "dbo.Shipping_method", "Id");
            AddForeignKey("dbo.Payable_Group_setting", "Tax_group_id", "dbo.TaxGroup_table", "TG_ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_Group_setting", "Tax_group_id", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Payable_Group_setting", "Shipping_method_id", "dbo.Shipping_method");
            DropForeignKey("dbo.Payable_Group_setting", "Payment_terms", "dbo.Payment_term");
            DropForeignKey("dbo.Payable_Group_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table");
            DropIndex("dbo.Payable_Group_setting", new[] { "Tax_group_id" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Shipping_method_id" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Def_Checkbook" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Payment_terms" });
            AlterColumn("dbo.Payable_Group_setting", "Tax_group_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_Group_setting", "Shipping_method_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_Group_setting", "Def_Checkbook", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_Group_setting", "Payment_terms", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_Group_setting", "Tax_group_id");
            CreateIndex("dbo.Payable_Group_setting", "Shipping_method_id");
            CreateIndex("dbo.Payable_Group_setting", "Def_Checkbook");
            CreateIndex("dbo.Payable_Group_setting", "Payment_terms");
            AddForeignKey("dbo.Payable_Group_setting", "Tax_group_id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: true);
            AddForeignKey("dbo.Payable_Group_setting", "Shipping_method_id", "dbo.Shipping_method", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Payable_Group_setting", "Payment_terms", "dbo.Payment_term", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Payable_Group_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table", "C_CBSID", cascadeDelete: true);
        }
    }
}
