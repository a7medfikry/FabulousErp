namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec8 : DbMigration
    {
        public override void Up()
        {
          //  DropForeignKey("dbo.Receivable_payment", "Transaction_p_id", "dbo.Receivable_transactions_types");
           // DropForeignKey("dbo.Receivable_payment", "Receivable_transactions_types_Id", "dbo.Receivable_transactions_types");
           // DropIndex("dbo.Receivable_payment", new[] { "Trans_doc_type_id" });
         //   DropIndex("dbo.Receivable_payment", new[] { "Receivable_transactions_types_Id" });
          // DropColumn("dbo.Receivable_payment", "Trans_doc_type_id");
           // RenameColumn(table: "dbo.Receivable_payment", name: "Receivable_transactions_types_Id", newName: "Trans_doc_type_id");
            //AlterColumn("dbo.Receivable_payment", "Trans_doc_type_id", c => c.Int(nullable: false));
            //CreateIndex("dbo.Receivable_payment", "Trans_doc_type_id");
            //AddForeignKey("dbo.Receivable_payment", "Transaction_p_id", "dbo.Receivable_transaction", "Id");
            //AddForeignKey("dbo.Receivable_payment", "Trans_doc_type_id", "dbo.Receivable_transactions_types", "Id", cascadeDelete: false);


          //  DropForeignKey("dbo.Payable_payment", "Transaction_p_id", "dbo.Payable_transactions_types");
        //    DropForeignKey("dbo.Payable_payment", "Payable_transactions_types_Id", "dbo.Payable_transactions_types");
            //DropIndex("dbo.Payable_payment", new[] { "Trans_doc_type_id" });
          //  DropIndex("dbo.Payable_payment", new[] { "Payable_transactions_types_Id" });
          //  DropColumn("dbo.Payable_payment", "Trans_doc_type_id");
         //   DropColumn("dbo.Payable_payment", "Payable_transactions_types_Id");
        //    AlterColumn("dbo.Payable_payment", "Trans_doc_type_id", c => c.Int(nullable: false));
        //    CreateIndex("dbo.Payable_payment", "Trans_doc_type_id");
        //    AddForeignKey("dbo.Payable_payment", "Transaction_p_id", "dbo.Payable_transaction", "Id");
        //    AddForeignKey("dbo.Payable_payment", "Trans_doc_type_id", "dbo.Payable_transactions_types", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receivable_payment", "Trans_doc_type_id", "dbo.Receivable_transactions_types");
            DropForeignKey("dbo.Receivable_payment", "Transaction_p_id", "dbo.Receivable_transaction");
            DropIndex("dbo.Receivable_payment", new[] { "Trans_doc_type_id" });
            AlterColumn("dbo.Receivable_payment", "Trans_doc_type_id", c => c.Int());
            RenameColumn(table: "dbo.Receivable_payment", name: "Trans_doc_type_id", newName: "Receivable_transactions_types_Id");
            AddColumn("dbo.Receivable_payment", "Trans_doc_type_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Receivable_payment", "Receivable_transactions_types_Id");
            CreateIndex("dbo.Receivable_payment", "Trans_doc_type_id");
            AddForeignKey("dbo.Receivable_payment", "Receivable_transactions_types_Id", "dbo.Receivable_transactions_types", "Id");
            AddForeignKey("dbo.Receivable_payment", "Transaction_p_id", "dbo.Receivable_transactions_types", "Id");
        }
    }
}
