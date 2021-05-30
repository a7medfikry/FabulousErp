namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayF : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Address_info", newName: "Payable_address_info");
            RenameTable(name: "dbo.Creditor_setting", newName: "Payable_creditor_setting");
            RenameTable(name: "dbo.Payment_term", newName: "Payable_payment_term");
            RenameTable(name: "dbo.Shipping_method", newName: "Payable_shipping_method");
            RenameTable(name: "dbo.Bank_info", newName: "Payable_bank_info");
            RenameTable(name: "dbo.Creditro_currencies", newName: "Payable_creditor_currencies");
            RenameTable(name: "dbo.Legal_info", newName: "Payable_legal_info");
            RenameTable(name: "dbo.other_option", newName: "Payable_other_option");
            RenameTable(name: "dbo.aging_period", newName: "Payable_aging_period");
            RenameTable(name: "dbo.password_option", newName: "Payable_password_option");
            RenameTable(name: "dbo.Aging_date_option", newName: "Payable_aging_date_option");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.Payable_legal_info", newName: "Legal_info");
            RenameTable(name: "dbo.Payable_creditor_currencies", newName: "Creditro_currencies");
            RenameTable(name: "dbo.Payable_bank_info", newName: "Bank_info");
            RenameTable(name: "dbo.Payable_shipping_method", newName: "Shipping_method");
            RenameTable(name: "dbo.Payable_payment_term", newName: "Payment_term");
            RenameTable(name: "dbo.Payable_creditor_setting", newName: "Creditor_setting");
            RenameTable(name: "dbo.Payable_address_info", newName: "Address_info");
        }
    }
}
