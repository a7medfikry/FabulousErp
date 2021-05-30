namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IventoryNew8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inv_po", "Po_type", c => c.Int(nullable:true));

        }

        public override void Down()
        {
         
        }
    }
}
