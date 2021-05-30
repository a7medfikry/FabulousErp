using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup
{
    public class AccountGroup_Table
    {
        [ForeignKey("CreateAccount_Table")]
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string UserID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string GroupID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCL { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCBI { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCPI { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCNA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SLOU { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCNG { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SLOG { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SAGI { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SUACP { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SUABP { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SUAFP { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SUAP { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SFYD { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCFY { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCNY { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCF { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCD { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCET { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ILOU { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCCOA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ICI { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IUP { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ISFP { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SACTC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ILCOA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCAA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCAAD { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SBAA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SFAA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SBCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SFCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCMCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SUMCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCCCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCAG { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCY { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SBAAD { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SFAAD { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SBMCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SFMCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SUBMCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SUFMCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SBCCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SFCCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SBUAP { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SFUAP { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IUFA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IBA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ICA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IFA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IGA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SDATCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCATCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IBAA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IBCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ICAA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ICCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ILOC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IFAA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IFCC { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SBCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SDATBA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCATBA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SFCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SDATFA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SCATFA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SOCP { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SUP { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SPS { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IBADA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IBCCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ICADA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SPD { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ICCCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IFADA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string IFCCA { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string TCCB { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string TBCB { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string TFCB { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string SUETR { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string TCGE { get; set; }


        //User Hasne o Group
        //public CreateGroup_Table CreateGroup_Table { get; set; }

        //public CreateAccount_Table CreateAccount_Table { get; set; }

    }
}
