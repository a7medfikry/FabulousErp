namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMySiscalYear2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id");
            AddForeignKey("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id", "dbo.FiscalAdjustment_Table", "ID", cascadeDelete: false);
            DropColumn("dbo.FiscalAdjustment_Table", "Financial");
            DropColumn("dbo.FiscalAdjustment_Table", "Purchasing");
            DropColumn("dbo.FiscalAdjustment_Table", "Sales");
            DropColumn("dbo.FiscalAdjustment_Table", "Inventory");
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
