namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxSettingFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.C_TaxSetting_table", "CompanyID", "dbo.CompanyMainInfo_Table");
            DropIndex("dbo.C_TaxSetting_table", new[] { "CompanyID" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.C_TaxSetting_table", "CompanyID");
            AddForeignKey("dbo.C_TaxSetting_table", "CompanyID", "dbo.CompanyMainInfo_Table", "CompanyID");
        }
    }
}
