using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.Accounting.CostCenter
{
    public class Main_Cost_Center_DTO
    {
        public int GroupID { get; set; }


        public string CostCenterID { get; set; }


        public string CostCenterName { get; set; }


        public string Percentage { get; set; }


        public string MainCostCenterID { get; set; }

        public string MainCostCenterName { get; set; }
    }
}
