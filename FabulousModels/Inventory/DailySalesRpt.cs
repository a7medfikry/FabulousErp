using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class DailySales
    {
        public Doc_type Doc_type { get; set; }
        public string Doc_no { get; set; }
        public DateTime Date { get; set; }
        public string Customer_name { get; set; }
        public decimal Sales { get; set; }
        public decimal Discount { get; set; }
        public decimal Net_amount { get; set; }
        public string Period_no { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
