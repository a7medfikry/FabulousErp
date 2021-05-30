using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Important
{
    public class Get_Small_Data_DTO
    {
        public string Name { get; set; }

        public string AnalyticID { get; set; }

        public string CostCenterID { get; set; }

        public string CostCenterGroupID { get; set; }

        public string CompanyID { get; set; }

        public string Message { get; set; }

        public double? From { get; set; }

        public double? To { get; set; }

        public string FromWithSep { get; set; }

        public string ToWithSep { get; set; }
    }
}
