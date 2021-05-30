namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamPoToPr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_purchase_request", "Pr_number", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_purchase_request", "Po_number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_purchase_request", "Po_number", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_purchase_request", "Pr_number");
        }
    }
}
