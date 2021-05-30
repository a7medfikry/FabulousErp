namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay75 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Payable_payment", name: "nvarchar", newName: "Reference");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Payable_payment", name: "Reference", newName: "nvarchar");
        }
    }
}
