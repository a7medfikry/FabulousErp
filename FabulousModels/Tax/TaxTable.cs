using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Tax
{
    public class TaxTable
    {
          public string Action {get;set;}
          public string Item_name {get;set;}
          public decimal? UOM {get;set;}
          public decimal Quantity {get;set;}
          public decimal Unit_price {get;set;}
          public decimal Orginal_amount {get;set;}
          public decimal Total_amount {get;set;}
          public decimal Discount {get;set;}
          public decimal Net_amount {get;set;}
          public decimal? Vat_amount {get;set;}
          public int? Deducte_tax_id {get;set;}
          public decimal Deducte_amount {get;set;}
    }
}
