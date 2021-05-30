namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvChangeType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Unit_of_measure", "Equivalante_quantity", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Unit_of_measure", "Equivalante_quantity", c => c.Int(nullable: false));
        }
    }
}
