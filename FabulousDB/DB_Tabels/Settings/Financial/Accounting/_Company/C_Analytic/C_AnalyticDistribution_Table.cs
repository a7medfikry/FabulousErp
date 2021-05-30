using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Analytic;
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
    public class C_AnalyticDistribution_Table
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_DistID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_AccountDistributionID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string C_AccountDistributionName { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_AnalyticAccountID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // every analytic account has many of analytic distribution
        public C_AnalyticAccount_Table C_AnalyticAccount_Table { get; set; }


        public virtual ICollection<C_CreatAccountDist_Table> C_CreatAccountDist_Table { get; set; }


        public virtual ICollection<C_SaveAnalytic_Table> C_SaveAnalytic_Tables { get; set; }
       
    }
}
