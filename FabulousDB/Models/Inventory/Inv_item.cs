using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_item
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Item ID")]
        [MaxLength(200)]
        [Required]
        public string Item_id { get; set; }
        [MaxLength(500)]
        [Required]
        public string Description { get; set; }
        [MaxLength(200)]
        public string Short_description { get; set; }
        [DisplayName("Item group id")]
        [ForeignKey("Inv_item_group")]
        public int? Item_group_id { get; set; }
        public Item_group_type Type { get; set; }
        [ForeignKey("Unit_of_measure")]
        [DisplayName("UOM")]
        public int? Unit_of_measure_id { get; set; }
        //[ForeignKey("Sales_tax_group")]
        //[Required]
        //public int Sales_tax_group_id { get; set; }
        //[ForeignKey("Purchase_tax_group")]
        //[Required]
        //public int Purchase_tax_group_id { get; set; }
        public Validation_method Validation_method { get; set; }
        public Unit_of_measure Unit_of_measure { get; set; }
        public Inv_item_group Inv_item_group { get; set; }
        public int? Vat_Item_type { get; set; }
        public int? Tax_type_id { get; set; }
        public int? Tax_table_type_id { get; set; }
        [ForeignKey("Tbl_vat")]
        public int? Tbl_vat_Id { get; set; }
        [ForeignKey("Vat")]
        public int? Vat_id { get; set; }
        [ForeignKey("Cost_center")]
        public string Cost_center_id { get; set; }
        [ForeignKey("Cost_center_account")]
        public int? Cost_center_account_id { get; set; }
        public bool Has_serial { get; set; } = false;
        public bool Has_warranty { get; set; } = false;
        public bool Has_expiry_date { get; set; } = false;
        public bool Inactive { get; set; } = false;
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Has_password { get; set; }=false ;
        public MartialService Martial_or_service { get; set; }
        public C_CostCenterAccounts_Table Cost_center_account { get; set; }
        public C_CostCenter_Table Cost_center { get; set; }
        public C_TaxSetting_table Tbl_vat { get; set; }
        public C_TaxSetting_table Vat { get; set; }
        public virtual ICollection<Inv_item_deduct_tax> Deduct_tax { get; set; }
        public virtual ICollection<Inv_item_gl_accounts> Item_gl_account { get; set; }
        public virtual ICollection<Inv_item_store_site> Item_store_site { get; set; }
        public virtual ICollection<Inv_receive_po_items> Inv_receive_po_items { get; set; }
        [InverseProperty("Main_item")]
        public virtual ICollection<Inv_item_recipe> Inv_item_recipe { get; set; }
        public virtual ICollection<Inv_item_option> Inv_item_option { get; set; }
        //public C_TaxSetting_table Sales_tax_group { get; set; }
        //public C_TaxSetting_table Purchase_tax_group { get; set; }

    }
    public enum MartialService
    {
        Martial=1,
        Service=2
    }
}
