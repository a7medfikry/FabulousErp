namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteUniqeCheckBook : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.C_CheckBookSetting_table", new[] { "C_AID" });
            CreateIndex("dbo.C_CheckBookSetting_table", "C_AID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.C_CheckBookSetting_table", new[] { "C_AID" });
            CreateIndex("dbo.C_CheckBookSetting_table", "C_AID", unique: true);
        }
    }
}
