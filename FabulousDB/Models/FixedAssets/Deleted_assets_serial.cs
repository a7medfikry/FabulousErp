namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Deleted_assets_serial
    {
        public int Id { get; set; }

        public int? Assets_id { get; set; }

        public int? Part_number { get; set; }

        [StringLength(200)]
        public string Serial { get; set; }
    }
}
