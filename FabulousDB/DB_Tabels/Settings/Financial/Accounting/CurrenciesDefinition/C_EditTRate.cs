﻿using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition
{
    public class C_EditTRate
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_ETRID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(11)]
        [Required]
        public string Module { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string TransactionFormName { get; set; }


        public bool AllowUserE { get; set; }


        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }
    }
}
