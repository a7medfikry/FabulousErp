namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay15 : DbMigration
    {
        public override void Up()
        {
          //  DropForeignKey("dbo.Payable_transaction", "Vendor_id", "dbo.Creditor_setting");
          //  DropIndex("dbo.Payable_transaction", new[] { "Vendor_id" });
          //  AddColumn("dbo.Payable_transaction", "Currency_id", c => c.Int(nullable: false));
          //  AddColumn("dbo.Payable_transaction", "Creditor_Id", c => c.Int());
          //  CreateIndex("dbo.Payable_transaction", "Currency_id");
          //  CreateIndex("dbo.Payable_transaction", "Creditor_Id");
          //  AddForeignKey("dbo.Payable_transaction", "Creditor_Id", "dbo.Creditor_setting", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_transaction", "Creditor_Id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Payable_transaction", "Currency_id", "dbo.AccountCurrencyDefinition_Table");
            DropIndex("dbo.Payable_transaction", new[] { "Creditor_Id" });
            DropIndex("dbo.Payable_transaction", new[] { "Currency_id" });
            DropColumn("dbo.Payable_transaction", "Creditor_Id");
            DropColumn("dbo.Payable_transaction", "Currency_id");
            CreateIndex("dbo.Payable_transaction", "Vendor_id");
            AddForeignKey("dbo.Payable_transaction", "Vendor_id", "dbo.Creditor_setting", "Id", cascadeDelete: false);
        }
    }
}
