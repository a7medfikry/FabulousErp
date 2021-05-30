using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.Accounting.CreateChartOfAccount
{
    public class Chart_Group_Content_DTO
    {
        public string AccountGroupID { get; set; }

        public string AccountGroupName { get; set; }


        public double? AccountFrom { get; set; }




        public double? AccountTo { get; set; }


        public string AccountName { get; set; }


        public string AccountFromWithSep { get; set; }


        public string AccountToWithSep { get; set; }
    }
}
