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
    public class Installment_setting
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        [DisplayName("Plan Id")]
        public string Plan_id { get; set; }
        public Installment_types Type { get; set; }
        public bool Inactive { get; set; } = false;
        [DisplayName("Cash Adv. Perc.")]
        public float Cash_advanced_percentage { get; set; }
        [NotMapped]
        public float Cash_advanced_amount { get; set; }=0;
        [DisplayName("No. Of Instal.")]
        public int Number_of_installment { get; set; }
        [DisplayName("Instal. Period")]
        public Installment_period Installment_period { get; set; }
        public Increase_by Increase_by { get; set; }
        public decimal Increase { get; set; } = 0;
        public Increase_by Penelty_by { get; set; }
        public float Penelty_days{ get; set; } = 0;
        public decimal Penelty { get; set; } = 0;
        public virtual ICollection<Custom_installment> Custom_installment { get; set; }
    }
    public enum Installment_types
    {
        Cash=1,
        Cheque=2
    }
    public enum Increase_by
    {
        Amount=1,
        Percentage=2
    }
    public enum Installment_period
    {
        Yearly=1,
        Monthly=2,
        Quarter=3,
        Custom = 4

    }
}
