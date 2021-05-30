namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec201 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Receivable_payment", "Taken_discount");
            DropColumn("dbo.Payable_payment", "Taken_discount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Receivable_payment", "Taken_discount", c => c.Decimal(nullable: false, precision: 30, scale: 9));
        }
    }
}
