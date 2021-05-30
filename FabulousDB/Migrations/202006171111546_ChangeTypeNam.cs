namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTypeNam : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inv_stocking", "Transaction_date", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inv_stocking", "Transaction_date", c => c.DateTime(nullable: false));
        }
    }
}
