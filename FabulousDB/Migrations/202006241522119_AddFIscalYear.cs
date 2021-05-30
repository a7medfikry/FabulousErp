namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFIscalYear : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fiscal_year_area",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Area_name = c.String(),
                        Allowed = c.Boolean(nullable: false),
                        FiscalYear_Table_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FiscalYear_Table", t => t.FiscalYear_Table_id, cascadeDelete: false)
                .Index(t => t.FiscalYear_Table_id);
            
            DropColumn("dbo.FiscalYear_Table", "Financial");
            DropColumn("dbo.FiscalYear_Table", "Purchasing");
            DropColumn("dbo.FiscalYear_Table", "Sales");
            DropColumn("dbo.FiscalYear_Table", "Inventory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FiscalYear_Table", "Inventory", c => c.Boolean());
            AddColumn("dbo.FiscalYear_Table", "Sales", c => c.Boolean());
            AddColumn("dbo.FiscalYear_Table", "Purchasing", c => c.Boolean());
            AddColumn("dbo.FiscalYear_Table", "Financial", c => c.Boolean());
            DropForeignKey("dbo.Fiscal_year_area", "FiscalYear_Table_id", "dbo.FiscalYear_Table");
            DropIndex("dbo.Fiscal_year_area", new[] { "FiscalYear_Table_id" });
            DropTable("dbo.Fiscal_year_area");
        }
    }
}
