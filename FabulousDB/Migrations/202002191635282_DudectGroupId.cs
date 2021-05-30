namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DudectGroupId : DbMigration
    {
        public override void Up()
        {
            DropTable("Inv_item_deduct_tax");

            CreateTable(
            "dbo.Inv_item_deduct_tax",
            c => new
            {
                Id = c.Int(nullable: false, identity: true),
                Deduct_id = c.Int(nullable: false),
                item_group_id = c.Int(nullable: false),
            })
            .PrimaryKey(t => t.Id)
            //.ForeignKey("dbo.C_TaxSetting_Table", t => t.Deduct_id, cascadeDelete: false)
            .ForeignKey("dbo.Inv_item_group", t => t.item_group_id, cascadeDelete: false)
            .Index(t => t.Deduct_id)
            .Index(t => t.item_group_id);
        }
        
        public override void Down()
        {
        }
    }
}
