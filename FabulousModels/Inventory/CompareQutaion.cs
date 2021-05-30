using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class CompareQutaion
    {
        public int Id {get;set;}
        public string Vendore_id {get;set;}
        public string Name { get; set; }
        public string Qutation_no { get; set; }
        public string Currency { get; set; }
        public decimal Grand_total { get; set; }
        public decimal Trade_discount { get; set; }
        public int Flight { get; set; }
        public int Miscellaneous { get; set; }
        public decimal Tax { get; set; }
        public decimal Net { get; set; }
     
    }
}
