using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition
{
    public class FormateSetting_Table
    {
        [ForeignKey("CompanyMainInfo_Table")]
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string Currency { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string DecimalNation { get; set; }



        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(1)]
        public string DecimalNumber { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(5)]
        public string Prefix { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(5)]
        public string Suffix { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(1)]
        public string Thousands { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(1)]
        public string Decimal { get; set; }



        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // one currency to one company
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }
    }
}
