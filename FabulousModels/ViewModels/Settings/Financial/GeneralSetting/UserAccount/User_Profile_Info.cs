using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.UserAccount
{
    public class User_Profile_Info
    {
        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string TitlePER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string NationalORPassportIDPER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "letters only")]
        public string FirstNamePER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "letters only")]
        public string LastNamePER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "letters only")]
        public string FamilyNamePER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string StreetPER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string BuldingNoPER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string AvenuePER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string StatePER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string CountryPER { get; set; }

        [Required]
        public string CityPER { get; set; }

        public string HomePhonePER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string MobilePhonePER { get; set; }


        public string OthMobilePhonePER { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string PositionFUN { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string DepartmentFUN { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string RoomNumFUN { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string FloorFUN { get; set; }


        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string BuildingFUN { get; set; }


        public string TelephoneNumFUN { get; set; }


        public string TEXtentionFUN { get; set; }


        public string FaxNumFUN { get; set; }


        public string FExtentionFUN { get; set; }


        public string MobilePhoneFUN { get; set; }


        [EmailAddress(ErrorMessage = "Invalid Email!")]
        public string EmailFUN { get; set; }


        public HttpPostedFileBase InputPersonalImg { get; set; }

    }
}
