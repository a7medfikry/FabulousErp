using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FabulousDB.Models
{
   public class Number_of_deprecation_units
    {
        [Key]
        public int Asset_id { get; set; }
        public string Asset_description { get; set; }
        public int Deprecation_unit { get; set; }
    }
    public enum Purchase_type
    {
        Direct=1,
        Payable=2,
        Inventory=3
    }
    public enum StockingStatus
    {
        Good=1,
        Bad=0
    }
}