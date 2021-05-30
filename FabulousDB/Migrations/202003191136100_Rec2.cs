namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec2 : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
