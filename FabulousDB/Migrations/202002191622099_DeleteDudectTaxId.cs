namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDudectTaxId : DbMigration
    {
        public override void Up()
        {
           // DropTable("dbo.Inv_item_deduct_tax");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Inv_item_deduct_tax",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deduct_id = c.Int(nullable: false),
                        item_group_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_item_deduct_tax", "item_group_id");
            CreateIndex("dbo.Inv_item_deduct_tax", "Deduct_id");
            AddForeignKey("dbo.Inv_item_deduct_tax", "item_group_id", "dbo.Inv_item_group", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_deduct_tax", "Deduct_id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: true);
           // RenameTable(name: "dbo.C_TaxSetting_Table", newName: "TaxGroup_table");
        }
    }
}
