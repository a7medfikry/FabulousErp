namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePrNo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inv_purchase_request", "PR_no");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_purchase_request", "PR_no", c => c.String());
        }
    }
}
