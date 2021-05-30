namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvGroupDeduct : DbMigration
    {
        public override void Up()
        {
         
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_deduct_tax", "item_group_id", "dbo.Inv_item_group");
            DropForeignKey("dbo.Inv_item_deduct_tax", "Deduct_id", "dbo.C_TaxSetting_Table");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_item_deduct_tax", new[] { "item_group_id" });
            DropIndex("dbo.Inv_item_deduct_tax", new[] { "Deduct_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropTable("dbo.Inv_item_deduct_tax");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
