namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po", "Creation_date", c => c.DateTime(nullable: false,defaultValueSql:"GetDate()"));
            AddColumn("dbo.Inv_sales_invoice", "Creation_date", c => c.DateTime(nullable: false, defaultValueSql: "GetDate()"));
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_sales_invoice", "Creation_date");
            DropColumn("dbo.Inv_receive_po", "Creation_date");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
