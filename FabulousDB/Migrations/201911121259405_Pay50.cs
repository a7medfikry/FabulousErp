namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay50 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assign_payable_doc", "Trans_doc_type_id_to", c => c.Int(nullable: false));
            CreateIndex("dbo.Assign_payable_doc", "Trans_doc_type_id_to");
            AddForeignKey("dbo.Assign_payable_doc", "Trans_doc_type_id_to", "dbo.Payable_transactions_types", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assign_payable_doc", "Trans_doc_type_id_to", "dbo.Payable_transactions_types");
            DropIndex("dbo.Assign_payable_doc", new[] { "Trans_doc_type_id_to" });
            DropColumn("dbo.Assign_payable_doc", "Trans_doc_type_id_to");
        }
    }
}
