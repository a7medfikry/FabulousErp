namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Gl_account", name: "Account_payable_C_AID", newName: "Account_payable_id");
            RenameColumn(table: "dbo.Gl_account", name: "Accrued_purchase_C_AID", newName: "Accrued_purchase_id");
            RenameColumn(table: "dbo.Gl_account", name: "Purchase_C_AID", newName: "Purchase_id");
            RenameColumn(table: "dbo.Gl_account", name: "Purchase_variance_C_AID", newName: "Purchase_variance_id");
            RenameColumn(table: "dbo.Gl_account", name: "Returne_C_AID", newName: "Returne_id");
            RenameColumn(table: "dbo.Gl_account", name: "Taken_discount_C_AID", newName: "Taken_discount_id");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Account_payable_C_AID", newName: "IX_Account_payable_id");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Purchase_C_AID", newName: "IX_Purchase_id");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Taken_discount_C_AID", newName: "IX_Taken_discount_id");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Purchase_variance_C_AID", newName: "IX_Purchase_variance_id");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Accrued_purchase_C_AID", newName: "IX_Accrued_purchase_id");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Returne_C_AID", newName: "IX_Returne_id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Gl_account", name: "IX_Returne_id", newName: "IX_Returne_C_AID");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Accrued_purchase_id", newName: "IX_Accrued_purchase_C_AID");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Purchase_variance_id", newName: "IX_Purchase_variance_C_AID");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Taken_discount_id", newName: "IX_Taken_discount_C_AID");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Purchase_id", newName: "IX_Purchase_C_AID");
            RenameIndex(table: "dbo.Gl_account", name: "IX_Account_payable_id", newName: "IX_Account_payable_C_AID");
            RenameColumn(table: "dbo.Gl_account", name: "Taken_discount_id", newName: "Taken_discount_C_AID");
            RenameColumn(table: "dbo.Gl_account", name: "Returne_id", newName: "Returne_C_AID");
            RenameColumn(table: "dbo.Gl_account", name: "Purchase_variance_id", newName: "Purchase_variance_C_AID");
            RenameColumn(table: "dbo.Gl_account", name: "Purchase_id", newName: "Purchase_C_AID");
            RenameColumn(table: "dbo.Gl_account", name: "Accrued_purchase_id", newName: "Accrued_purchase_C_AID");
            RenameColumn(table: "dbo.Gl_account", name: "Account_payable_id", newName: "Account_payable_C_AID");
        }
    }
}
