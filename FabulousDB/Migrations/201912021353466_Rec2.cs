namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Receivable_gl_account", name: "Accrued_purchase_id", newName: "Accrued_sales_id");
            RenameColumn(table: "dbo.Receivable_gl_account", name: "Purchase_id", newName: "Discount_id");
            RenameColumn(table: "dbo.Receivable_gl_account", name: "Purchase_variance_id", newName: "Sales_id");
            RenameColumn(table: "dbo.Receivable_gl_account", name: "Taken_discount_id", newName: "Sales_variance_id");
            RenameIndex(table: "dbo.Receivable_gl_account", name: "IX_Purchase_variance_id", newName: "IX_Sales_id");
            RenameIndex(table: "dbo.Receivable_gl_account", name: "IX_Purchase_id", newName: "IX_Discount_id");
            RenameIndex(table: "dbo.Receivable_gl_account", name: "IX_Taken_discount_id", newName: "IX_Sales_variance_id");
            RenameIndex(table: "dbo.Receivable_gl_account", name: "IX_Accrued_purchase_id", newName: "IX_Accrued_sales_id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Receivable_gl_account", name: "IX_Accrued_sales_id", newName: "IX_Accrued_purchase_id");
            RenameIndex(table: "dbo.Receivable_gl_account", name: "IX_Sales_variance_id", newName: "IX_Taken_discount_id");
            RenameIndex(table: "dbo.Receivable_gl_account", name: "IX_Discount_id", newName: "IX_Purchase_id");
            RenameIndex(table: "dbo.Receivable_gl_account", name: "IX_Sales_id", newName: "IX_Purchase_variance_id");
            RenameColumn(table: "dbo.Receivable_gl_account", name: "Sales_variance_id", newName: "Taken_discount_id");
            RenameColumn(table: "dbo.Receivable_gl_account", name: "Sales_id", newName: "Purchase_variance_id");
            RenameColumn(table: "dbo.Receivable_gl_account", name: "Discount_id", newName: "Purchase_id");
            RenameColumn(table: "dbo.Receivable_gl_account", name: "Accrued_sales_id", newName: "Accrued_purchase_id");
        }
    }
}
