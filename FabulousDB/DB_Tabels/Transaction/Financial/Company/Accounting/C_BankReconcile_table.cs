using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting
{
    public class C_BankReconcile_table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankReconcile_ID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }

        public int? C_CBSID { get; set; }

        public int BankReconcile_Number { get; set; }

        public decimal Bank_Statment_Ending_Balance { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string Bank_Statment_Ending_Date { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string Book_Statment_Ending_Date { get; set; }

        public bool? Reconciled { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string UserID { get; set; }

        public DateTime BankReconcile_DateTime { get; set; }







        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }
        public virtual C_CheckBookSetting_table C_CheckBookSetting_Table { get; set; }
        public virtual CreateAccount_Table CreateAccount_Table { get; set; }



    }
}
