namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logistis : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Unit_of_measure", new[] { "Equivalante_id" });
            AlterColumn("dbo.Unit_of_measure", "Equivalante_id", c => c.Int());
            CreateIndex("dbo.Unit_of_measure", "Equivalante_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Unit_of_measure", new[] { "Equivalante_id" });
            AlterColumn("dbo.Unit_of_measure", "Equivalante_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Unit_of_measure", "Equivalante_id");
        }
    }
}
