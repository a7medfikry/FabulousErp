using FabulousDB.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels
{
    public class ChartHead
    {
        public List<string> Labels { get; set; }
        public float Max { get; set; }
    }
    public class ChartDetails
    {
        public string Label { get; set; }
        public List<float> Data { get; set; }
    }
    public class Chart
    {
        public int Order { get; set; }
        public ChartHead Head { get; set; }
        public List<ChartDetails> Details { get; set; }
    }
    public class Chart2
    {
        public List<GP> GP { get; set; }
        public List<ChartQtyAvaliable> QA { get; set; }
        public List<ChartMonthlySales> MS1 { get; set; }
        public List<ChartMonthlySales> MS2 { get; set; }
        public List<ChartMonthlySales> MS3 { get; set; }
        public List<ChartMonthlySales> MS4 { get; set; }
        public List<ChartVendorBalanc> VB { get; set; }
        public List<ChartVendorBalanc> VPurchase { get; set; }    
        public List<ChartCustomerBalanc> CB { get; set; }
        public List<ChartCustomerBalanc> CPurchase { get; set; }
    }
    public class ChartVendorBalanc
    {
        public string Vendore { get; set; }
        public decimal Amount { get; set; }
    }
    public class ChartCustomerBalanc
    {
        public string Customer { get; set; }
        public decimal Amount { get; set; }
    }
    public class ChartQtyAvaliable
    {
        public float Quantity_In { get; set; }
        public float Quantity_Out { get; set; }
        public float Quantity_Avaliable { get; set; }
        public string Item_name { get; set; }
    }
    public class GP
    {
        public decimal Cost_Of_Sales_Items { get; set; }
        public decimal Total_Sales_Income { get; set; }
        public decimal Grand_Profit { get; set; }
        public string Item_name { get; set; }
    }
    public class ChartMonthlySales
    {
        public decimal Sales { get; set; }
        public decimal Discount { get; set; }
        public decimal Total_Sales { get; set; }
    public string Item_name { get; set; }

}
public class Chart2D
{
    public string Key { get; set; }
    public decimal Value { get; set; }
    //public string X { get; set; }
    //public decimal Y1 { get; set; }
    //public decimal Y2 { get; set; }
    //public decimal Y3 { get; set; }
    // public List<ChartDetails> Details { get; set; }

}
}
