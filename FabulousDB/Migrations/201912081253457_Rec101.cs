namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec101 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Related_rec_trans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Transaction_id = c.Int(nullable: false),
                        Payment_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                
                .Index(t => t.Transaction_id)
                .Index(t => t.Payment_id);

            AddForeignKey("Related_rec_trans", "Transaction_id", "Receivable_transaction", "Id");
            AddForeignKey("Related_rec_trans", "Payment_id", "Receivable_payment", "Id");

              CreateTable(
                "dbo.Related_pay_trans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Transaction_id = c.Int(nullable: false),
                        Payment_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
            
                .Index(t => t.Transaction_id)
                .Index(t => t.Payment_id);
            AddForeignKey("Related_pay_trans", "Transaction_id", "Payable_transaction", "Id");
            AddForeignKey("Related_pay_trans", "Payment_id", "Payable_payment", "Id");


        }

        public override void Down()
        {
            DropForeignKey("dbo.Related_pay_trans", "Transaction_id", "dbo.Receivable_transaction");
            DropForeignKey("dbo.Related_pay_trans", "Payment_id", "dbo.Receivable_payment");
            DropIndex("dbo.Related_pay_trans", new[] { "Payment_id" });
            DropIndex("dbo.Related_pay_trans", new[] { "Transaction_id" });
            DropTable("dbo.Related_pay_trans");
        }
    }
}
