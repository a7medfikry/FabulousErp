namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay34 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_Group_setting", "Credit_limit_amount", c => c.Decimal(precision: 20, scale: 4));
            AddColumn("dbo.Payable_Group_setting", "Min_payment", c => c.Decimal(precision: 20, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payable_Group_setting", "Min_payment");
            DropColumn("dbo.Payable_Group_setting", "Credit_limit_amount");
        }
    }
}
