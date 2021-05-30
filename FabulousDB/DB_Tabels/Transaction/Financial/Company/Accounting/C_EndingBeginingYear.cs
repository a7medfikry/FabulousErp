using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting
{
    public class C_EndingBeginingYear
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_EBYearID { get; set; }

        [ForeignKey("NewFiscalYear_Table")]
        public int YearID { get; set; }

        [ForeignKey("C_CreateAccount_Table")]
        public int C_AID { get; set; }

        public double? Debit { get; set; }

        public double? Credit { get; set; }

        public double Ballance { get; set; }

        public int Type { get; set; }

        public double? Adjustment { get; set; }

        //[ForeignKey("C_GeneralJournalEntry_Table")]
        //public int? C_PostingNumber { get; set; }

        public virtual NewFiscalYear_Table NewFiscalYear_Table { get; set; }

        public virtual C_CreateAccount_Table C_CreateAccount_Table { get; set; }

        //public virtual C_GeneralJournalEntry_Table C_GeneralJournalEntry_Table { get; set; }
    }
}
