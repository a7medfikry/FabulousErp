namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay351 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payable_Group_setting", "Group_id", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Payable_Group_setting", "Minimum_transaction", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Payable_Group_setting", "Maximum_transaction", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Payable_Group_setting", "Credit_limit", c => c.Int());
            AlterColumn("dbo.Payable_Group_setting", "Minimum_payment", c => c.Int());
            AlterColumn("dbo.Payable_Group_setting", "Payment_per", c => c.Int());
            AlterColumn("dbo.Payable_Group_setting", "Revaluate", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payable_Group_setting", "Revaluate", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Payable_Group_setting", "Payment_per", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_Group_setting", "Minimum_payment", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_Group_setting", "Credit_limit", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_Group_setting", "Maximum_transaction", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_Group_setting", "Minimum_transaction", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_Group_setting", "Group_id", c => c.String(maxLength: 50));
        }
    }
}
