namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay44 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payable_transaction", "Desc", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payable_transaction", "Desc", c => c.String());
        }
    }
}
