namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_transaction", "Due_date", c => c.DateTime(nullable: true, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payable_transaction", "Due_date");
        }
    }
}
