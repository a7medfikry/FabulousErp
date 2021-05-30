namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pau20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assign_payable_doc", "Currency_id", c => c.String(maxLength: 50));
            CreateIndex("dbo.Assign_payable_doc", "Currency_id");
            AddForeignKey("dbo.Assign_payable_doc", "Currency_id", "dbo.CurrenciesDefinition_Table", "CurrencyID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assign_payable_doc", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropIndex("dbo.Assign_payable_doc", new[] { "Currency_id" });
            DropColumn("dbo.Assign_payable_doc", "Currency_id");
        }
    }
}
