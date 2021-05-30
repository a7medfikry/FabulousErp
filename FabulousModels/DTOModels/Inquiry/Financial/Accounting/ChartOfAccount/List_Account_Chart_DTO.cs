using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Inquiry.Financial.Accounting.ChartOfAccount
{
    public class List_Account_Chart_DTO
    {
        public string AccountChartID { get; set; }

        public string AccountChartName { get; set; }

        public string EstablishDate { get; set; }

        public bool? Active { get; set; }
    }
}
