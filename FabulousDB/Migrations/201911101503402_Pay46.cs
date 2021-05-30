namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay46 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Payable_payment", "Payment_type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payable_payment", "Payment_type", c => c.Int(nullable: false));
        }
    }
}
