namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay40 : DbMigration
    {
        public override void Up()
        {
            Sql("delete from Payable_gl_account");
            DropForeignKey("dbo.Payable_gl_account", "Account_payable_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Accrued_purchase_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Creditor_setting_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Payable_gl_account", "Payable_Group_setting_id", "dbo.Payable_Group_setting");
            DropForeignKey("dbo.Payable_gl_account", "Purchase_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Purchase_variance_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Returne_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Taken_discount_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Payable_gl_account", new[] { "Account_payable_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Purchase_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Taken_discount_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Purchase_variance_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Accrued_purchase_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Returne_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Payable_Group_setting_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Creditor_setting_id" });
            AlterColumn("dbo.Payable_gl_account", "Account_payable_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_gl_account", "Purchase_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_gl_account", "Taken_discount_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_gl_account", "Purchase_variance_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_gl_account", "Accrued_purchase_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_gl_account", "Returne_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_gl_account", "Payable_Group_setting_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_gl_account", "Creditor_setting_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_gl_account", "Account_payable_id");
            CreateIndex("dbo.Payable_gl_account", "Purchase_id");
            CreateIndex("dbo.Payable_gl_account", "Taken_discount_id");
            CreateIndex("dbo.Payable_gl_account", "Purchase_variance_id");
            CreateIndex("dbo.Payable_gl_account", "Accrued_purchase_id");
            CreateIndex("dbo.Payable_gl_account", "Returne_id");
            CreateIndex("dbo.Payable_gl_account", "Payable_Group_setting_id");
            CreateIndex("dbo.Payable_gl_account", "Creditor_setting_id");
            AddForeignKey("dbo.Payable_gl_account", "Account_payable_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: false);
            AddForeignKey("dbo.Payable_gl_account", "Accrued_purchase_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: false);
            AddForeignKey("dbo.Payable_gl_account", "Creditor_setting_id", "dbo.Creditor_setting", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Payable_gl_account", "Payable_Group_setting_id", "dbo.Payable_Group_setting", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Payable_gl_account", "Purchase_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: false);
            AddForeignKey("dbo.Payable_gl_account", "Purchase_variance_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: false);
            AddForeignKey("dbo.Payable_gl_account", "Returne_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: false);
            AddForeignKey("dbo.Payable_gl_account", "Taken_discount_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_gl_account", "Taken_discount_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Returne_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Purchase_variance_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Purchase_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Payable_Group_setting_id", "dbo.Payable_Group_setting");
            DropForeignKey("dbo.Payable_gl_account", "Creditor_setting_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Payable_gl_account", "Accrued_purchase_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Account_payable_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Payable_gl_account", new[] { "Creditor_setting_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Payable_Group_setting_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Returne_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Accrued_purchase_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Purchase_variance_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Taken_discount_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Purchase_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Account_payable_id" });
            AlterColumn("dbo.Payable_gl_account", "Creditor_setting_id", c => c.Int());
            AlterColumn("dbo.Payable_gl_account", "Payable_Group_setting_id", c => c.Int());
            AlterColumn("dbo.Payable_gl_account", "Returne_id", c => c.Int());
            AlterColumn("dbo.Payable_gl_account", "Accrued_purchase_id", c => c.Int());
            AlterColumn("dbo.Payable_gl_account", "Purchase_variance_id", c => c.Int());
            AlterColumn("dbo.Payable_gl_account", "Taken_discount_id", c => c.Int());
            AlterColumn("dbo.Payable_gl_account", "Purchase_id", c => c.Int());
            AlterColumn("dbo.Payable_gl_account", "Account_payable_id", c => c.Int());
            CreateIndex("dbo.Payable_gl_account", "Creditor_setting_id");
            CreateIndex("dbo.Payable_gl_account", "Payable_Group_setting_id");
            CreateIndex("dbo.Payable_gl_account", "Returne_id");
            CreateIndex("dbo.Payable_gl_account", "Accrued_purchase_id");
            CreateIndex("dbo.Payable_gl_account", "Purchase_variance_id");
            CreateIndex("dbo.Payable_gl_account", "Taken_discount_id");
            CreateIndex("dbo.Payable_gl_account", "Purchase_id");
            CreateIndex("dbo.Payable_gl_account", "Account_payable_id");
            AddForeignKey("dbo.Payable_gl_account", "Taken_discount_id", "dbo.C_CreateAccount_Table", "C_AID");
            AddForeignKey("dbo.Payable_gl_account", "Returne_id", "dbo.C_CreateAccount_Table", "C_AID");
            AddForeignKey("dbo.Payable_gl_account", "Purchase_variance_id", "dbo.C_CreateAccount_Table", "C_AID");
            AddForeignKey("dbo.Payable_gl_account", "Purchase_id", "dbo.C_CreateAccount_Table", "C_AID");
            AddForeignKey("dbo.Payable_gl_account", "Payable_Group_setting_id", "dbo.Payable_Group_setting", "Id");
            AddForeignKey("dbo.Payable_gl_account", "Creditor_setting_id", "dbo.Creditor_setting", "Id");
            AddForeignKey("dbo.Payable_gl_account", "Accrued_purchase_id", "dbo.C_CreateAccount_Table", "C_AID");
            AddForeignKey("dbo.Payable_gl_account", "Account_payable_id", "dbo.C_CreateAccount_Table", "C_AID");
        }
    }
}
