using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo
{
    public class FactoryCommInfo_Table
    {
        [Key, ForeignKey("CompanyFactoryInfo_Table")]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string FactoryID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string International1 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Telephone1 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string TelephoneEX1 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string International2 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Telephone2 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string TelephoneEX2 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string International3 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Telephone3 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string TelephoneEX3 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string International4 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Telephone4 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string TelephoneEX4 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string International5 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Telephone5 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string TelephoneEX5 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Fax1 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FaxEX1 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Fax2 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FaxEX2 { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Fax3 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FaxEX3 { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Fax4 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FaxEX4 { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Fax5 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FaxEX5 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Code1 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Code2 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Code3 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Code4 { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Code5 { get; set; }


        public virtual CompanyFactoryInfo_Table CompanyFactoryInfo_Table { get; set; }
    }
}
