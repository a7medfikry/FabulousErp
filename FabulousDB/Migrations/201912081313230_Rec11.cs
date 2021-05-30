namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec11 : DbMigration
    {
        public override void Up()
        {
            DropTable("Related_rec_trans");
            DropTable("Related_pay_trans");
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

            AddForeignKey("Related_rec_trans", "Transaction_id", "Receivable_transactions_types", "Id");
            AddForeignKey("Related_rec_trans", "Payment_id", "Receivable_transactions_types", "Id");

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
            AddForeignKey("Related_pay_trans", "Transaction_id", "Payable_transactions_types", "Id");
            AddForeignKey("Related_pay_trans", "Payment_id", "Payable_transactions_types", "Id");
        }
        
        public override void Down()
        {
        }
    }
}
