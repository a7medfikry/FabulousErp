namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Assets_accounts
    {
        public int Id { get; set; }

        public int? Class_id { get; set; }

        public int Cost_account { get; set; }

        public int Deprecation_accumulated_account { get; set; }

        public int? Profit_account { get; set; }

        public int? Lose_account { get; set; }

        public int? Payable_account { get; set; }

        public int? Revaluation_account { get; set; }

        public int Deprcation { get; set; }

        public int? Retirment { get; set; }

        public int? Accrued { get; set; }

        public int? Receivable_account { get; set; }

        public virtual Assets_class Assets_class { get; set; }
    }
}
