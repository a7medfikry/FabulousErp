namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay67 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payable_transaction", "Creditor_Id", "dbo.Creditor_setting");
            DropIndex("dbo.Payable_transaction", new[] { "Creditor_Id" });
            DropColumn("dbo.Payable_transaction", "Vendor_id");
            RenameColumn(table: "dbo.Payable_transaction", name: "Creditor_Id", newName: "Vendor_id");
            AlterColumn("dbo.Payable_transaction", "Vendor_id", c => c.Int(nullable: true));
            CreateIndex("dbo.Payable_transaction", "Vendor_id");
            AddForeignKey("dbo.Payable_transaction", "Vendor_id", "dbo.Creditor_setting", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_transaction", "Vendor_id", "dbo.Creditor_setting");
            DropIndex("dbo.Payable_transaction", new[] { "Vendor_id" });
            AlterColumn("dbo.Payable_transaction", "Vendor_id", c => c.Int());
            RenameColumn(table: "dbo.Payable_transaction", name: "Vendor_id", newName: "Creditor_Id");
            AddColumn("dbo.Payable_transaction", "Vendor_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_transaction", "Creditor_Id");
            AddForeignKey("dbo.Payable_transaction", "Creditor_Id", "dbo.Creditor_setting", "Id");
        }
    }
}
