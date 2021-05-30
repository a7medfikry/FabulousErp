namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Password_option", "Password", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Password_option", "Password");
        }
    }
}
