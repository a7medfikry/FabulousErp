using System.ComponentModel.DataAnnotations;

using FabulousDB.Models; namespace FabulousErp.Payable.Models
{
    public class Payable_aging_date_option
    {
        [Key]
        public int Id { get; set; }
        public Date_option Date_option { get; set; }
    }
}