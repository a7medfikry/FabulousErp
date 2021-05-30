using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.GeneralSettings.FiscalPeriods
{
    public class Open_Close_Periods_DTO
    {
        public int ID { get; set; }

        public int Period_No { get; set; }

        public string Period_Start_Date { get; set; }

        public string Period_End_Date { get; set; }

        public ICollection<Fiscal_year_area> AreaNames { get; set; }
        //public bool? Financial { get; set; }
        //public bool? Purchasing { get; set; }
        //public bool? Sales { get; set; }
        //public bool? Inventory { get; set; }

    }
}
