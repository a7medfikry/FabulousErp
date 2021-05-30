namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_po_GS", "Next_pr_no", c => c.Int(nullable: false));
           
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_po_GS", "Next_pr_no");
        }
    }
}
