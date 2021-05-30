using FabulousErp.MyRoleProvider;
using FabulousModels.Inventory;
using FabulousModels.ViewModels;
using LinqToExcel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers
{
    [AuthorizationFilter]
    public class DashboardController : Controller
    {

        public ActionResult Index()
        {
            return RedirectToAction("Index3");
            List<Chart> Chart = new List<Chart>();
            using (Inventory.Controllers.RptController
                Rpt = new Inventory.Controllers.RptController())
            {
               
                List<GrandProfit> GrandRes = new List<GrandProfit>();

                try
                {
                    GrandRes = ViewBag.GrandRes = Rpt.CalcGrandProfit(MyDbContext.Instance.Inv_item.ToList().DefaultIfEmpty(new FabulousDB.Models.Inv_item { Id=0}).FirstOrDefault().Id
                , MyDbContext.Instance.Inv_item.OrderByDescending(x => x.Id).ToList().DefaultIfEmpty(new FabulousDB.Models.Inv_item { Id = 0 }).FirstOrDefault().Id,
                null, null, null, null, true).ToList().DefaultIfEmpty(new GrandProfit { }).ToList();
                    Chart.Add(new Chart
                    {
                        Order = 1,
                        Head = new ChartHead
                        {
                            Labels = GrandRes.OrderBy(z => z.Item_id).Select(z => z.Item_name).ToList(),
                            Max = (float)(GrandRes.Max(x => x.Total_Revenue_from_sales) > GrandRes.Max(x => x.Cost_of_qty_out)
                            ? GrandRes.Max(x => x.Total_Revenue_from_sales) : GrandRes.Max(x => x.Cost_of_qty_out))
                        },
                        Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Cost Of Sales Items"),
                            Data =GrandRes.OrderBy(z=>z.Item_id).Select(z => (float)z.Cost_of_qty_out).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Total Sales Income"),
                            Data =GrandRes.OrderBy(z=>z.Item_id).Select(z => (float)z.Total_Revenue_from_sales).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Grand Profit"),
                            Data =GrandRes.OrderBy(z=>z.Item_id).Select(z => (float)(z.Total_Revenue_from_sales-z.Cost_of_qty_out)).ToList()
                        }
                    }
                    });

                }
                catch
                {

                }
                List<QtyAvaliable> QtyRes = new List<QtyAvaliable>();

                try
                {
                    QtyRes = ViewBag.QtyRes = Rpt.CalcQtyAvaliable(true);
                    Chart.Add(new Chart
                    {
                        Order = 2,
                        Head = new ChartHead
                        {
                            Labels = QtyRes.OrderBy(z => z.Item_id).Select(z => z.Item_name).ToList(),
                        },
                        Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Quantity In"),
                            Data =QtyRes.OrderBy(z=>z.Item_id).Select(z => (float)z.Total_qty_in).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Quantity Out"),
                            Data =QtyRes.OrderBy(z=>z.Item_id).Select(z => (float)z.Total_qty_out).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Quantity Avaliable"),
                            Data =QtyRes.OrderBy(z=>z.Item_id).Select(z => (float)(z.Total_qty_in-z.Total_qty_out)).ToList()
                        }
                    }
                    });
                }
                catch
                {

                }
                List<string> Columns;
                int YearId=ViewBag.YearId = MyDbContext.Instance.NewFiscalYear_Table.FirstOrDefault(x => x.Closed != false).YearID;
              
                List<MonthlySalesRpt> MonthInvSalesRes=  Rpt.CalcMonthlySalesRes(false, false, YearId, "Month", null, null, out Columns);
                Chart.Add(new Chart
                {
                    Order = 3,
                    Head = new ChartHead
                    {
                        Labels = Columns,
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Sales"),
                            Data =MonthInvSalesRes.Select(z => (float)z.Sales).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Discount"),
                            Data =MonthInvSalesRes.Select(z => (float)z.Discount).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Total Sales"),
                            Data =MonthInvSalesRes.Select(z => (float)(z.Sales-z.Discount)).ToList()
                        }
                    }
                });

                List<MonthlySalesRpt> MonthRetSalesRes = Rpt.CalcMonthlySalesRes(false,true, YearId, "Month", null, null, out Columns);
                Chart.Add(new Chart
                {
                    Order = 4,
                    Head = new ChartHead
                    {
                        Labels = Columns,
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Sales"),
                            Data =MonthRetSalesRes.Select(z => (float)z.Sales).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Discount"),
                            Data =MonthRetSalesRes.Select(z => (float)z.Discount).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Total Sales"),
                            Data =MonthRetSalesRes.Select(z => (float)(z.Sales-z.Discount)).ToList()
                        }
                    }
                });   
                
                List<MonthlySalesRpt> MonthInvPurchasRes=  Rpt.CalcMonthlySalesRes(true, false, YearId, "Month", null, null, out Columns);
                Chart.Add(new Chart
                {
                    Order = 5,
                    Head = new ChartHead
                    {
                        Labels = Columns,
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Purchase"),
                            Data =MonthInvPurchasRes.Select(z => (float)z.Sales).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Discount"),
                            Data =MonthInvPurchasRes.Select(z => (float)z.Discount).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Total Purchase"),
                            Data =MonthInvSalesRes.Select(z => (float)(z.Sales-z.Discount)).ToList()
                        }
                    }
                });

                List<MonthlySalesRpt> MonthRetPurchasRes = Rpt.CalcMonthlySalesRes(true, true, YearId, "Month", null, null, out Columns);
                Chart.Add(new Chart
                {
                    Order = 6,
                    Head = new ChartHead
                    {
                        Labels = Columns,
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Purchase"),
                            Data =MonthRetPurchasRes.Select(z => (float)z.Sales).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Discount"),
                            Data =MonthRetPurchasRes.Select(z => (float)z.Discount).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Total Purchase"),
                            Data =MonthRetSalesRes.Select(z => (float)(z.Sales-z.Discount)).ToList()
                        }
                    }
                }); 
                /////////////////////////
                List<SalesAndDebit> VendoreBalance=  Rpt.CalcCustomerSales(true,null,null,MyDbContext.Instance.Payable_creditor_setting.FirstOrDefault().Id,
                    MyDbContext.Instance.Payable_creditor_setting.OrderByDescending(x=>x.Id).FirstOrDefault().Id
                    ,true);
                Chart.Add(new Chart
                {
                    Order = 7,
                    Head = new ChartHead
                    {
                        Labels = VendoreBalance.SelectMany(x=>x.Debit.GroupBy(z=>z.Customer_name).Select(z=>z.FirstOrDefault().Customer_name))
                        .ToList(),
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Amount"),
                            Data =VendoreBalance.SelectMany(z => z.Debit.GroupBy(i=>i.Customer_name).Select(y=>(float)(y.Sum(k=>k.Amount)))).ToList()
                        },
                    }
                }); 

                Chart.Add(new Chart
                {
                    Order = 8,
                    Head = new ChartHead
                    {
                        Labels = VendoreBalance.SelectMany(x=>x.Sales.Select(z=>z.Customer_name)).ToList(),
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Amount"),
                            Data =VendoreBalance.SelectMany(z => z.Sales.Select(y=>(float)(y.Amount-y.Discount))).ToList()
                        },
                    }
                });

                List<SalesAndDebit> CustomerBalance = Rpt.CalcCustomerSales(false, null, null, MyDbContext.Instance.Receivable_vendore_settings.FirstOrDefault().Id,
                    MyDbContext.Instance.Receivable_vendore_settings.OrderByDescending(x => x.Id).FirstOrDefault().Id
                    , true);

                Chart.Add(new Chart
                {
                    Order = 9,
                    Head = new ChartHead
                    {
                        Labels = CustomerBalance.SelectMany(x => x.Debit.GroupBy(z => z.Customer_name).Select(z => z.FirstOrDefault().Customer_name)).ToList(),
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Amount"),
                            Data =CustomerBalance.SelectMany(z => z.Debit.GroupBy(i=>i.Customer_name).Select(y=>(float)(y.Sum(k=>k.Amount)))).ToList()
                        },
                    }
                });

                Chart.Add(new Chart
                {
                    Order =10,
                    Head = new ChartHead
                    {
                        Labels = CustomerBalance.SelectMany(x => x.Sales.Select(z => z.Customer_name)).ToList(),
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Amount"),
                            Data =CustomerBalance.SelectMany(z => z.Sales.Select(y=>(float)(y.Amount-y.Discount))).ToList()
                        },
                    }
                });


            }
            return View(Chart);
        }
        public ActionResult Index2()
        {
            List<Chart> Chart = new List<Chart>();
            using (Inventory.Controllers.RptController
                Rpt = new Inventory.Controllers.RptController())
            {

                List<GrandProfit> GrandRes = new List<GrandProfit>();

                try
                {
                    GrandRes = ViewBag.GrandRes = Rpt.CalcGrandProfit(MyDbContext.Instance.Inv_item.ToList().DefaultIfEmpty(new FabulousDB.Models.Inv_item { Id = 0 }).FirstOrDefault().Id
                , MyDbContext.Instance.Inv_item.OrderByDescending(x => x.Id).ToList().DefaultIfEmpty(new FabulousDB.Models.Inv_item { Id = 0 }).FirstOrDefault().Id,
                null, null, null, null, true).ToList().DefaultIfEmpty(new GrandProfit { }).ToList();
                    Chart.Add(new Chart
                    {
                        Order = 1,
                        Head = new ChartHead
                        {
                            Labels = GrandRes.OrderBy(z => z.Item_id).Select(z => z.Item_name).ToList(),
                            Max = (float)(GrandRes.Max(x => x.Total_Revenue_from_sales) > GrandRes.Max(x => x.Cost_of_qty_out)
                            ? GrandRes.Max(x => x.Total_Revenue_from_sales) : GrandRes.Max(x => x.Cost_of_qty_out))
                        },
                        Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Cost Of Sales Items"),
                            Data =GrandRes.OrderBy(z=>z.Item_id).Select(z => (float)z.Cost_of_qty_out).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Total Sales Income"),
                            Data =GrandRes.OrderBy(z=>z.Item_id).Select(z => (float)z.Total_Revenue_from_sales).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Grand Profit"),
                            Data =GrandRes.OrderBy(z=>z.Item_id).Select(z => (float)(z.Total_Revenue_from_sales-z.Cost_of_qty_out)).ToList()
                        }
                    }
                    });

                }
                catch
                {

                }
                List<QtyAvaliable> QtyRes = new List<QtyAvaliable>();

                try
                {
                    QtyRes = ViewBag.QtyRes = Rpt.CalcQtyAvaliable(true);
                    Chart.Add(new Chart
                    {
                        Order = 2,
                        Head = new ChartHead
                        {
                            Labels = QtyRes.OrderBy(z => z.Item_id).Select(z => z.Item_name).ToList(),
                        },
                        Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Quantity In"),
                            Data =QtyRes.OrderBy(z=>z.Item_id).Select(z => (float)z.Total_qty_in).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Quantity Out"),
                            Data =QtyRes.OrderBy(z=>z.Item_id).Select(z => (float)z.Total_qty_out).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Quantity Avaliable"),
                            Data =QtyRes.OrderBy(z=>z.Item_id).Select(z => (float)(z.Total_qty_in-z.Total_qty_out)).ToList()
                        }
                    }
                    });
                }
                catch
                {

                }
                List<string> Columns;
                int YearId = ViewBag.YearId = MyDbContext.Instance.NewFiscalYear_Table.FirstOrDefault(x => x.Closed != false).YearID;

                List<MonthlySalesRpt> MonthInvSalesRes = Rpt.CalcMonthlySalesRes(false, false, YearId, "Month", null, null, out Columns);
                Chart.Add(new Chart
                {
                    Order = 3,
                    Head = new ChartHead
                    {
                        Labels = Columns,
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Sales"),
                            Data =MonthInvSalesRes.Select(z => (float)z.Sales).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Discount"),
                            Data =MonthInvSalesRes.Select(z => (float)z.Discount).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Total Sales"),
                            Data =MonthInvSalesRes.Select(z => (float)(z.Sales-z.Discount)).ToList()
                        }
                    }
                });

                List<MonthlySalesRpt> MonthRetSalesRes = Rpt.CalcMonthlySalesRes(false, true, YearId, "Month", null, null, out Columns);
                Chart.Add(new Chart
                {
                    Order = 4,
                    Head = new ChartHead
                    {
                        Labels = Columns,
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Sales"),
                            Data =MonthRetSalesRes.Select(z => (float)z.Sales).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Discount"),
                            Data =MonthRetSalesRes.Select(z => (float)z.Discount).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Total Sales"),
                            Data =MonthRetSalesRes.Select(z => (float)(z.Sales-z.Discount)).ToList()
                        }
                    }
                });

                List<MonthlySalesRpt> MonthInvPurchasRes = Rpt.CalcMonthlySalesRes(true, false, YearId, "Month", null, null, out Columns);
                Chart.Add(new Chart
                {
                    Order = 5,
                    Head = new ChartHead
                    {
                        Labels = Columns,
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Purchase"),
                            Data =MonthInvPurchasRes.Select(z => (float)z.Sales).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Discount"),
                            Data =MonthInvPurchasRes.Select(z => (float)z.Discount).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Total Purchase"),
                            Data =MonthInvSalesRes.Select(z => (float)(z.Sales-z.Discount)).ToList()
                        }
                    }
                });

                List<MonthlySalesRpt> MonthRetPurchasRes = Rpt.CalcMonthlySalesRes(true, true, YearId, "Month", null, null, out Columns);
                Chart.Add(new Chart
                {
                    Order = 6,
                    Head = new ChartHead
                    {
                        Labels = Columns,
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Purchase"),
                            Data =MonthRetPurchasRes.Select(z => (float)z.Sales).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Discount"),
                            Data =MonthRetPurchasRes.Select(z => (float)z.Discount).ToList()
                        },
                        new ChartDetails
                        {
                            Label =Business.Translate("Total Purchase"),
                            Data =MonthRetSalesRes.Select(z => (float)(z.Sales-z.Discount)).ToList()
                        }
                    }
                });
                /////////////////////////
                List<SalesAndDebit> VendoreBalance = Rpt.CalcCustomerSales(true, null, null, MyDbContext.Instance.Payable_creditor_setting.FirstOrDefault().Id,
                    MyDbContext.Instance.Payable_creditor_setting.OrderByDescending(x => x.Id).FirstOrDefault().Id
                    , true);
                Chart.Add(new Chart
                {
                    Order = 7,
                    Head = new ChartHead
                    {
                        Labels = VendoreBalance.SelectMany(x => x.Debit.GroupBy(z => z.Customer_name).Select(z => z.FirstOrDefault().Customer_name))
                        .ToList(),
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Amount"),
                            Data =VendoreBalance.SelectMany(z => z.Debit.GroupBy(i=>i.Customer_name).Select(y=>(float)(y.Sum(k=>k.Amount)))).ToList()
                        },
                    }
                });

                Chart.Add(new Chart
                {
                    Order = 8,
                    Head = new ChartHead
                    {
                        Labels = VendoreBalance.SelectMany(x => x.Sales.Select(z => z.Customer_name)).ToList(),
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Amount"),
                            Data =VendoreBalance.SelectMany(z => z.Sales.Select(y=>(float)(y.Amount-y.Discount))).ToList()
                        },
                    }
                });

                List<SalesAndDebit> CustomerBalance = Rpt.CalcCustomerSales(false, null, null, MyDbContext.Instance.Receivable_vendore_settings.FirstOrDefault().Id,
                    MyDbContext.Instance.Receivable_vendore_settings.OrderByDescending(x => x.Id).FirstOrDefault().Id
                    , true);

                Chart.Add(new Chart
                {
                    Order = 9,
                    Head = new ChartHead
                    {
                        Labels = CustomerBalance.SelectMany(x => x.Debit.GroupBy(z => z.Customer_name).Select(z => z.FirstOrDefault().Customer_name)).ToList(),
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Amount"),
                            Data =CustomerBalance.SelectMany(z => z.Debit.GroupBy(i=>i.Customer_name).Select(y=>(float)(y.Sum(k=>k.Amount)))).ToList()
                        },
                    }
                });

                Chart.Add(new Chart
                {
                    Order = 10,
                    Head = new ChartHead
                    {
                        Labels = CustomerBalance.SelectMany(x => x.Sales.Select(z => z.Customer_name)).ToList(),
                    },
                    Details = new List<ChartDetails>
                    {
                        new ChartDetails
                        {
                            Label =Business.Translate("Amount"),
                            Data =CustomerBalance.SelectMany(z => z.Sales.Select(y=>(float)(y.Amount-y.Discount))).ToList()
                        },
                    }
                });


            }
            return View(Chart);
        }
        public ActionResult Index3()
        {
            Chart2 Chart = new Chart2();
            ViewBag.Title = "DashBoard";
            ViewBag.NewCustomer = MyDbContext.Instance.Receivable_vendore_settings
                   .Where(x => x.Creation_date.Year == DateTime.Now.Year).Count()
                   - MyDbContext.Instance.Receivable_vendore_settings
                   .Where(x => x.Creation_date.Year == DateTime.Now.Year - 1).Count(); 
            
            ViewBag.NewCustomer = MyDbContext.Instance.Receivable_vendore_settings
                   .Where(x => x.Creation_date.Year == DateTime.Now.Year).Count()
                   - MyDbContext.Instance.Receivable_vendore_settings
                   .Where(x => x.Creation_date.Year == DateTime.Now.Year - 1).Count();
            try
            {
                using (Inventory.Controllers.RptController
         Rpt = new Inventory.Controllers.RptController())
                {

                    List<GrandProfit> GrandRes = new List<GrandProfit>();

                    try
                    {
                        GrandRes = ViewBag.GrandRes = Rpt.CalcGrandProfit(MyDbContext.Instance.Inv_item.ToList().DefaultIfEmpty(new FabulousDB.Models.Inv_item { Id = 0 }).FirstOrDefault().Id
                    , MyDbContext.Instance.Inv_item.OrderByDescending(x => x.Id).ToList().DefaultIfEmpty(new FabulousDB.Models.Inv_item { Id = 0 }).FirstOrDefault().Id,
                    null, null, null, null, true).ToList().DefaultIfEmpty(new GrandProfit { }).ToList();

                        ViewBag.Names1 = new List<string>
                        {
                       "Cost Of Sales Items",
                       "Total Sales Income",
                        "Grand Profit"
                        };
                        List<GP> GP = GrandRes.OrderBy(x => x.Item_id).Select(x => new GP
                        {
                            Item_name = x.Item_name,
                            Cost_Of_Sales_Items = x.Cost_of_qty_out,
                            Total_Sales_Income = x.Total_Revenue_from_sales,
                            Grand_Profit = x.Total_Revenue_from_sales - x.Cost_of_qty_out

                        }).ToList();
                        ViewBag.GrandProfit = GrandRes.Sum(x => x.Total_Revenue_from_sales - x.Cost_of_qty_out);
                        ViewBag.SalesMinusLastYear =
                            GrandRes.Sum(x => x.Total_Revenue_from_sales - x.Cost_of_qty_out)
                            -
                            Rpt.CalcGrandProfit(MyDbContext.Instance.Inv_item.ToList().DefaultIfEmpty(new FabulousDB.Models.Inv_item { Id = 0 }).FirstOrDefault().Id
                    , MyDbContext.Instance.Inv_item.OrderByDescending(x => x.Id).ToList().DefaultIfEmpty(new FabulousDB.Models.Inv_item { Id = 0 }).FirstOrDefault().Id,
                    null, new DateTime(DateTime.Now.Year - 1, 1, 1), new DateTime(DateTime.Now.Year - 1, 12, 31), null, true).ToList().DefaultIfEmpty(new GrandProfit
                    {
                        Total_Revenue_from_sales = 0,
                        Cost_of_qty_out = 0
                    }).Sum(x => x.Total_Revenue_from_sales - x.Cost_of_qty_out);
                        Chart.GP = GP;
                        ViewBag.SalesInvNum = MyDbContext.Instance.Inv_sales_invoice.Where(x => x.Doc_type == FabulousDB.Models.Doc_type.Invoice).Count();
                    }
                    catch
                    {

                    }
                    List<QtyAvaliable> QtyRes = new List<QtyAvaliable>();

                    try
                    {
                        QtyRes = ViewBag.QtyRes = Rpt.CalcQtyAvaliable(true);
                        ViewBag.Names2 = new List<string>
                    {
                       "Quantity In",
                       "Quantity Out",
                       "Quantity Avaliable"
                    };
                        List<ChartQtyAvaliable> QA =
                            QtyRes.OrderBy(z => z.Item_id).Select(x => new ChartQtyAvaliable
                            {
                                Quantity_In = x.Total_qty_in,
                                Quantity_Out = x.Total_qty_out,
                                Quantity_Avaliable = x.Total_qty_in - x.Total_qty_out,
                                Item_name = x.Item_name
                            }).ToList();
                        Chart.QA = QA;
                    }
                    catch
                    {

                    }
                    List<string> Columns;

                    ViewBag.Names3 = new List<string>
                    {
                       "Sales",
                       "Discount",
                       "Total Sales"
                    };

                    int YearId = ViewBag.YearId = 0;
                    try
                    {
                        YearId = ViewBag.YearId = MyDbContext.Instance.NewFiscalYear_Table.FirstOrDefault(x => x.Closed != false).YearID;

                    }
                    catch
                    {

                    }

                    List<MonthlySalesRpt> MonthInvSalesRes = Rpt.CalcMonthlySalesRes(false, false, YearId, "Month", null, null, out Columns);
                    List<ChartMonthlySales> Ms = MonthInvSalesRes.Select(x => new ChartMonthlySales
                    {
                        Discount = x.Discount,
                        Total_Sales = x.Sales - x.Discount,
                        Sales = x.Sales,
                        Item_name = x.Col
                    }).ToList();
                    Chart.MS1 = Ms;

                    List<MonthlySalesRpt> MonthRetSalesRes = Rpt.CalcMonthlySalesRes(false, true, YearId, "Month", null, null, out Columns);

                    List<ChartMonthlySales> Ms2 = MonthRetSalesRes.Select(x => new ChartMonthlySales
                    {
                        Discount = x.Discount,
                        Total_Sales = x.Sales - x.Discount,
                        Sales = x.Sales,
                        Item_name = x.Col
                    }).ToList();
                    Chart.MS2 = Ms2;

                    List<MonthlySalesRpt> MonthInvPurchasRes = Rpt.CalcMonthlySalesRes(true, false, YearId, "Month", null, null, out Columns);

                    List<ChartMonthlySales> Ms3 = MonthInvPurchasRes.Select(x => new ChartMonthlySales
                    {
                        Discount = x.Discount,
                        Total_Sales = x.Sales - x.Discount,
                        Sales = x.Sales,
                        Item_name = x.Col
                    }).ToList();
                    Chart.MS3 = Ms3;


                    List<MonthlySalesRpt> MonthRetPurchasRes = Rpt.CalcMonthlySalesRes(true, true, YearId, "Month", null, null, out Columns);

                    List<ChartMonthlySales> Ms4 = MonthRetPurchasRes.Select(x => new ChartMonthlySales
                    {
                        Discount = x.Discount,
                        Total_Sales = x.Sales - x.Discount,
                        Sales = x.Sales,
                        Item_name = x.Col
                    }).ToList();
                    Chart.MS4 = Ms4;

                    ///////////////////////////

                    ViewBag.Names4 = new List<string>
                {
                   "Amount"
                };

                    List<SalesAndDebit> VendoreBalance = Rpt.CalcCustomerSales(true, null, null, MyDbContext.Instance.Payable_creditor_setting.FirstOrDefault().Id,
                        MyDbContext.Instance.Payable_creditor_setting.OrderByDescending(x => x.Id).FirstOrDefault().Id
                        , true);
                    List<ChartVendorBalanc> VB = VendoreBalance.SelectMany(x => x.Debit).Select(x => new ChartVendorBalanc
                    {
                        Vendore = x.Customer_name,
                        Amount = x.Amount
                    }).ToList();
                    List<ChartVendorBalanc> VPurchase = VendoreBalance.SelectMany(x => x.Sales).Select(x => new ChartVendorBalanc
                    {
                        Vendore = x.Customer_name,
                        Amount = x.Amount
                    }).ToList();
                    Chart.VPurchase = VPurchase;
                    Chart.VB = VB;



                    List<SalesAndDebit> CustomerBalance = Rpt.CalcCustomerSales(false, null, null, MyDbContext.Instance.Receivable_vendore_settings.FirstOrDefault().Id,
                        MyDbContext.Instance.Receivable_vendore_settings.OrderByDescending(x => x.Id).FirstOrDefault().Id
                        , true);

                    List<ChartCustomerBalanc> CB = CustomerBalance.SelectMany(x => x.Debit).Select(x => new ChartCustomerBalanc
                    {
                        Customer = x.Customer_name,
                        Amount = x.Amount
                    }).ToList();
                    List<ChartCustomerBalanc> CPurchase = CustomerBalance.SelectMany(x => x.Sales).Select(x => new ChartCustomerBalanc
                    {
                        Customer = x.Customer_name,
                        Amount = x.Amount
                    }).ToList();
                    Chart.CPurchase = CPurchase;
                    Chart.CB = CB;
                }
            }
            catch
            {

            }
            return View(Chart);
        }
        // GET: Dashboard
        public ActionResult dashboard()
        {
            return View();
        }
    }
}