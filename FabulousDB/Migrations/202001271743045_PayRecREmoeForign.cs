namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayRecREmoeForign : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Receivable_payment", "FK_dbo.Receivable_payment_dbo.Installment_setting_Installment_id");
            DropForeignKey("dbo.Payable_payment", "FK_dbo.Payable_payment_dbo.Installment_setting_Installment_id");
        }
        
        public override void Down()
        {
        }
    }
}
