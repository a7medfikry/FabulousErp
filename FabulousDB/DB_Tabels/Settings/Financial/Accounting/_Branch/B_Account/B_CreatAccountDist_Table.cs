using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Analytic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account
{
    public class B_CreatAccountDist_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        public int B_AID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_AnalyticAccountID { get; set; }


        public int? B_DistID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string Percentage { get; set; }



        public virtual B_CreateAccount_Table B_CreateAccount_Table { get; set; }


        public virtual B_AnalyticAccount_Table B_AnalyticAccount_Table { get; set; }


        public virtual B_AnalyticDistribution_Table B_AnalyticDistribution_Table { get; set; }
    }
}
