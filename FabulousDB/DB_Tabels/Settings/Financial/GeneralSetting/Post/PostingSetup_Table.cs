using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Post
{
    public class PostingSetup_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PS_ID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(11)]
        [Required]
        public string Module { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(2)]
        [Required]
        public string PostingType { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(2)]
        [Required]
        public string CreateJEPer { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(2)]
        public string Batch { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(2)]
        public string PostingDataFrom { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(2)]
        public string ExistingBatch { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(2)]
        [Required]
        public string EditPostingDate { get; set; }



        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }

    }
}
