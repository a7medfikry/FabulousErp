using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo
{
    public class Branch_Main_Info
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Branch ID Required")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Branch ID required 7 characters")]
        public string BranchID { get; set; }

        [Required(ErrorMessage = "Branch Name Required")]
        public string Branchname { get; set; }

        public string BranchAlies { get; set; }

        [Required(ErrorMessage = "Company ID Required")]
        public string CompanyID { get; set; }

        public string EstablishDate { get; set; }

        public bool Status { get; set; }
    }
}
