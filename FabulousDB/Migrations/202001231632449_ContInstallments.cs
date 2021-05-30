namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContInstallments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Installments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contract_id = c.Int(nullable: false),
                        Cheque_number = c.String(maxLength: 500),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Percentage = c.Single(nullable: false),
                        Cheque_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Installment_contract", t => t.Contract_id, cascadeDelete: true)
                .Index(t => t.Contract_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Installments", "Contract_id", "dbo.Installment_contract");
            DropIndex("dbo.Installments", new[] { "Contract_id" });
            DropTable("dbo.Installments");
        }
    }
}
