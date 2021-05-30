using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Payable.Models
{
    public class Payable_payment_term
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName ="nvarchar"),MaxLength(50)]
        public string Terms_id { get; set; }
        [DefaultValue(false)]
        public bool Inactive { get; set; }
        [DisplayName("Amount type")]
        public Amount_type Amount_type { get; set; }
        public decimal Amount { get; set; }
        [DisplayName("Net Days")]
        public int Net_Days { get; set; }
        [DisplayName("Total Days")]
        public int Total_Days { get; set; }
        public Date_option Date_option { get; set; }
    }
   
}