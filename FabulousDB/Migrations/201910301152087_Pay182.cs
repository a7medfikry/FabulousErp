namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay182 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_payment", "Cash_type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payable_payment", "Cash_type");
        }
    }
}
