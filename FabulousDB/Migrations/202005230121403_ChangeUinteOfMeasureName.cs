namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUinteOfMeasureName : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Unit_of_measure", "Quantity", "Equivalante_quantity");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Unit_of_measure", "Equivalante_quantity", "Quantity");

        }
    }
}
