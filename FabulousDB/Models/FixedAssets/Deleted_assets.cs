namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Deleted_assets
    {
        public int Id { get; set; }

        public int Assets_class_id { get; set; }

        public int? Assets_main_id { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Foreign_name { get; set; }

        public int Type { get; set; }

        public int Proparty_type { get; set; }

        public decimal Acquisation_cost { get; set; }

        public decimal? Scrap_value { get; set; }

        [Column(TypeName = "date")]
        public DateTime Start_use { get; set; }

        [Column(TypeName = "date")]
        public DateTime Start_derecation_date { get; set; }

        public int? Number_of_units { get; set; }

        public bool Deactive_depraction { get; set; }

        public bool Fully_depraction { get; set; }

        public bool Disposal { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date_of_orgin { get; set; }

        public decimal Adjustment_cost { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date_of_Adjustmemt { get; set; }

        [Column(TypeName = "date")]
        public DateTime Use_life { get; set; }

        public int Deprecation_method { get; set; }

        public bool Include_scerap_value { get; set; }

        [StringLength(200)]
        public string Assets_number { get; set; }

        public decimal Deprecation_rate { get; set; }

        public int? Book_id { get; set; }

        public int? Assets_transaction_id { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public int? Gl_transaction_id { get; set; }

        public DateTime Deleted_date { get; set; }
    }
}
