using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.FiscalPeriods
{
    public class Fiscal_Definition
    {
        [Required(ErrorMessage = "This Field Is Required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Fiscal Year ID Is required 5 Characters")]
        public string Fiscal_Year_ID { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        public string Fiscal_Year_Name { get; set; }
        /*
        [Required(ErrorMessage = "This Field Is Required")]
        public string Fiscal_Year_Start { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        public string Fiscal_Year_End { get; set; }
        */
        [Required(ErrorMessage = "This Field Is Required")]
        public int Number_Of_Periods { get; set; }

        [Required(ErrorMessage = "This Field Is Required")]
        public int Number_Of_Adjustment_Periods { get; set; }
    }
}
