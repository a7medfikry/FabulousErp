using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation
{
    public class Company_Legal_Info
    {

        [Required(ErrorMessage = "Company Type Is Required", AllowEmptyStrings = false)]
        public string CompanyType { get; set; }


        public string EstablishDate { get; set; }


        [Required(ErrorMessage = "Commerical Register Is Required", AllowEmptyStrings = false)]
        public string CommericalRegister { get; set; }


        public string CommericalOffice { get; set; }


        [Required(ErrorMessage = "Tax File Number Is Required", AllowEmptyStrings = false)]
        public string TaxFileNo { get; set; }


        public string TaxOffice { get; set; }



        public string VatID { get; set; }



        public string TaxVaOffice { get; set; }



        public string ImporterID { get; set; }



        public string ImportOffice { get; set; }



        public string ExportID { get; set; }



        public string ExportOffice { get; set; }



        public string SocialInsuranceID { get; set; }



        public string SocialInsuranceOffice { get; set; }


        public HttpPostedFileBase InputCommerical { get; set; }



        public HttpPostedFileBase InputTax { get; set; }



        public HttpPostedFileBase InputVat { get; set; }


        public HttpPostedFileBase InputImport { get; set; }



        public HttpPostedFileBase InputExport { get; set; }



        public HttpPostedFileBase InputInsurance { get; set; }

        // public byte[] CommericalRegisterImg { get; set; }

        //  public byte[] TaxFileImg { get; set; }

        //   public byte[] VatIDImg { get; set; }

        //   public byte[] ImportIDImg { get; set; }

        //  public byte[] ExportIDImg { get; set; }


    }
}
