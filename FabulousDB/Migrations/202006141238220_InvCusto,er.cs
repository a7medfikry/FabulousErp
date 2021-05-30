namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvCustoer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_receive_po", "Customer_id", "dbo.Receivable_vendore_setting");
            DropIndex("dbo.Inv_receive_po", new[] { "Customer_id" });
            AlterColumn("dbo.Inv_receive_po", "Customer_id", c => c.Int());
            CreateIndex("dbo.Inv_receive_po", "Customer_id");
            AddForeignKey("dbo.Inv_receive_po", "Customer_id", "dbo.Receivable_vendore_setting", "Id");
        }
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po", "Customer_id", "dbo.Receivable_vendore_setting");
            DropIndex("dbo.Inv_receive_po", new[] { "Customer_id" });
            AlterColumn("dbo.Inv_receive_po", "Customer_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_receive_po", "Customer_id");
            AddForeignKey("dbo.Inv_receive_po", "Customer_id", "dbo.Receivable_vendore_setting", "Id", cascadeDelete: true);
        }
    }
}
