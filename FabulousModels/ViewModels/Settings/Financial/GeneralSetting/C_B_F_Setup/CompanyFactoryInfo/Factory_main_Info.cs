using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo
{
    public class Factory_main_Info
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Factory ID Is required")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Factory ID required 9 characters")]
        public string FactoryID { get; set; }

        [Required(ErrorMessage = "Factory Name Is Required")]
        public string FactoryName { get; set; }

        public string FactoryAlies { get; set; }

        public string BranchID { get; set; }

        [Required(ErrorMessage = "Company ID Is Required")]
        public string CompanyID { get; set; }

        public string EstablishDate { get; set; }

        public bool Status { get; set; }

    }
}
