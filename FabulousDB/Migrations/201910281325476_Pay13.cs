namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay13 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Payable_payment", name: "Transaction Date", newName: "Transaction_date");
            RenameColumn(table: "dbo.Payable_payment", name: "Posting Date", newName: "Posting_date");
            RenameColumn(table: "dbo.Payable_payment", name: "Vendor Id", newName: "Vendor_id");
            RenameColumn(table: "dbo.Payable_payment", name: "Payment Type", newName: "Payment_type");
            RenameColumn(table: "dbo.Payable_payment", name: "Check Book", newName: "Check_book_id");
            RenameColumn(table: "dbo.Payable_payment", name: "Withdraw number", newName: "Withdraw_number");
            RenameColumn(table: "dbo.Payable_payment", name: "Currency Id", newName: "Currency_id");
            RenameColumn(table: "dbo.Payable_payment", name: "Cheque Number", newName: "Cheque_number");
            RenameColumn(table: "dbo.Payable_payment", name: "Due Date", newName: "Due_date");
            RenameColumn(table: "dbo.Payable_payment", name: "System Rate", newName: "System_rate");
            RenameColumn(table: "dbo.Payable_payment", name: "Transaction Rate", newName: "Transaction_rate");
            RenameColumn(table: "dbo.Payable_payment", name: "Orginal Amount", newName: "Orginal_amount");
            RenameColumn(table: "dbo.Payable_payment", name: "Taken Discount", newName: "Taken_discount");
            RenameColumn(table: "dbo.Payable_payment", name: "Paid Amount", newName: "Paid_amount");
            RenameIndex(table: "dbo.Payable_payment", name: "IX_Vendor Id", newName: "IX_Vendor_id");
            RenameIndex(table: "dbo.Payable_payment", name: "IX_Check Book", newName: "IX_Check_book_id");
            RenameIndex(table: "dbo.Payable_payment", name: "IX_Currency Id", newName: "IX_Currency_id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Payable_payment", name: "IX_Currency_id", newName: "IX_Currency Id");
            RenameIndex(table: "dbo.Payable_payment", name: "IX_Check_book_id", newName: "IX_Check Book");
            RenameIndex(table: "dbo.Payable_payment", name: "IX_Vendor_id", newName: "IX_Vendor Id");
            RenameColumn(table: "dbo.Payable_payment", name: "Paid_amount", newName: "Paid Amount");
            RenameColumn(table: "dbo.Payable_payment", name: "Taken_discount", newName: "Taken Discount");
            RenameColumn(table: "dbo.Payable_payment", name: "Orginal_amount", newName: "Orginal Amount");
            RenameColumn(table: "dbo.Payable_payment", name: "Transaction_rate", newName: "Transaction Rate");
            RenameColumn(table: "dbo.Payable_payment", name: "System_rate", newName: "System Rate");
            RenameColumn(table: "dbo.Payable_payment", name: "Due_date", newName: "Due Date");
            RenameColumn(table: "dbo.Payable_payment", name: "Cheque_number", newName: "Cheque Number");
            RenameColumn(table: "dbo.Payable_payment", name: "Currency_id", newName: "Currency Id");
            RenameColumn(table: "dbo.Payable_payment", name: "Withdraw_number", newName: "Withdraw number");
            RenameColumn(table: "dbo.Payable_payment", name: "Check_book_id", newName: "Check Book");
            RenameColumn(table: "dbo.Payable_payment", name: "Payment_type", newName: "Payment Type");
            RenameColumn(table: "dbo.Payable_payment", name: "Vendor_id", newName: "Vendor Id");
            RenameColumn(table: "dbo.Payable_payment", name: "Posting_date", newName: "Posting Date");
            RenameColumn(table: "dbo.Payable_payment", name: "Transaction_date", newName: "Transaction Date");
        }
    }
}
