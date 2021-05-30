using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Analytic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account
{
    public class C_CreatAccountDist_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        public int C_AID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_AnalyticAccountID { get; set; }


        public int? C_DistID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string Percentage { get; set; }


        public virtual C_CreateAccount_Table C_CreateAccount_Table { get; set; }


        public virtual C_AnalyticAccount_Table C_AnalyticAccount_Table { get; set; }


        public virtual C_AnalyticDistribution_Table C_AnalyticDistribution_Table { get; set; }
    }
}
