using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition
{
    public class CurrenciesDefinition_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ISOCode { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string DisActive { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Currency_unit_name { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Currency_small_unit_name { get; set; }


        //public int? C_AID { get; set; }


        // one company have many of currency
        public CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }


        // currency definition has many of currency Exchange
        public ICollection<CurrenciesExchange_Table> CurrenciesExchange_Table { get; set; }

        // currency definition has many of Checkbook
        public ICollection<C_CheckBookSetting_table> C_CheckBookSetting_Tables { get; set; }
        public ICollection<B_CheckBookSetting_table> B_CheckBookSetting_Tables { get; set; }
        public ICollection<F_CheckBookSetting_table> F_CheckBookSetting_Tables { get; set; }


        //public virtual C_CreateAccount_Table C_CreateAccount_Table { get; set; }

        public virtual ICollection<CurrenciesDefinition_Tables> AccountCurrencyDefinition_Tables { get; set; }

        public virtual ICollection<B_AccountCurrencyDefinition_Table> B_AccountCurrencyDefinition_Tables { get; set; }

        public virtual ICollection<F_AccountCurrencyDefinition_Table> F_AccountCurrencyDefinition_Tables { get; set; }



        public virtual ICollection<C_CurrencyCreateAccount_Table> C_CurrencyCreateAccount_Tables { get; set; }

        public virtual ICollection<B_CurrencyCreateAccount_Table> B_CurrencyCreateAccount_Tables { get; set; }

        public virtual ICollection<F_CurrencyCreateAccount_Table> F_CurrencyCreateAccount_Tables { get; set; }


        public virtual ICollection<C_GeneralJournalEntry_Table> C_GeneralJournalEntry_Tables { get; set; }

        public ICollection<C_CheckbookTransactions_table> C_CheckbookTransactions_Tables { get; set; }

    }
}
