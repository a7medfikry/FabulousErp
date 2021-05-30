namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec111 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Receivable_vendore_setting", "Vendor_name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Payable_creditor_setting", "Vendor_name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Receivable_vendore_setting", "Vendor_name", c => c.String(maxLength: 100));
        }
    }
}
