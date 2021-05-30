using FabulousDB.DB_Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FabulousDB.Models
{
    [MetadataType(typeof(Asset_validation))]
    public partial class Asset
    {
        DBContext db = new DBContext();

        public class Asset_validation
        {

            public int Id { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public Nullable<System.DateTime> Start_use { get; set; }
            [Required]
            public string Description { get; set; }
            [Required]
            public Nullable<int> Assets_transaction_id { get; set; }
            [Required]
            public Nullable<int> Book_id { get; set; }

        }
      
      
        [NotMapped]
        public DateTime? Transaction_date { get; set; }
       

      

    }

    //[MetadataType(typeof(Asset_main_validation))]
    //public partial class Assets_main
    //{
    //    public class Asset_main_validation
    //    {
    //        [Required]
    //        public int Assets_class_id { get; set; }
    //        [Required]
    //        public string Description { get; set; }
    //        [Required]
    //        public string Assets_number { get; set; }

    //    }
    //}
    [NotMapped]
    public class InsertAssets_main : Assets_main
    {
        [Required]
        public int Assets_class_id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Assets_number { get; set; }
    }
    [MetadataType(typeof(Assets_class_validation))]
    public partial class Assets_class
    {
        public class Assets_class_validation
        {
            [Required]
            public int Class_id { get; set; }
            [Required]
            public string Description { get; set; }
            [Range(1,100)]
            public Nullable<decimal> Deperecation_rate { get; set; }

        }
    }
    [MetadataType(typeof(Assets_accounts_validation))]
    public partial class Assets_accounts
    {
        public class Assets_accounts_validation
        {
            [Required]
            public int? Cost_account { get; set; }
            [Required]
            public int? Deprecation_accumulated_account { get; set; }
            [Required]
            public int? Deprcation { get; set; }

        }
    }
    [MetadataType(typeof(Book_validation))]
    public partial class Book
    {
        public class Book_validation
        {
            [Required]
            public string Description { get; set; }
        }

    }

    public partial class Deprecation_Setting
    {
        public class Deprecation_Setting_validation
        {
        }
        [NotMapped]
        public Auto_or_manual Auto_or_manual_enum { get; set; }
        [NotMapped]
        public Deprecation_calcualtion Deprecation_calcualtion_enum { get; set; }
        [NotMapped]
        public Deprecation_jv Deprecation_jv_enum { get; set; }

    }
    [MetadataType(typeof(New_assets_transaction_validate))]
    public partial class New_assets_transaction
    {
        public class New_assets_transaction_validate
        {
            [Required]
            public DateTime Date_of_orgin { get; set; }
        }
    }
    [MetadataType(typeof(Fixed_assets_renewal_validate))]

    public partial class Fixed_assets_renewal
    {
        public class Fixed_assets_renewal_validate
        {
       
            [Required]
            public Nullable<int> Assets_id { get; set; }
            [Required]
            public Nullable<decimal> Renewal_amount { get; set; }
            [Required]
            public Nullable<int> Currency_id { get; set; }
        }
        [NotMapped]
        public DateTime Transaction_date { get; set; }
    }
}