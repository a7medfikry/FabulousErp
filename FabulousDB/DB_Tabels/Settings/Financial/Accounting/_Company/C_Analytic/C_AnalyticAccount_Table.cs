using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Analytic
{
    public class C_AnalyticAccount_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_AnalyticAccountID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string C_AnalyticAccountName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }



        //one company has many of Analytic Accounts
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }


        // every analytic account has many of analytic distribution
        public ICollection<C_AnalyticDistribution_Table> C_AnalyticDistribution_Table { get; set; }


        public virtual ICollection<C_CreatAccountDist_Table> C_CreatAccountDist_Table { get; set; }



        public virtual ICollection<C_SaveAnalytic_Table> C_SaveAnalytic_Tables { get; set; }
    }
}
