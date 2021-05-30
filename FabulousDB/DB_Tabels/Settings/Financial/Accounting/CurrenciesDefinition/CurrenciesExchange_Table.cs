using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition
{
    public class CurrenciesExchange_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExchangeID { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string EstablishDate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string StartDate { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string ExpireDate { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }



        // currency definition has many of currency Exchange
        public CurrenciesDefinition_Table CurrenciesDefinition_Table { get; set; }
    }
}
