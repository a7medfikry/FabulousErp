namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostriData : DbMigration
    {
        public override void Up()
        {
            Sql("Update Inv_receive_po set Transaction_date=R.Transaction_date , Posting_date=R.Posting_date from Receivable_transaction R where R.Id=Inv_receive_po.Receivable_id");

            Sql("Update Inv_receive_po set Transaction_date = P.Transaction_date, Posting_date = P.Posting_date from Payable_transaction P where P.Id = Inv_receive_po.Payable_id");

            Sql("Update Inv_sales_invoice set Transaction_date = R.Transaction_date, Posting_date = R.Posting_date from Receivable_transaction R where R.Id = Inv_sales_invoice.Receivable_id");

            Sql("Update Inv_sales_invoice set Transaction_date = P.Transaction_date, Posting_date = P.Posting_date from Payable_transaction P where P.Id = Inv_sales_invoice.Payable_id");
        }
        
        public override void Down()
        {
        }
    }
}
