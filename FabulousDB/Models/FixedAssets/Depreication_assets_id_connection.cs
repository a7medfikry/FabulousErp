namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Depreication_assets_id_connection
    {
        public int Id { get; set; }

        public int? Deprecation_id { get; set; }

        public int? Assets_id { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual Deprecation Deprecation { get; set; }
    }
}
