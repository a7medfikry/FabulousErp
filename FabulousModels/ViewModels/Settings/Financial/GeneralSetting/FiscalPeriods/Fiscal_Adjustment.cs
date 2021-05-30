using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.FiscalPeriods
{
    public class Fiscal_Adjustment
    {
        public int ID { get; set; }

        public int? Fiscal_Year_ID { get; set; }

        public string Fiscal_Year_Name { get; set; }

        public int Period_No { get; set; }

        public string Period_Start_Date { get; set; }

        //public string Period_End_Date { get; set; }

        public string CheckDate { get; set; }
    }
}
