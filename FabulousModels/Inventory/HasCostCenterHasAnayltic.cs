using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class HasCostCenterHasAnayltic
    { 
        public int Aid { get; set; }
        public bool HasCostCenter { get; set; } = false;
        public bool HasAnayltic { get; set; } = false;
        public string CostCetnerType { get; set; } = "";
        public string CostCenterId { get; set; } = "";
        public string CostGroupId { get; set; } = "";
    }
}
