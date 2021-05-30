namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContInst1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Payable_payment", "Installment_id", "Installment_setting");
            AddForeignKey("Payable_payment", "Installment_id", "Installments");
        }
        
        public override void Down()
        {
        }
    }
}
