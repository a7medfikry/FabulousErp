namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay80 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assign_payable_doc", "Transaction_rate", c => c.Decimal(nullable: false, precision: 30, scale: 9));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assign_payable_doc", "Transaction_rate");
        }
    }
}
