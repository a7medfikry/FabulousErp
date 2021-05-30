namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay78 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Payable_payment", "Paid_amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payable_payment", "Paid_amount", c => c.Decimal(nullable: false, precision: 30, scale: 9));
        }
    }
}
