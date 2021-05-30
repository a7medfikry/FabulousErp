using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Reports.Financial.Accounting
{
    public class Reports_DTO
    {
        public string YearName { get; set; }
        public int YearID { get; set; }
        public string First_Year_Start { get; set; }
        public string First_Year_End { get; set; }
        public int C_AID { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
    }
}
