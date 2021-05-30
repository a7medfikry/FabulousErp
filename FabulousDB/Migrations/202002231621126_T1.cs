namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class T1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.B_TaxSetting_table", "C_TaxSetting_table_CT_ID", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.F_TaxSetting_table", "C_TaxSetting_table_CT_ID", "dbo.C_TaxSetting_table");
            DropIndex("dbo.B_TaxSetting_table", new[] { "C_TaxSetting_table_CT_ID" });
            DropIndex("dbo.F_TaxSetting_table", new[] { "C_TaxSetting_table_CT_ID" });
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
            CreateIndex("dbo.F_TaxSetting_table", "C_TaxSetting_table_CT_ID");
            CreateIndex("dbo.B_TaxSetting_table", "C_TaxSetting_table_CT_ID");
            AddForeignKey("dbo.F_TaxSetting_table", "C_TaxSetting_table_CT_ID", "dbo.C_TaxSetting_table", "CT_ID");
            AddForeignKey("dbo.B_TaxSetting_table", "C_TaxSetting_table_CT_ID", "dbo.C_TaxSetting_table", "CT_ID");
        }
    }
}
