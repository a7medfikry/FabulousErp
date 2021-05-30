namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay71 : DbMigration
    {
        public override void Up()
        {
           // RenameColumn(table: "dbo.Payable_transaction", name: "Description", newName: "Desc");
            //RenameColumn(table: "dbo.Payable_transaction", name: "Transaction Date", newName: "Transation_date");
            //RenameColumn(table: "dbo.Payable_transaction", name: "Posting Date", newName: "Posting_date");
            //RenameColumn(table: "dbo.Payable_transaction", name: "System Rate", newName: "System_rate");
            //RenameColumn(table: "dbo.Payable_transaction", name: "Transaction Rate", newName: "Transaction_rate");
            //RenameColumn(table: "dbo.Payable_transaction", name: "Creditor", newName: "Vendor_id");
            //RenameColumn(table: "dbo.Payable_transaction", name: "P.O Number", newName: "PONumber");
            //RenameColumn(table: "dbo.Payable_transaction", name: "V.Document Number", newName: "VDocument_number");
            //RenameColumn(table: "dbo.Payable_transaction", name: "Document Date", newName: "Doc_date");
            //RenameColumn(table: "dbo.Payable_transaction", name: "Payment Term", newName: "Payment_terms_id");
            //RenameColumn(table: "dbo.Payable_transaction", name: "Taken Discount", newName: "Taken_discount");
            //RenameIndex(table: "dbo.Payable_transaction", name: "IX_Creditor", newName: "IX_Vendor_id");
            //RenameIndex(table: "dbo.Payable_transaction", name: "IX_Payment Term", newName: "IX_Payment_terms_id");
            AddColumn("dbo.Payable_transaction", "Doc_type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payable_transaction", "Doc_type");
            RenameIndex(table: "dbo.Payable_transaction", name: "IX_Payment_terms_id", newName: "IX_Payment Term");
            RenameIndex(table: "dbo.Payable_transaction", name: "IX_Vendor_id", newName: "IX_Creditor");
            RenameColumn(table: "dbo.Payable_transaction", name: "Taken_discount", newName: "Taken Discount");
            RenameColumn(table: "dbo.Payable_transaction", name: "Payment_terms_id", newName: "Payment Term");
            RenameColumn(table: "dbo.Payable_transaction", name: "Doc_date", newName: "Document Date");
            RenameColumn(table: "dbo.Payable_transaction", name: "VDocument_number", newName: "V.Document Number");
            RenameColumn(table: "dbo.Payable_transaction", name: "PONumber", newName: "P.O Number");
            RenameColumn(table: "dbo.Payable_transaction", name: "Vendor_id", newName: "Creditor");
            RenameColumn(table: "dbo.Payable_transaction", name: "Transaction_rate", newName: "Transaction Rate");
            RenameColumn(table: "dbo.Payable_transaction", name: "System_rate", newName: "System Rate");
            RenameColumn(table: "dbo.Payable_transaction", name: "Posting_date", newName: "Posting Date");
            RenameColumn(table: "dbo.Payable_transaction", name: "Transation_date", newName: "Transaction Date");
            RenameColumn(table: "dbo.Payable_transaction", name: "Desc", newName: "Description");
        }
    }
}
