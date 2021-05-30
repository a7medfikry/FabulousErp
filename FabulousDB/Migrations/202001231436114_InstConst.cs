namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstConst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Installment_contract",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contract_no = c.String(),
                        Desc = c.String(),
                        Vendore_id = c.Int(),
                        Customer_id = c.Int(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency_id = c.String(maxLength: 50),
                        Installment_plan_id = c.Int(nullable: false),
                        Contract_date = c.DateTime(nullable: false),
                        Creation_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Installment_setting", t => t.Installment_plan_id, cascadeDelete: false)
                .ForeignKey("dbo.Payable_creditor_setting", t => t.Vendore_id)
                .Index(t => t.Vendore_id)
                .Index(t => t.Currency_id)
                .Index(t => t.Installment_plan_id);
            AddForeignKey("dbo.Installment_contract", "Customer_id", "dbo.Receivable_vendore_setting"
                , "Id");
          
            
        }
        
        public override void Down()
        {
          
        }
    }
}
