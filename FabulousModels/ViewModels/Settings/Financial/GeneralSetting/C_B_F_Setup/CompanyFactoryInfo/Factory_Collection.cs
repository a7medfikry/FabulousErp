using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo
{
    public class Factory_Collection
    {
        public Factory_main_Info Factory_Main_Info { get; set; }

        public Factory_Legal_Info Factory_Legal_Info { get; set; }

        public Communication_Info Communication_Info { get; set; }

        public Address_Information Address_Information { get; set; }
    }
}
