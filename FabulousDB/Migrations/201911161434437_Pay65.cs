namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay65 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assign_payable_doc", "Assign_no", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assign_payable_doc", "Assign_no");
        }
    }
}
