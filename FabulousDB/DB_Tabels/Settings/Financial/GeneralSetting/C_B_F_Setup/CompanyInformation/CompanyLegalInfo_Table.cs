using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation
{
    public class CompanyLegalInfo_Table
    {
        [ForeignKey("CompanyMainInfo_Table")]
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CompanyType { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string EstablishDate { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CommericalRegister { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CommericalOffice { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string TaxFileNo { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string TaxOffice { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string VatID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string TaxVaOffice { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string ImporterID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string ImportOffice { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string ExportID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string ExportOffice { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string SocialInsuranceID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string SocialInsuranceOffice { get; set; }

        public string CommericalRegisterPath { get; set; }

        public string CommericalRegisterName { get; set; }

        public string TaxFilePath { get; set; }

        public string TaxFileName { get; set; }

        public string VatIDPath { get; set; }

        public string VatIDName { get; set; }

        public string ImportIDPath { get; set; }

        public string ImportIDName { get; set; }

        public string ExportIDPath { get; set; }

        public string ExportIDName { get; set; }


        public string InsuranceIDPath { get; set; }

        public string InsuranceIDName { get; set; }


        // one To one From Main Company
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }
    }
}
