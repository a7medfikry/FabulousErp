namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvTaxSettingFx : DbMigration
    {
        public override void Up()
        {
           // DropForeignKey("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1", "dbo.CompanyMainInfo_Table");
           // DropIndex("dbo.C_TaxSetting_table", new[] { "CompanyMainInfo_Table_CompanyID1" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            //DropColumn("dbo.C_TaxSetting_table", "CompanyID");
            //RenameColumn(table: "dbo.C_TaxSetting_table", name: "CompanyMainInfo_Table_CompanyID", newName: "CompanyID");
            //RenameIndex(table: "dbo.C_TaxSetting_table", name: "IX_CompanyMainInfo_Table_CompanyID", newName: "IX_CompanyID");
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
           // DropColumn("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1", c => c.String(maxLength: 10));
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.C_TaxSetting_table", name: "IX_CompanyID", newName: "IX_CompanyMainInfo_Table_CompanyID");
            RenameColumn(table: "dbo.C_TaxSetting_table", name: "CompanyID", newName: "CompanyMainInfo_Table_CompanyID");
            AddColumn("dbo.C_TaxSetting_table", "CompanyID", c => c.String(maxLength: 10));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1");
            AddForeignKey("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1", "dbo.CompanyMainInfo_Table", "CompanyID");
        }
    }
}
