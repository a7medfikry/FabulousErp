namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvStocing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_stocking", "Po_id", "dbo.Inv_receive_po");
            DropIndex("dbo.Inv_stocking", new[] { "Po_id" });
            AddColumn("dbo.Inv_stocking", "UOM_id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_stocking", "Site_id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_stocking", "Exist", c => c.Single(nullable: false));
            AddColumn("dbo.Inv_stocking", "Adjust", c => c.Single(nullable: false));
            AddColumn("dbo.Inv_stocking", "Diffrance", c => c.Single(nullable: false));
            AddColumn("dbo.Inv_stocking", "Orginal_amount", c => c.Single(nullable: false));
            AddColumn("dbo.Inv_stocking", "Transaction_date", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Inv_stocking", "UOM_id");
            CreateIndex("dbo.Inv_stocking", "Site_id");
            AddForeignKey("dbo.Inv_stocking", "Site_id", "dbo.Inv_store_site", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Inv_stocking", "UOM_id", "dbo.Unit_of_measure", "Id", cascadeDelete: false);
            DropColumn("dbo.Inv_stocking", "Posting_num");
            DropColumn("dbo.Inv_stocking", "Po_id");
            DropColumn("dbo.Inv_stocking", "Diffrence");
            DropColumn("dbo.Inv_stocking", "Actual");
            DropColumn("dbo.Inv_stocking", "Damage");
            DropColumn("dbo.Inv_stocking", "Unit_cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_stocking", "Unit_cost", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AddColumn("dbo.Inv_stocking", "Damage", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AddColumn("dbo.Inv_stocking", "Actual", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AddColumn("dbo.Inv_stocking", "Diffrence", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AddColumn("dbo.Inv_stocking", "Po_id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_stocking", "Posting_num", c => c.String());
            DropForeignKey("dbo.Inv_stocking", "UOM_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_stocking", "Site_id", "dbo.Inv_store_site");
            DropIndex("dbo.Inv_stocking", new[] { "Site_id" });
            DropIndex("dbo.Inv_stocking", new[] { "UOM_id" });
            DropColumn("dbo.Inv_stocking", "Transaction_date");
            DropColumn("dbo.Inv_stocking", "Orginal_amount");
            DropColumn("dbo.Inv_stocking", "Diffrance");
            DropColumn("dbo.Inv_stocking", "Adjust");
            DropColumn("dbo.Inv_stocking", "Exist");
            DropColumn("dbo.Inv_stocking", "Site_id");
            DropColumn("dbo.Inv_stocking", "UOM_id");
            CreateIndex("dbo.Inv_stocking", "Po_id");
            AddForeignKey("dbo.Inv_stocking", "Po_id", "dbo.Inv_receive_po", "Id", cascadeDelete: true);
        }
    }
}
