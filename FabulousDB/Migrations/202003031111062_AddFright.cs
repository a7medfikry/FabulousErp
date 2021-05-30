namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFright : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AddColumn("dbo.Inv_item", "Validation_method", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_item_group", "Validation_method", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_receive_po_items", "Fright", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Inv_item", "Valudation_method");
            DropColumn("dbo.Inv_item_group", "Valudation_method");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_item_group", "Valudation_method", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_item", "Valudation_method", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_receive_po_items", "Fright");
            DropColumn("dbo.Inv_item_group", "Validation_method");
            DropColumn("dbo.Inv_item", "Validation_method");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
