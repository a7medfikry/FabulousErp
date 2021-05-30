namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayableToPoRec : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Inv_receive_po", "Payable_id");
            AddForeignKey("dbo.Inv_receive_po", "Payable_id", "dbo.Payable_transaction", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po", "Payable_id", "dbo.Payable_transaction");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_receive_po", new[] { "Payable_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
