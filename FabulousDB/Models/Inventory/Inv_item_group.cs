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
    public class Inv_item_group
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Item_group_id { get; set; }
        [MaxLength(500)]
        [DisplayName("Description")]
        public string Desc { get; set; }
        public Item_group_type Type { get; set; }
        [ForeignKey("Unit_of_measure")]
        public int? Unit_of_measure_id { get; set; }
        public int Item_type { get; set; }
        public int? Tax_type_id { get; set; }
        public int? Tax_table_type_id { get; set; }
        public int? Vat_Item_type { get; set; }
        [ForeignKey("Tbl_vat")]
        public int? Tbl_vat_Id { get; set; }
        [ForeignKey("Vat")]
        public int? Vat_id { get; set; }
        [ForeignKey("Cost_center")]
        public string Cost_center_id { get; set; }
        [ForeignKey("Cost_center_account")]
        public int? Cost_center_account_id { get; set; }
        public C_CostCenterAccounts_Table Cost_center_account { get; set; }
        public C_CostCenter_Table Cost_center { get; set; }
        [DisplayName("Validation Name")]
        public Validation_method Validation_method { get; set; }
        public Unit_of_measure Unit_of_measure { get; set; }
        public C_TaxSetting_table Tbl_vat { get; set; }
        public C_TaxSetting_table Vat { get; set; }
        public bool Has_serial { get; set; } = false;
        public bool Has_warranty { get; set; } = false;
        public bool Has_expiry_date { get; set; } = false;
        public MartialService Martial_or_service { get; set; }
        public virtual ICollection<Inv_item_group_deduct_tax> Deduct_tax { get; set; }
        public virtual ICollection<Inv_gorup_gl_accounts> Inv_group_gl { get; set; }
    }
    public enum Item_group_type
    {
        Purchase = 1,
        Sales = 2,
        Both
    }
    public enum Validation_method
    {
        FIFO = 1, 
        LIFO = 2, 
        WA=3
    }
}
