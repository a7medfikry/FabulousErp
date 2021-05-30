namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Stocking_notes
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public bool? Important { get; set; }

        public int? Stoking_assets_id { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public virtual Stoking_assets Stoking_assets { get; set; }
    }
}
