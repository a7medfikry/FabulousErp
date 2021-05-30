using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.FiscalPeriods
{
    public class FiscalDefandPeriods
    {
        public Fiscal_Year Fiscal_Year { get; set; }

        public List<Fiscal_Year> Fiscal_Year_List { get; set; }

        public Fiscal_Adjustment Fiscal_Adjustment { get; set; }

        public List<Fiscal_Adjustment> Fiscal_Adjustment_List { get; set; }

        public FiscalDefinition_Table FiscalDefinition_Table { get; set; }



    }
}
