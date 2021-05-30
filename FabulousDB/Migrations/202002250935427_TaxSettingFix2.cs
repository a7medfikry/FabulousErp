namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxSettingFix2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.B_TaxSetting_table", "TG_ID", "dbo.TaxGroup_table");
            DropForeignKey("dbo.F_TaxSetting_table", "TG_ID", "dbo.TaxGroup_table");
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
            AddForeignKey("dbo.F_TaxSetting_table", "TG_ID", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: true);
            AddForeignKey("dbo.B_TaxSetting_table", "TG_ID", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: true);
        }
    }
}
