namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvModicatin4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_po_item_transfer", "Desc", c => c.String());
            AddColumn("dbo.Inv_po_item_transfer", "Transaction_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Inv_po_item_transfer", "Posting_date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_po_item_transfer", "Posting_date");
            DropColumn("dbo.Inv_po_item_transfer", "Transaction_date");
            DropColumn("dbo.Inv_po_item_transfer", "Desc");
        }
    }
}
