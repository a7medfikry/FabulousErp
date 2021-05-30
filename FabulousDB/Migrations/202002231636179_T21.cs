namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class T21 : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            //CreateIndex("dbo.C_TaxSetting_table", "CompanyID");
            //CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            //AddForeignKey("dbo.C_TaxSetting_table", "CompanyID", "dbo.CompanyMainInfo_Table", "CompanyID");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.C_TaxSetting_table", "CompanyID", "dbo.CompanyMainInfo_Table");
            //DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            //DropIndex("dbo.C_TaxSetting_table", new[] { "CompanyID" });
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            //CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
