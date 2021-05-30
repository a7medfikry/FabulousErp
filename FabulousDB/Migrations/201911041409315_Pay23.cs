namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay23 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Payable_transaction", name: "Shipping Method", newName: "Shipping_method_id");
            RenameIndex(table: "dbo.Payable_transaction", name: "IX_Shipping Method", newName: "IX_Shipping_method_id");
            AddColumn("dbo.Payable_payment", "Posting_number", c => c.Int(nullable: false));
            AddColumn("dbo.Payable_transaction", "Posting_number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payable_transaction", "Posting_number");
            DropColumn("dbo.Payable_payment", "Posting_number");
            RenameIndex(table: "dbo.Payable_transaction", name: "IX_Shipping_method_id", newName: "IX_Shipping Method");
            RenameColumn(table: "dbo.Payable_transaction", name: "Shipping_method_id", newName: "Shipping Method");
        }
    }
}
