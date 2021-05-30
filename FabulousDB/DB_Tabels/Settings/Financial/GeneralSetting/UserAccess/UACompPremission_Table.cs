using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccess
{
    public class UACompPremission_Table
    {            
        [Key]
        [Column(Order = 1 ,TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }

        
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CompanyName { get; set; }

        [Key]
        [Column(Order = 2 ,TypeName = "nvarchar"), MaxLength(50)]
        public string UserID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string UserName { get; set; }

        //many to many
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }

        //many to many
        public virtual CreateAccount_Table CreateAccount_Table { get; set; }
    }
}
