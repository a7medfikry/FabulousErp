namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPriceList : DbMigration
    {
        public override void Up()
        {
          
            DropColumn("dbo.Inv_pricelist", "Desc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_pricelist", "Desc", c => c.String(maxLength: 500));
        }
    }
}
