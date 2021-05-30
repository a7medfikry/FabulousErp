using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo
{
    public class BranchLegalInfo_Table
    {
        [Key, ForeignKey("CompanyBranchInfo_Table")]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string BranchID { get; set; }
       
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string InsuranceID { get; set; }
       
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string InsuranceOffice { get; set; }


        public virtual CompanyBranchInfo_Table CompanyBranchInfo_Table { get; set; }
    }
}
