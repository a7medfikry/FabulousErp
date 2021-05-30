using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Analytic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account
{
    public class F_CreatAccountDist_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        public int F_AID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_AnalyticAccountID { get; set; }


        public int? F_DistID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string Percentage { get; set; }


        public virtual F_CreateAccount_Table F_CreateAccount_Table { get; set; }


        public virtual F_AnalyticAccount_Table F_AnalyticAccount_Table { get; set; }


        public virtual F_AnalyticDistribution_Table F_AnalyticDistribution_Table { get; set; }
    }
}
