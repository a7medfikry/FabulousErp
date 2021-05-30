using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Analytic
{
    public class B_AnalyticAccount_Table
    {

        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_AnalyticAccountID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string B_AnalyticAccountName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string BranchID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // one branch has many of analytic accounts
        public virtual CompanyBranchInfo_Table CompanyBranchInfo_Table { get; set; }


        // every analytic account has many of analytic distribution
        public ICollection<B_AnalyticDistribution_Table> B_AnalyticDistribution_Table { get; set; }

    }
}
