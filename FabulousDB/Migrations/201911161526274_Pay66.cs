namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay66 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assign_payable_doc", "Creation_date", c => c.DateTime(nullable: false, defaultValueSql: "GetDate()"));
            AddColumn("dbo.Payable_payment", "Creation_date", c => c.DateTime(nullable: false, defaultValueSql: "GetDate()"));
            AddColumn("dbo.Payable_transaction", "Creation_date", c => c.DateTime(nullable: false, defaultValueSql: "GetDate()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payable_transaction", "Creation_date");
            DropColumn("dbo.Payable_payment", "Creation_date");
            DropColumn("dbo.Assign_payable_doc", "Creation_date");
        }
    }
}
