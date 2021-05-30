namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay341 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Creditor_setting", "Credit_limit_amount", c => c.Decimal(precision: 20, scale: 4));
            AddColumn("dbo.Creditor_setting", "Min_payment_amount", c => c.Decimal(precision: 20, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Creditor_setting", "Min_payment_amount");
            DropColumn("dbo.Creditor_setting", "Credit_limit_amount");
        }
    }
}
