namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvUpdateFiscalYearAra : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id", "dbo.FiscalAdjustment_Table");
            DropForeignKey("dbo.Fiscal_year_area", "FiscalYear_Table_id", "dbo.FiscalYear_Table");
            DropIndex("dbo.Fiscal_year_area", new[] { "FiscalYear_Table_id" });
            DropIndex("dbo.Fiscal_year_area", new[] { "FiscalAdjustment_Table_id" });
            AlterColumn("dbo.Fiscal_year_area", "FiscalYear_Table_id", c => c.Int());
            AlterColumn("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id", c => c.Int());
            CreateIndex("dbo.Fiscal_year_area", "FiscalYear_Table_id");
            CreateIndex("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id");
            AddForeignKey("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id", "dbo.FiscalAdjustment_Table", "ID");
            AddForeignKey("dbo.Fiscal_year_area", "FiscalYear_Table_id", "dbo.FiscalYear_Table", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fiscal_year_area", "FiscalYear_Table_id", "dbo.FiscalYear_Table");
            DropForeignKey("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id", "dbo.FiscalAdjustment_Table");
            DropIndex("dbo.Fiscal_year_area", new[] { "FiscalAdjustment_Table_id" });
            DropIndex("dbo.Fiscal_year_area", new[] { "FiscalYear_Table_id" });
            AlterColumn("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Fiscal_year_area", "FiscalYear_Table_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id");
            CreateIndex("dbo.Fiscal_year_area", "FiscalYear_Table_id");
            AddForeignKey("dbo.Fiscal_year_area", "FiscalYear_Table_id", "dbo.FiscalYear_Table", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Fiscal_year_area", "FiscalAdjustment_Table_id", "dbo.FiscalAdjustment_Table", "ID", cascadeDelete: true);
        }
    }
}
