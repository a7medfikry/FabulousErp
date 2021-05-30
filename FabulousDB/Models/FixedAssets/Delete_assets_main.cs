namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Delete_assets_main
    {
        public int Id { get; set; }

        public int Assets_class_id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int Number_of_parts { get; set; }

        public bool Inactive { get; set; }

        [StringLength(200)]
        public string Assets_number { get; set; }

        public int? Assets_custom_id { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public DateTime Deleted_date { get; set; }
    }
}
