namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Installment2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receivable_payment", "Installment_id", x => x.Int(nullable: true));
            AddForeignKey("dbo.Receivable_payment", "Installment_id",
                "dbo.Installment_setting", "Id", cascadeDelete: false);

            AddColumn("dbo.Payable_payment", "Installment_id", x => x.Int(nullable: true));
            AddForeignKey("dbo.Payable_payment", "Installment_id",
                "dbo.Installment_setting", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
        }
    }
}
