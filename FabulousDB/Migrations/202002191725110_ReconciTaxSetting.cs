namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReconciTaxSetting : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.C_TaxSetting_Table", newName: "TaxGroup_table");
            //DropForeignKey("dbo.C_TaxSetting_table", "CompanyID", "dbo.CompanyMainInfo_Table");
            //DropForeignKey("dbo.C_TaxSetting_Table", "CompanyID", "dbo.CompanyMainInfo_Table");
            //DropForeignKey("dbo.B_TaxSetting_table", "CT_ID", "dbo.C_TaxSetting_table");
            //DropForeignKey("dbo.F_TaxSetting_table", "CT_ID", "dbo.C_TaxSetting_table");
            //DropIndex("dbo.C_TaxSetting_table", new[] { "CompanyID" });
            //DropIndex("dbo.B_TaxSetting_table", new[] { "CT_ID" });
            //DropIndex("dbo.F_TaxSetting_table", new[] { "CT_ID" });
            //DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            //AddColumn("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID", c => c.String(maxLength: 10));
            //AddColumn("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1", c => c.String(maxLength: 10));
            //AddColumn("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID2", c => c.String(maxLength: 10));
            //AddColumn("dbo.B_TaxSetting_table", "C_TaxSetting_Table_CT_ID", c => c.Int());
            //AddColumn("dbo.F_TaxSetting_table", "C_TaxSetting_Table_CT_ID", c => c.Int());
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            //AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            //CreateIndex("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID");
            //CreateIndex("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1");
            //CreateIndex("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID2");
            //CreateIndex("dbo.B_TaxSetting_table", "C_TaxSetting_Table_CT_ID");
            //CreateIndex("dbo.B_TaxSetting_table", "C_TaxSetting_table_CT_ID");
            //CreateIndex("dbo.F_TaxSetting_table", "C_TaxSetting_Table_CT_ID");
            //CreateIndex("dbo.F_TaxSetting_table", "C_TaxSetting_table_CT_ID");
            //CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            //AddForeignKey("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1", "dbo.CompanyMainInfo_Table", "CompanyID");
            //AddForeignKey("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID2", "dbo.CompanyMainInfo_Table", "CompanyID");
            //AddForeignKey("dbo.B_TaxSetting_table", "C_TaxSetting_table_CT_ID", "dbo.C_TaxSetting_table", "CT_ID");
            //AddForeignKey("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID", "dbo.CompanyMainInfo_Table", "CompanyID");
            //AddForeignKey("dbo.F_TaxSetting_table", "C_TaxSetting_table_CT_ID", "dbo.C_TaxSetting_table", "CT_ID");
            //AddForeignKey("dbo.B_TaxSetting_table", "C_TaxSetting_Table_CT_ID", "dbo.C_TaxSetting_table", "CT_ID");
            //AddForeignKey("dbo.F_TaxSetting_table", "C_TaxSetting_Table_CT_ID", "dbo.C_TaxSetting_table", "CT_ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.F_TaxSetting_table", "C_TaxSetting_Table_CT_ID", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.B_TaxSetting_table", "C_TaxSetting_Table_CT_ID", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.F_TaxSetting_table", "C_TaxSetting_table_CT_ID", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID", "dbo.CompanyMainInfo_Table");
            DropForeignKey("dbo.B_TaxSetting_table", "C_TaxSetting_table_CT_ID", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID2", "dbo.CompanyMainInfo_Table");
            DropForeignKey("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1", "dbo.CompanyMainInfo_Table");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.F_TaxSetting_table", new[] { "C_TaxSetting_table_CT_ID" });
            DropIndex("dbo.F_TaxSetting_table", new[] { "C_TaxSetting_Table_CT_ID" });
            DropIndex("dbo.B_TaxSetting_table", new[] { "C_TaxSetting_table_CT_ID" });
            DropIndex("dbo.B_TaxSetting_table", new[] { "C_TaxSetting_Table_CT_ID" });
            DropIndex("dbo.C_TaxSetting_table", new[] { "CompanyMainInfo_Table_CompanyID2" });
            DropIndex("dbo.C_TaxSetting_table", new[] { "CompanyMainInfo_Table_CompanyID1" });
            DropIndex("dbo.C_TaxSetting_table", new[] { "CompanyMainInfo_Table_CompanyID" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.F_TaxSetting_table", "C_TaxSetting_Table_CT_ID");
            DropColumn("dbo.B_TaxSetting_table", "C_TaxSetting_Table_CT_ID");
            DropColumn("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID2");
            DropColumn("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID1");
            DropColumn("dbo.C_TaxSetting_table", "CompanyMainInfo_Table_CompanyID");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.F_TaxSetting_table", "CT_ID");
            CreateIndex("dbo.B_TaxSetting_table", "CT_ID");
            CreateIndex("dbo.C_TaxSetting_table", "CompanyID");
            AddForeignKey("dbo.F_TaxSetting_table", "CT_ID", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.B_TaxSetting_table", "CT_ID", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.C_TaxSetting_Table", "CompanyID", "dbo.CompanyMainInfo_Table", "CompanyID", cascadeDelete: true);
            AddForeignKey("dbo.C_TaxSetting_table", "CompanyID", "dbo.CompanyMainInfo_Table", "CompanyID");
            RenameTable(name: "dbo.TaxGroup_table", newName: "C_TaxSetting_Table");
        }
    }
}
