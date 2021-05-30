using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup
{
    public class Address_Information
    {
        public string SpecificId { get; set; }


        [Required(ErrorMessage = "Street Name Is Required", AllowEmptyStrings = false)]
        public string StreetName { get; set; }


        [Required(ErrorMessage = "Building Number Is Required", AllowEmptyStrings = false)]
        public string BuldingNo { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Floor Number must be numeric")]
        public string FloorNo { get; set; }


        public string Area { get; set; }


        public string City { get; set; }


        [Required(ErrorMessage = "Governorate/State Is Required", AllowEmptyStrings = false)]
        public string Governorate { get; set; }
    }
}
