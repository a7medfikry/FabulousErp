using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo
{
    public class Branch_Collections
    {
        public Branch_Main_Info Branch_Main_Info { get; set; }
        
        public Branch_legal_Info Branch_Legal_Info { get; set; }

        public Communication_Info Communication_Info { get; set; }

        public Address_Information Address_Information { get; set; }
        
    }
}
