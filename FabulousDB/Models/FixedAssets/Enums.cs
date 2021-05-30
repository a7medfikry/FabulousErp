using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
   
        public enum Assets_type
        {
            New = 1,
            Used = 2
        }
        public enum Property_type
        {
            Persnoal = 1,
            Other = 2
        }
        public enum Deprecation_method
        {
            Fixed = 1,
            Decreased = 2,
            Number_of_units = 3,
            None = 4
        }
        public enum Stoking_assets_status
        {
            Good = 1,
            Bad = 0
        }
        public enum Auto_or_manual
        {
            Auto = 0,
            Manual = 1
        }
        public enum Deprecation_calcualtion
        {
            // Daily =0,
            Periodic = 1,
            Monthly = 2,
            Yearly = 3
        }
        public enum Deprecation_jv
        {
            Periodic = 0,
            Monthly = 1,
            Yearly = 2
        }
        public enum Prefix
        {
            Cash = 0,
            Asset = 1,
            Pay = 2,
            REC = 3,
            Tax = 4,
            INV = 5

        }
        public enum Posting_type
        {
            BallanceSheet = 1,
            PL = 2
        }
        public enum Assets_trnsaction_type
        {
            Direct = 0,
            Payable = 1,
            Inventory = 2
        }
  
}
