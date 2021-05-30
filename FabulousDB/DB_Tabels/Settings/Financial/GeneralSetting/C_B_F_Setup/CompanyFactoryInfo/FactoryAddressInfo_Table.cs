using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo
{
    public class FactoryAddressInfo_Table
    {
        [Key, ForeignKey("CompanyFactoryInfo_Table")]
        [Column(TypeName = "nvarchar") ,MaxLength(10)]
        public string FactoryID { get; set; }


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


        public virtual CompanyFactoryInfo_Table CompanyFactoryInfo_Table { get; set; }
    }
}
