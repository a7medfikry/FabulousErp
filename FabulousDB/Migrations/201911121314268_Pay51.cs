namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay51 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assign_payable_doc", "Earn_or_lose", c => c.Decimal(nullable: false, precision: 20, scale: 4));
        }
        public override void Down()
        {
            DropColumn("dbo.Assign_payable_doc", "Earn_or_lose");
        }
    }
}
