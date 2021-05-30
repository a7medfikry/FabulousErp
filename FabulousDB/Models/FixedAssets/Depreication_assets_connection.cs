namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Depreication_assets_connection
    {
        public int Id { get; set; }

        public int? Deprecation_id { get; set; }

        public int? Assets_class_id { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public virtual Assets_class Assets_class { get; set; }

        public virtual Deprecation Deprecation { get; set; }
    }
}
