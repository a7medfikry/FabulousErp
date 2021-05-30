namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RPC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receivable_vendore_setting", "Creation_date", c => c.DateTime(nullable: false,defaultValueSql:"GetDate()"));
            AddColumn("dbo.Payable_creditor_setting", "Creation_date", c => c.DateTime(nullable: false, defaultValueSql: "GetDate()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payable_creditor_setting", "Creation_date");
            DropColumn("dbo.Receivable_vendore_setting", "Creation_date");
        }
    }
}
