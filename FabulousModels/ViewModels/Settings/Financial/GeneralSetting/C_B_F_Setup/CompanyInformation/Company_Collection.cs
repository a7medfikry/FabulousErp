using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation
{
    public class Company_Collection
    {
        public Company_Main_Info Company_Main_Info { get; set; }

        public Company_Legal_Info Company_Legal_Info { get; set; }

        public Communication_Info Communication_Info { get; set; }

        public Address_Information Address_Information { get; set; }
    }
}
