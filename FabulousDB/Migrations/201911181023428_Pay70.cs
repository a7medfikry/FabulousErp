namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay70 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assign_payable_doc", "Is_void", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payable_payment", "Is_void", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payable_transaction", "Is_void", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payable_transaction", "Is_void");
            DropColumn("dbo.Payable_payment", "Is_void");
            DropColumn("dbo.Assign_payable_doc", "Is_void");
        }
    }
}
