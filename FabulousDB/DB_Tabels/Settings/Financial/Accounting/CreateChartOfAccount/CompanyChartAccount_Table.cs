using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount
{
    public class CompanyChartAccount_Table
    {
        [ForeignKey("CompanyMainInfo_Table")]
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string AccountChartID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // one To one From Main Company
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }


        // Account Chart can relation with many of company
        public virtual AccountChart_Table AccountChart_Table { get; set; }
    }
}
