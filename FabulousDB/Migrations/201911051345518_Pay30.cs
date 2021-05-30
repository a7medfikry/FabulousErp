namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay30 : DbMigration
    {
        public override void Up()
        {
            Sql("delete from Payable_payment");
            AddColumn("dbo.Payable_payment", "Trans_doc_type_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_payment", "Trans_doc_type_id");
            AddForeignKey("dbo.Payable_payment", "Trans_doc_type_id", "dbo.Payable_transactions_types", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_payment", "Trans_doc_type_id", "dbo.Payable_transactions_types");
            DropIndex("dbo.Payable_payment", new[] { "Trans_doc_type_id" });
            DropColumn("dbo.Payable_payment", "Trans_doc_type_id");
        }
    }
}
