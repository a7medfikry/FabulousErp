using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Analytic
{
    public class B_AnalyticDistribution_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int B_DistID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_AccountDistributionID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string B_AccountDistributionName { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_AnalyticAccountID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // every analytic account has many of analytic distribution
        public B_AnalyticAccount_Table B_AnalyticAccount_Table { get; set; }


        public virtual ICollection<B_CreatAccountDist_Table> B_CreatAccountDist_Table { get; set; }

    }
}
