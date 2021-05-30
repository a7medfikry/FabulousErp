namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntT10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_item", "Unit_of_measure_id", "dbo.Unit_of_measure");
            DropIndex("dbo.Inv_item", new[] { "Unit_of_measure_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_item", "Unit_of_measure_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_item", "Unit_of_measure_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            AddForeignKey("dbo.Inv_item", "Unit_of_measure_id", "dbo.Unit_of_measure", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item", "Unit_of_measure_id", "dbo.Unit_of_measure");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_item", new[] { "Unit_of_measure_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_item", "Unit_of_measure_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_item", "Unit_of_measure_id");
            AddForeignKey("dbo.Inv_item", "Unit_of_measure_id", "dbo.Unit_of_measure", "Id", cascadeDelete: true);
        }
    }
}
