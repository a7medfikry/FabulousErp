using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.Accounting
{
    public class Checkbook_DTO
    {
        public string CurrencyID { get; set; }
        public string Company_AccountsID { get; set; }
        public string CurrencyName { get; set; }
        public string CheckbookID { get; set; }
        public string CheckbookName { get; set; }
        public string CheckbookType { get; set; }
        public bool? Status { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public double? MaxAmount { get; set; }
        public double? MinAmount { get; set; }
        public int? NextWithDraw { get; set; }
        public int? NextDeposit { get; set; }
        public double? CurrentCheckbookBalance { get; set; }
        public double? CurrentCashBalance { get; set; }
        public double? LastReconsileBalance { get; set; }
        public double? LastReconsileDate { get; set; }
        public string BankName { get; set; }
        public int? BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string BranchName { get; set; }
        public string SwiftCode { get; set; }
        public string IBAN { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string Branch_AccountsID { get; set; }
        public string Factory_AccountsID { get; set; }


        public double cashAccountBalance { get; set; }


    }
}
