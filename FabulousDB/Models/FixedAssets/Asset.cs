namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Asset
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Asset()
        {
            Additional_information = new HashSet<Additional_information>();
            Assets_part_serial = new HashSet<Assets_part_serial>();
            Deprecation_record = new HashSet<Deprecation_record>();
            Deprecation_number_of_units = new HashSet<Deprecation_number_of_units>();
            Deprecation_temp_record = new HashSet<Deprecation_temp_record>();
            Depreication_assets_id_connection = new HashSet<Depreication_assets_id_connection>();
            Fixed_assets_disposel = new HashSet<Fixed_assets_disposel>();
            Fixed_assets_renewal = new HashSet<Fixed_assets_renewal>();
            Fixed_assets_revaluate = new HashSet<Fixed_assets_revaluate>();
            Stoking_assets = new HashSet<Stoking_assets>();
        }

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
        [ForeignKey("Book")]
        public int? Book_id { get; set; }

        [ForeignKey("New_assets_transaction")]
        public int? Assets_transaction_id { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public DateTime? Creation_date { get; set; }

        public int? Purchase_types { get; set; }

        public int? Payable_Inv_trans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Additional_information> Additional_information { get; set; }

        public virtual Assets_class Assets_class { get; set; }

        public virtual Assets_main Assets_main { get; set; }

        public virtual New_assets_transaction New_assets_transaction { get; set; }

        public virtual Book Book { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Assets_part_serial> Assets_part_serial { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deprecation_record> Deprecation_record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deprecation_number_of_units> Deprecation_number_of_units { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deprecation_temp_record> Deprecation_temp_record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Depreication_assets_id_connection> Depreication_assets_id_connection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fixed_assets_disposel> Fixed_assets_disposel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [InverseProperty("Asset")]
        public virtual ICollection<Fixed_assets_renewal> Fixed_assets_renewal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fixed_assets_revaluate> Fixed_assets_revaluate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stoking_assets> Stoking_assets { get; set; }
    }
}
