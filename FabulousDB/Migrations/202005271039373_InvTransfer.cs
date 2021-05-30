namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvTransfer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_po_item_transfer", "Transfer_date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Inv_po_item_transfer", "Transaction_date");
            DropColumn("dbo.Inv_po_item_transfer", "Posting_date");
        }
        public override void Down()
        {
            AddColumn("dbo.Inv_po_item_transfer", "Posting_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Inv_po_item_transfer", "Transaction_date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Inv_po_item_transfer", "Transfer_date");
        }
    }
}
