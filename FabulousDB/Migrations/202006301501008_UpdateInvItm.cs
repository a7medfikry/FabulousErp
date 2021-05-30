namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Security.Cryptography.X509Certificates;

    public partial class UpdateInvItm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item", "Inactive", c => c.Boolean(nullable: false,defaultValue:false,defaultValueSql:"0"));
            AddColumn("dbo.Inv_item", "Password", c => c.String());
            AddColumn("dbo.Inv_item", "Has_password", c => c.Boolean(nullable: false, defaultValue: false, defaultValueSql: "0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_item", "Has_password");
            DropColumn("dbo.Inv_item", "Password");
            DropColumn("dbo.Inv_item", "Inactive");
        }
    }
}
