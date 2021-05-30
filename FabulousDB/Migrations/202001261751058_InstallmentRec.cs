namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstallmentRec : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Receivable_payment", "Installment_id", "Installment_setting");
            AddForeignKey("Receivable_payment", "Installment_id", "Installments");
        }

        public override void Down()
        {
        }
    }
}
