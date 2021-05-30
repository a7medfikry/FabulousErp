namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class T : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID", "dbo.CompanyMainInfo_Table");
            //DropIndex("dbo.C_TaxSetting_table", new[] { "CompanyMainInfo_Table_CompanyID1" });
            //DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            //DropColumn("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID");
            //RenameColumn(table: "dbo.C_TaxSetting_table", name: "CompanyMainInfo_Table_CompanyID1", newName: "__mig_tmp__0");
            //RenameColumn(table: "dbo.C_TaxSetting_table", name: "CompanyMainInfo_Table_CompanyID2", newName: "CompanyMainInfo_Table_CompanyID1");
            //RenameColumn(table: "dbo.C_TaxSetting_table", name: "__mig_tmp__0", newName: "CompanyMainInfo_Table_CompanyID");
            //RenameIndex(table: "dbo.C_TaxSetting_table", name: "IX_CompanyMainInfo_Table_CompanyID2", newName: "IX_CompanyMainInfo_Table_CompanyID1");
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            //CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
        
        public override void Down()
        {
            //DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            //RenameIndex(table: "dbo.C_TaxSetting_table", name: "IX_CompanyMainInfo_Table_CompanyID1", newName: "IX_CompanyMainInfo_Table_CompanyID2");
            //RenameColumn(table: "dbo.C_TaxSetting_table", name: "CompanyMainInfo_Table_CompanyID", newName: "__mig_tmp__0");
            //RenameColumn(table: "dbo.C_TaxSetting_table", name: "CompanyMainInfo_Table_CompanyID1", newName: "CompanyMainInfo_Table_CompanyID2");
            //RenameColumn(table: "dbo.C_TaxSetting_table", name: "__mig_tmp__0", newName: "CompanyMainInfo_Table_CompanyID1");
            //AddColumn("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID", c => c.String(maxLength: 10));
            //CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            //CreateIndex("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1");
            //AddForeignKey("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID", "dbo.CompanyMainInfo_Table", "CompanyID");
        }
    }
}
