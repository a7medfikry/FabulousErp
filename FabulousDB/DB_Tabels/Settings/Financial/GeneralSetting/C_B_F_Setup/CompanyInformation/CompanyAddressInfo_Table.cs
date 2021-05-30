using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation
{
    public class CompanyAddressInfo_Table
    {
        [Key,ForeignKey("CompanyMainInfo_Table")]
        [Column(TypeName = "nvarchar") ,MaxLength(10)]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar") , MaxLength(50)]
        public string StreetName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string BuldingNo { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FloorNo { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Area { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string City { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Governorate { get; set; }


        // one To one From Main Company
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }
    }
}
