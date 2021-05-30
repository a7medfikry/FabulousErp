namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaybleInstallment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Purchase_Installment_contract_invoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contract_id = c.Int(nullable: false),
                        Invoice_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Installment_contract", t => t.Contract_id, cascadeDelete: true)
                .ForeignKey("dbo.Inv_receive_po", t => t.Invoice_id, cascadeDelete: true)
                .Index(t => t.Contract_id)
                .Index(t => t.Invoice_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purchase_Installment_contract_invoice", "Invoice_id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Purchase_Installment_contract_invoice", "Contract_id", "dbo.Installment_contract");
            DropIndex("dbo.Purchase_Installment_contract_invoice", new[] { "Invoice_id" });
            DropIndex("dbo.Purchase_Installment_contract_invoice", new[] { "Contract_id" });
            DropTable("dbo.Purchase_Installment_contract_invoice");
        }
    }
}
