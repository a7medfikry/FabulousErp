using FabulousDB.DB_Tabels.Important;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Post;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccess;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount
{
    public class CreateAccount_Table
    {

        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int ID { get; set; }

        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string UserID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        [Required]
        public string UserName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(60)]
        [Required]
        public string Password { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        [Required]
        public string Date { get; set; }

        //[Column(TypeName = "nvarchar"), MaxLength(50)]
        //public string GroupID { get; set; }

        public Nullable<bool> ChangePassFirst { get; set; }


        public Nullable<bool> UpdateProfFirst { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string TitlePER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string NationalORPassportIDPER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        public string FirstNamePER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        public string LastNamePER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        public string FamilyNamePER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string StreetPER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        public string BuldingNoPER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string AvenuePER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string StatePER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CountryPER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CityPER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string HomePhonePER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MobilePhonePER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string OthMobilePhonePER { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string PositionFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string DepartmentFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        public string RoomNumFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        public string FloorFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        public string BuildingFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        public string TelephoneNumFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string TEXtentionFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FaxNumFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string FExtentionFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MobilePhoneFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string EmailFUN { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string LastPasswordChangedDate { get; set; }

        public Nullable<bool> PasswordExpired { get; set; }


        public Nullable<bool> DisActive { get; set; }


        public Nullable<bool> Deleted { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(60)]
        public string oldPassword { get; set; }


        public byte[] ProfilePicByte { get; set; }

        
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string DateOfAssignGroup { get; set; }


        //many to many
        public virtual ICollection<UACompPremission_Table> UACompPremission_Table { get; set; }

        //many to many
        public virtual ICollection<UABranchPremission_Table> UABranchPremission_Table { get; set; }

        //many to many
        public virtual ICollection<UAFactoryPremission_Table> UAFactoryPremission_Table { get; set; }


        //User Has one Group
        public virtual CreateGroup_Table CreateGroup_Table { get; set; }

        // 2public AccountGroup_Table AccountGroup_Table { get; set; }

        public virtual ICollection<User_Post_Table> User_Post_Table { get; set; }


        // C_CashRecipt Transactions
        public virtual ICollection<C_CheckbookTransactions_table> C_CheckbookTransactions_Tables { get; set; }
        public virtual ICollection<C_BankReconcile_table> C_BankReconcile_Tables { get; set; }

        public virtual ICollection<C_GeneralJournalEntry_Table> C_GeneralJournalEntry_Tables { get; set; }


        public virtual ICollection<C_CreateBatch_Table> C_CreateBatch_Tables { get; set; }

        public virtual ICollection<C_UserBatchApproval_Table> C_UserBatchApproval_Tables { get; set; }



        public virtual ICollection<UserFormsAccess_Table> UserFormsAccess_Tables { get; set; }

        public virtual UserGroup_Table UserGroup_Table { get; set; }

        public virtual ICollection<FavouritesForms_Table> FavouritesForms_Tables { get; set; }

    }
}
