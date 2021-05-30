using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods
{
    public class CompanyFiscalYear_Table
    {
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
       
        
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Index(IsUnique = true)]
        [Required]
        public string CompanyID { get; set; }
        

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public String Fiscal_Year_ID { get; set; }


        //many to one To Main Company
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }

        //many to one To Main Fiscal definition
        public virtual FiscalDefinition_Table FiscalDefinition_Table { get; set; }
        
    }
}
