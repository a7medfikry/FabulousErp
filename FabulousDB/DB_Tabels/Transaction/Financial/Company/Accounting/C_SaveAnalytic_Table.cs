using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Analytic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting
{
    public class C_SaveAnalytic_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_SA { get; set; }


        public int C_PostingNumber { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_AnalyticAccountID { get; set; }


        public int? C_DistID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string Describtion { get; set; }


        public double C_Amount { get; set; }


        public double C_Percentage { get; set; }


        public double? C_Debit { get; set; }


        public double? C_Credit { get; set; }


        public double Ballance { get; set; }


        public bool Post { get; set; }


        public int? C_AID { get; set; }



        public virtual C_GeneralJournalEntry_Table C_GeneralJournalEntry_Table { get; set; }

        public virtual C_AnalyticAccount_Table C_AnalyticAccount_Table { get; set; }

        public virtual C_AnalyticDistribution_Table C_AnalyticDistribution_Table { get; set; }

        public virtual C_CreateAccount_Table C_CreateAccount_Table { get; set; }


    }
}
