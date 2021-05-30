using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation
{
    public class Company_Main_Info
    {
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Company ID required 5 characters")]
        [Required(ErrorMessage = "Company ID Is Required", AllowEmptyStrings = false)]
        public string CompanyID { get; set; }


        [Required(ErrorMessage = "Company Name Is Required", AllowEmptyStrings = false)]
        public string CompanyName { get; set; }


        //[Required(ErrorMessage = "Company Alies Is Required", AllowEmptyStrings = false)]
        public string CompanyAlies { get; set; }


        [Required(ErrorMessage = "Country Name Is Required", AllowEmptyStrings = false)]
        public string CountryName { get; set; }


        [Required(ErrorMessage = "Language Is Required", AllowEmptyStrings = false)]
        public string Language { get; set; }


        [Required(ErrorMessage = "Currency Is Required", AllowEmptyStrings = false)]
        public string Currency { get; set; }


        [Required(ErrorMessage = "Company Main Activity Is Required", AllowEmptyStrings = false)]
        public string CompanyMainActivity { get; set; }

        public bool Status { get; set; }

        public byte[] LogoByte { get; set; }

        public HttpPostedFileBase InputLogo { get; set; }

    }
}
