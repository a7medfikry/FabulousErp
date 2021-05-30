using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.FiscalPeriods
{
    public class Company_Fiscal_Year
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Company ID Is Required")]
        public string CompanyID { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required(ErrorMessage ="Fiscal Year ID Is Required")]
        public string FiscalYearID { get; set; }
        
        [Required]
        public string FiscalYearName { get; set; }
    }
}
