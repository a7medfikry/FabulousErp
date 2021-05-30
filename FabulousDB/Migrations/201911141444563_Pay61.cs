namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay61 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_transactions_types", "Origin", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payable_transactions_types", "Origin");
        }
    }
}
