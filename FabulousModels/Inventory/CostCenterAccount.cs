using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public  class CostCenterAccount
    {
        [Key]
        public int C_aid { get; set; }
        public string Account_name { get; set; }
        public string Cost_center { get; set; }
        public bool Exist { get; set; }
    }
}
