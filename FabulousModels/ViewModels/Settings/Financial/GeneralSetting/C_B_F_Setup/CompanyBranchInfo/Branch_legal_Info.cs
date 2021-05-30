using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo
{
    public class Branch_legal_Info
    {
        public int ID { get; set; }

        public string InsuranceID { get; set; }

        public string InsuranceOffice { get; set; }
    }
}
