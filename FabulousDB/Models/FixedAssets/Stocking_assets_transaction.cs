namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Stocking_assets_transaction
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Stocking_no { get; set; }

        public DateTime Reconcile_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Transaction_date { get; set; }

        public int? Status { get; set; }

        public bool? Reconcile { get; set; }

        public int? Stocking_assets_id { get; set; }

        public virtual Stoking_assets Stoking_assets { get; set; }
    }
}
