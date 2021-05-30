namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePayReceMandatory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Payable_gl_account", "Purchase_id", x => x.Int(nullable: true));
            AlterColumn("Payable_gl_account", "Taken_discount_id", x => x.Int(nullable: true));
            AlterColumn("Payable_gl_account", "Purchase_variance_id", x => x.Int(nullable: true));
            AlterColumn("Payable_gl_account", "Accrued_purchase_id", x => x.Int(nullable: true));
            AlterColumn("Payable_gl_account", "Returne_id", x => x.Int(nullable: true));

            AlterColumn("Receivable_gl_account", "Sales_id", x => x.Int(nullable: true));
            AlterColumn("Receivable_gl_account", "Discount_id", x => x.Int(nullable: true));
            AlterColumn("Receivable_gl_account", "Sales_variance_id", x => x.Int(nullable: true));
            AlterColumn("Receivable_gl_account", "Accrued_sales_id", x => x.Int(nullable: true));
            AlterColumn("Receivable_gl_account", "Returne_id", x => x.Int(nullable: true));
        }
        
        public override void Down()
        {
        }
    }
}
