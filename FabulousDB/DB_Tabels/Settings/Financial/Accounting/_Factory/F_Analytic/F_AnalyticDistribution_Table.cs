using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Analytic
{
    public class F_AnalyticDistribution_Table
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int F_DistID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_AccountDistributionID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string F_AccountDistributionName { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_AnalyticAccountID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // every analytic account has many of analytic distribution
        public F_AnalyticAccount_Table F_AnalyticAccount_Table { get; set; }


        public virtual ICollection<F_CreatAccountDist_Table> F_CreatAccountDist_Table { get; set; }
    }
}
