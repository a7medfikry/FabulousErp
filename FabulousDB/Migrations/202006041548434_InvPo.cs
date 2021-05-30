namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item_adjustment", "Posting_num", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_item_adjustment", "Posting_num");
        }
    }
}
