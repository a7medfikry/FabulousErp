namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Deprecation_Setting
    {
        public int Id { get; set; }

        public int Auto_or_manual { get; set; }

        public int Deprecation_calcualtion { get; set; }

        public int Deprecation_jv { get; set; }

        public bool Change_deprecation_method { get; set; }

        public bool Can_add_assets_info { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }
    }
}
