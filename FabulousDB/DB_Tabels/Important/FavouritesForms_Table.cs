using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Important
{
    public class FavouritesForms_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "nvarchar")]
        public string UserID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string FormCode { get; set; }

        [Column(TypeName = "nvarchar")]
        [Required]
        public string FormURL { get; set; }

        [Column(TypeName = "nvarchar")]
        [Required]
        public string FormName { get; set; }



        public int Type { get; set; }

        public CreateAccount_Table CreateAccount_Table { get; set; }
    }
}
