namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewExpiry : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Inv_receive_expiery", newName: "Inv_receive_expiry");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Inv_receive_expiry", newName: "Inv_receive_expiery");
        }
    }
}
