using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Analytic
{
    public class F_AnalyticAccount_Table
    {

        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_AnalyticAccountID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string F_AnalyticAccountName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string FactoryID { get; set; }



        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }



        // one factory has many of analytic accounts
        public virtual CompanyFactoryInfo_Table CompanyFactoryInfo_Table { get; set; }


        // every analytic account has many of analytic distribution
        public ICollection<F_AnalyticDistribution_Table> F_AnalyticDistribution_Table { get; set; }


        public virtual ICollection<F_CreatAccountDist_Table> F_CreatAccountDist_Table { get; set; }

    }
}
