using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup
{
    public class Communication_Info
    {
        public string SpecificId { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string International1 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Telephone1 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string TelephoneEX1 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string International2 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Telephone2 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string TelephoneEX2 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string International3 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Telephone3 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string TelephoneEX3 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string International4 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Telephone4 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string TelephoneEX4 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string International5 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Telephone5 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string TelephoneEX5 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Fax1 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string FaxEX1 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Fax2 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string FaxEX2 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Fax3 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string FaxEX3 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Fax4 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string FaxEX4 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Fax5 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string FaxEX5 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Code1 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Code2 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Code3 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Code4 { get; set; }


        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers Only Is Required")]
        public string Code5 { get; set; }


        public string Website { get; set; }

    }
}
