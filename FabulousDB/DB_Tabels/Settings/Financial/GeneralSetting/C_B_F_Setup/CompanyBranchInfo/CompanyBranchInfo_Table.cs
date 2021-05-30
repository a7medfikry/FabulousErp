using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CostCenter;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccess;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using FabulousDB.DB_Tabels.Transaction.Financial.Branch.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo
{
    public class CompanyBranchInfo_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string BranchID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string BranchName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string BranchAlies { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string EstablishDate { get; set; }

        public bool? Status { get; set; }


        //one to one 'Main Branch to his legal info'
        public virtual BranchLegalInfo_Table BranchLegalInfo_Table { get; set; }

        //one to one 'Main Branch to his Address info'
        public virtual BranchAddressInfo_Table BranchAddressInfo_Table { get; set; }

        //one to one 'Main Branch to his Communication info'
        public virtual BranchCommInfo_Table BranchCommInfo_Table { get; set; }

        //one to Many 'branch has one Company'
        public CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }

        //one to Many 'branch Has a collection of Factories'
        public ICollection<CompanyFactoryInfo_Table> CompanyFactoryInfo_Table { get; set; }
        public ICollection<UAFactoryPremission_Table> UAFactoryPremission_Table { get; set; }


        //many to many
        public ICollection<UABranchPremission_Table> UABranchPremission_Table { get; set; }



        // one branch has many of analytic accounts
        public virtual ICollection<B_AnalyticAccount_Table> B_AnalyticAccount_Table { get; set; }


        // one branch has many of cost center
        public virtual ICollection<B_CostCenter_Table> B_CostCenter_Table { get; set; }


        // one branch has many of Main cost center
        public virtual ICollection<B_MainCostCenter_Table> B_MainCostCenter_Table { get; set; }


        public virtual ICollection<B_CreateAccount_Table> B_CreateAccount_Table { get; set; }


        public virtual ICollection<B_CreateBatch_Table> B_CreateBatch_Table { get; set; }


        public virtual ICollection<B_EditTRate> B_EditTRate { get; set; }
        public virtual ICollection<B_CheckBookSetting_table> B_CheckBookSetting_Tables { get; set; }


    }
}
