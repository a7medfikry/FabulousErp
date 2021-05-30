namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Installment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Installment_contract", "FK_dbo.Installment_contract_dbo.Receivable_vendore_setting_Customer_id");
            CreateIndex("dbo.Installment_contract", "Customer_id");
            AddForeignKey("dbo.Installment_contract", "Customer_id", "dbo.Receivable_vendore_setting", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Installment_contract", "Customer_id", "dbo.Receivable_vendore_setting");
            DropIndex("dbo.Installment_contract", new[] { "Customer_id" });
        }
    }
}
