namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Finalize1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CurrenciesDefinition_Table", "Currency_unit_name", c => c.String(maxLength: 50));
            AddColumn("dbo.CurrenciesDefinition_Table", "Currency_small_unit_name", c => c.String(maxLength: 50));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_CheckNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_CheckNumber", c => c.Int());
            DropColumn("dbo.CurrenciesDefinition_Table", "Currency_small_unit_name");
            DropColumn("dbo.CurrenciesDefinition_Table", "Currency_unit_name");
        }
    }
}
