using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels
{
    public class Installment_view
    {
        [Key]
        public int Id { get; set; }
        public string Refrence { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public string Cheque_number { get; set; }
        public string Currency { get; set; }
        public DateTime? Date { get; set; }
        public Installment_due_state State { get; set; }
        public bool Historical { get; set; } = false;
        public string Transaction_date { get; set; }
    }
    public enum Installment_due_state
    {
        Due,
        Collecting,
        Achieved,
        Not_due,

        Payment_in_progress,
        Paid,

    }
}
