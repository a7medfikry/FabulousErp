namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStokingName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_stocking", "Orginal_qty", c => c.Single(nullable: false));
            DropColumn("dbo.Inv_stocking", "Orginal_amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_stocking", "Orginal_amount", c => c.Single(nullable: false));
            DropColumn("dbo.Inv_stocking", "Orginal_qty");
        }
    }
}
