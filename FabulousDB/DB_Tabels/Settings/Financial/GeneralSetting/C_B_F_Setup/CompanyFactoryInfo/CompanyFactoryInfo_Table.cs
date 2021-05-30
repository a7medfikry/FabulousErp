using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CostCenter;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccess;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using FabulousDB.DB_Tabels.Transaction.Financial.Factory.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo
{
    public class CompanyFactoryInfo_Table
    {
        //public int ID { get; set; }

        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string FactoryID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FactoryName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FactoryAlies { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string BranchID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string EstablishDate { get; set; }

        public bool? Status { get; set; }


        //one to one 'Main Factory to his Legal Info'
        public virtual FactoryLegalInfo_Table FactoryLegalInfo_Table { get; set; }

        //one to one 'Main Factory to his Address Info'
        public virtual FactoryAddressInfo_Table FactoryAddressInfo_Table { get; set; }

        //one to one 'Main Factory to his Communication Info'
        public virtual FactoryCommInfo_Table FactoryCommInfo_Table { get; set; }

        //one to many 'Factory has one Company'
        public CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }

        //one to many 'Factory Has one Branch'
        public CompanyBranchInfo_Table CompanyBranchInfo_Table { get; set; }


        //many to many
        public ICollection<UAFactoryPremission_Table> uAFactoryPremission_Tables { get; set; }



        // one factory has many of analytic accounts
        public virtual ICollection<F_AnalyticAccount_Table> F_AnalyticAccount_Tables { get; set; }



        // one factory has many of cost center
        public virtual ICollection<F_CostCenter_Table> F_CostCenter_Tables { get; set; }


        // one factory has many of main cost center
        public virtual ICollection<F_MainCostCenter_Table> F_MainCostCenter_Tables { get; set; }


        public virtual ICollection<F_CreateAccount_Table> F_CreateAccount_Table { get; set; }


        public virtual ICollection<F_CreateBatch_Table> F_CreateBatch_Table { get; set; }


        public virtual ICollection<F_EditTRate> F_EditTRate { get; set; }

        public virtual ICollection<F_CheckBookSetting_table> F_CheckBookSetting_Tables { get; set; }


    }
}
