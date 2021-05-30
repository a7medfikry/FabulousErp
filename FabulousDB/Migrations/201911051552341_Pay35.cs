namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay35 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_Group_setting", "Min_payment_amount", c => c.Decimal(precision: 20, scale: 4));
            DropColumn("dbo.Payable_Group_setting", "Min_payment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payable_Group_setting", "Min_payment", c => c.Decimal(precision: 20, scale: 4));
            DropColumn("dbo.Payable_Group_setting", "Min_payment_amount");
        }
    }
}
