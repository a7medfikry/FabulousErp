namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay62 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Reciept", c => c.Decimal(precision: 30, scale: 9));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Payment", c => c.Decimal(precision: 30, scale: 9));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Balance", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.C_BankReconcile_table", "Bank_Statment_Ending_Balance", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Creditor_setting", "Minimum_order", c => c.Decimal(precision: 30, scale: 9));
            AlterColumn("dbo.Creditor_setting", "Maximum_order", c => c.Decimal(precision: 30, scale: 9));
            AlterColumn("dbo.Creditor_setting", "Credit_limit_amount", c => c.Decimal(precision: 30, scale: 9));
            AlterColumn("dbo.Creditor_setting", "Min_payment_amount", c => c.Decimal(precision: 30, scale: 9));
            AlterColumn("dbo.Payable_Group_setting", "Minimum_transaction", c => c.Decimal(precision: 30, scale: 9));
            AlterColumn("dbo.Payable_Group_setting", "Maximum_transaction", c => c.Decimal(precision: 30, scale: 9));
            AlterColumn("dbo.Payable_Group_setting", "Credit_limit_amount", c => c.Decimal(precision: 30, scale: 9));
            AlterColumn("dbo.Payable_Group_setting", "Min_payment_amount", c => c.Decimal(precision: 30, scale: 9));
            AlterColumn("dbo.Payment_term", "Amount", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Assign_payable_doc", "Orginal_amount", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Assign_payable_doc", "Applay_assign", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Assign_payable_doc", "Unassign_amount", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Assign_payable_doc", "Earn_or_lose", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Payable_payment", "System_rate", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Payable_payment", "Transaction_rate", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Payable_payment", "Orginal_amount", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Payable_payment", "Taken_discount", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Payable_payment", "Paid_amount", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Payable_transaction", "System_rate", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Payable_transaction", "Transaction_rate", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Payable_transaction", "Purchase", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Payable_transaction", "Taken_discount", c => c.Decimal(nullable: false, precision: 30, scale: 9));
            AlterColumn("dbo.Payable_transaction", "Tax", c => c.Decimal(nullable: false, precision: 30, scale: 9));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payable_transaction", "Tax", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_transaction", "Taken_discount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_transaction", "Purchase", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_transaction", "Transaction_rate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_transaction", "System_rate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_payment", "Paid_amount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_payment", "Taken_discount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_payment", "Orginal_amount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_payment", "Transaction_rate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_payment", "System_rate", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Assign_payable_doc", "Earn_or_lose", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Assign_payable_doc", "Unassign_amount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Assign_payable_doc", "Applay_assign", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Assign_payable_doc", "Orginal_amount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payment_term", "Amount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_Group_setting", "Min_payment_amount", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Payable_Group_setting", "Credit_limit_amount", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Payable_Group_setting", "Maximum_transaction", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Payable_Group_setting", "Minimum_transaction", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Creditor_setting", "Min_payment_amount", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Creditor_setting", "Credit_limit_amount", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Creditor_setting", "Maximum_order", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.Creditor_setting", "Minimum_order", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.C_BankReconcile_table", "Bank_Statment_Ending_Balance", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Balance", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Payment", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Reciept", c => c.Decimal(precision: 20, scale: 4));
        }
    }
}
