namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Delete_fixed_assets_revaluate");
            AlterColumn("dbo.Delete_fixed_assets_revaluate", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Delete_fixed_assets_revaluate", "Id");
            SqlFile(@"C:\FixedAssetsMigration.sql");
        }

        public override void Down()
        {
            DropPrimaryKey("dbo.Delete_fixed_assets_revaluate");
            AlterColumn("dbo.Delete_fixed_assets_revaluate", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Delete_fixed_assets_revaluate", new[] { "Id", "Delete_date" });
        }
    }
}
