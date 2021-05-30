using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Custom_installment
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Custom_no { get; set; }
        public float Percetage { get; set; }
        [ForeignKey("Installment_setting")]
        [DisplayName("Installment setting")]
        public int Installment_setting_id { get; set; }
        public Installment_setting Installment_setting { get; set; }
    }
}
