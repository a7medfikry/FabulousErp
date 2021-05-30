namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvetoryNewForign3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inv_receive_po", "Doc_type", c => c.Int(nullable: true));
            AlterColumn("dbo.Inv_receive_po", "Batch_id", c => c.Int(nullable: true));

        }

        public override void Down()
        {
        }
    }
}
