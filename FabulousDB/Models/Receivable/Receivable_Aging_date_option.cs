using System.ComponentModel.DataAnnotations;

using FabulousDB.Models; namespace FabulousErp.Receivable.Models
{
    public class Receivable_aging_date_option
    {
        [Key]
        public int Id { get; set; }
        public Date_option Date_option { get; set; }
    }
   
}