using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
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
    public class Inv_item_gl_accounts
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Description")]
        public string Desc { get; set; }
        [ForeignKey("item")]
        public int Item_id { get; set; }

        [ForeignKey("Inventory")]
        public int Inventory_id { get; set; }
        [ForeignKey("Inventory_returne")]
        [DisplayName("Inventory Returne")]
        public int? Inventory_returne_id { get; set; }
        [ForeignKey("Damage")]
        public int? Damage_id { get; set; }
        [ForeignKey("Variancies")]
        public int? Variancies_id { get; set; }

        [ForeignKey("Sales")]
        public int? Sales_id { get; set; }
        [ForeignKey("Sales_return")]
        public int? Sales_return_id { get; set; }
        [ForeignKey("Cost_of_GS")]
        public int? Cost_of_GS_id { get; set; }
        [ForeignKey("Purchase_variance")]
        public int? Purchase_variance_id { get; set; }

        [ForeignKey("Fright")]
        [DisplayName("Fright")]
        public int? Fright_id { get; set; } 
        [ForeignKey("Accrual_fright")]
        [DisplayName("Accrual fright")]
        public int? Accrual_fright_id { get; set; }


        public C_CreateAccount_Table Sales { get; set; }
        public C_CreateAccount_Table Sales_return { get; set; }
        public C_CreateAccount_Table Cost_of_GS { get; set; }
        public C_CreateAccount_Table Purchase_variance { get; set; }
        public C_CreateAccount_Table Inventory { get; set; }
        public C_CreateAccount_Table Inventory_returne { get; set; }
        public C_CreateAccount_Table Damage { get; set; }
        public C_CreateAccount_Table Variancies { get; set; }
        public C_CreateAccount_Table Fright { get; set; }
        public C_CreateAccount_Table Accrual_fright { get; set; }
        public Inv_item item { get; set; }
    }
}
