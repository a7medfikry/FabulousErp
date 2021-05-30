namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay81 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payable_payment", "Reference", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payable_payment", "Reference", c => c.String(maxLength: 500));
        }
    }
}
