namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receivable_shipping_method", "Inactive", c => c.Boolean(nullable: false, defaultValueSql: "0"));
            AddColumn("dbo.Payable_shipping_method", "Inactive", c => c.Boolean(nullable: false,defaultValueSql:"0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Receivable_shipping_method", "Inactive");
        }
    }
}
