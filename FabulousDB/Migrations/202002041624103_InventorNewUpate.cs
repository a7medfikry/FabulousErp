namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventorNewUpate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inv_po", "Pr_no_id", c => c.Int(nullable:true));

        }

        public override void Down()
        {
        }
    }
}
