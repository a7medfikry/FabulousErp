namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPriceList1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_pricelist", "Unit_of_measure_id", "dbo.Unit_of_measure");
            DropIndex("dbo.Inv_pricelist", new[] { "Unit_of_measure_id" });
            AlterColumn("dbo.Inv_pricelist", "Unit_of_measure_id", c => c.Int());
            CreateIndex("dbo.Inv_pricelist", "Unit_of_measure_id");
            AddForeignKey("dbo.Inv_pricelist", "Unit_of_measure_id", "dbo.Unit_of_measure", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_pricelist", "Unit_of_measure_id", "dbo.Unit_of_measure");
            DropIndex("dbo.Inv_pricelist", new[] { "Unit_of_measure_id" });
            AlterColumn("dbo.Inv_pricelist", "Unit_of_measure_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_pricelist", "Unit_of_measure_id");
            AddForeignKey("dbo.Inv_pricelist", "Unit_of_measure_id", "dbo.Unit_of_measure", "Id", cascadeDelete: true);
        }
    }
}
