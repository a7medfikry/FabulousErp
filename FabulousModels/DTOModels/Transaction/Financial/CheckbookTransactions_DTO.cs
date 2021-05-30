using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Transaction.Financial
{
    public class CheckbookTransactions_DTO
    {
        // Identity ID
        public int C_CBT { get; set; }

        public string CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CheckbookName { get; set; }
        public string Company_AccountsID { get; set; }
        public string Currency_AccountsID { get; set; }
        public string Company_AccountsName { get; set; }
        public string CB_Password { get; set; }
        public string CB_UserID { get; set; }
        public int C_AID { get; set; }
        public double Rate { get; set; }
        public double? MinAmount { get; set; }
        public double? MaxAmount { get; set; }
        public decimal? Balance { get; set; }
        public int NextDepositNumber { get; set; }
        public int NextWithdrawNumber { get; set; }
        public int? C_CBTVoid { get; set; }

        /////////////////////////////////////////////////////////
        public string Date { get; set; }
        public int DocumentNumber { get; set; }
        public int PostingNumber { get; set; }
        public string CheckNumber { get; set; }
        public string DocumentType { get; set; }
        public decimal? Payment { get; set; }
        public decimal? Deposit { get; set; }
        public decimal CashAccountBalance { get; set; }
        public double CashAccountBalance2 { get; set; }

        /////////////////////////////////////////////////////////

        public string AccountType { get; set; }

        /////////////////////////////////////////////////////////
        public int? C_CBSID { get; set; }
        public string Checkbook_ID { get; set; }
        public string Checkbook_Type { get; set; }
        public int BankReconcile_Number { get; set; }
        public decimal Bank_Statment_Ending_Balance { get; set; }
        public string Bank_Statment_Ending_Date { get; set; }
        public string Book_Statment_Ending_Date { get; set; }
        public bool? ReconcileStatus { get; set; }

        ///////////////////////////////////////////////////////////
        public string RecievedFrom { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }

        public string PostingKey { get; set; }
        public string Refrence { get; set; }
    }
}
