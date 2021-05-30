namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay131 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_transaction", "Transaction_date", c => c.DateTime(nullable: false, storeType: "date"));
            DropColumn("dbo.Payable_transaction", "Transation_date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payable_transaction", "Transation_date", c => c.DateTime(nullable: false, storeType: "date"));
            DropColumn("dbo.Payable_transaction", "Transaction_date");
        }
    }
}
