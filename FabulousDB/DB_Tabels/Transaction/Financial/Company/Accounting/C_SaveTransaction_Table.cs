using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting
{
    public class C_SaveTransaction_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_ST { get; set; }


        public int C_PostingNumber { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string C_Describtion { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(3)]
        public string C_Document { get; set; }


        public int? C_AID { get; set; }


        public double C_OriginalAmount { get; set; }


        public double? C_Debit { get; set; }


        public double? C_Credit { get; set; }


        public double Ballance { get; set; }



        public virtual C_GeneralJournalEntry_Table C_GeneralJournalEntry_Table { get; set; }

        public virtual C_CreateAccount_Table C_CreateAccount_Table { get; set; }

    }
}
