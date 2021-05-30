namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay181 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payable_transaction", "Due_date", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payable_transaction", "Due_date", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
