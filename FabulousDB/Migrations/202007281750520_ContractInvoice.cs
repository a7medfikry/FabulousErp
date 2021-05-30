namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContractInvoice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Installment_contract_invoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contract_id = c.Int(nullable: false),
                        Invoice_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Installment_contract", t => t.Contract_id, cascadeDelete: true)
                .ForeignKey("dbo.Inv_sales_invoice", t => t.Invoice_id, cascadeDelete: true)
                .Index(t => t.Contract_id)
                .Index(t => t.Invoice_id);
            
            AddColumn("dbo.Installment_contract", "Pay_for", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Installment_contract_invoice", "Invoice_id", "dbo.Inv_sales_invoice");
            DropForeignKey("dbo.Installment_contract_invoice", "Contract_id", "dbo.Installment_contract");
            DropIndex("dbo.Installment_contract_invoice", new[] { "Invoice_id" });
            DropIndex("dbo.Installment_contract_invoice", new[] { "Contract_id" });
            DropColumn("dbo.Installment_contract", "Pay_for");
            DropTable("dbo.Installment_contract_invoice");
        }
    }
}
