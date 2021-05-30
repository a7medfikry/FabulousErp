namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPriceList2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_pricelist", "Unit_of_measure_id", "dbo.Unit_of_measure");
            DropIndex("dbo.Inv_pricelist", new[] { "Unit_of_measure_id" });
            DropColumn("dbo.Inv_pricelist", "Unit_of_measure_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_pricelist", "Unit_of_measure_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_pricelist", "Unit_of_measure_id");
            AddForeignKey("dbo.Inv_pricelist", "Unit_of_measure_id", "dbo.Unit_of_measure", "Id", cascadeDelete: true);
        }
    }
}
