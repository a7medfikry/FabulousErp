namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMyFiscalYear : DbMigration
    {
        public override void Up()
        {
          
        }
        
        public override void Down()
        {
            AddColumn("dbo.FiscalAdjustment_Table", "Inventory", c => c.Boolean());
            AddColumn("dbo.FiscalAdjustment_Table", "Sales", c => c.Boolean());
            AddColumn("dbo.FiscalAdjustment_Table", "Purchasing", c => c.Boolean());
            AddColumn("dbo.FiscalAdjustment_Table", "Financial", c => c.Boolean());
            DropForeignKey("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id", "dbo.FiscalAdjustment_Table");
            DropIndex("dbo.Fiscal_year_area", new[] { "FiscalAdjustment_Table_id" });
            DropColumn("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id");
        }
    }
}
