namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay19 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payable_payment", "Payment_no", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payable_payment", "Payment_no", c => c.Int(nullable: false));
        }
    }
}
