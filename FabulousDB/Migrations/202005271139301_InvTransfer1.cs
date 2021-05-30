namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvTransfer1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po", "Transfer_num", c => c.Int());
            AddColumn("dbo.Inv_sales_invoice", "Transfer_num", c => c.Int());
            AddColumn("dbo.Inv_po_item_transfer", "Transfer_num", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po", "Gr_num", c => c.Int());
            AlterColumn("dbo.Inv_sales_invoice", "Go_num", c => c.Int());
            DropColumn("dbo.Inv_receive_po", "Batch_id");
            DropColumn("dbo.Inv_receive_po", "Buyer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_receive_po", "Buyer", c => c.String());
            AddColumn("dbo.Inv_receive_po", "Batch_id", c => c.Int());
            AlterColumn("dbo.Inv_sales_invoice", "Go_num", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po", "Gr_num", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_po_item_transfer", "Transfer_num");
            DropColumn("dbo.Inv_sales_invoice", "Transfer_num");
            DropColumn("dbo.Inv_receive_po", "Transfer_num");
        }
    }
}
