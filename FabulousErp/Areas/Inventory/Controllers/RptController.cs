using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousDB.Models;
using FabulousDB.Models.Inventory;
using FabulousErp.Bussiness;
using FabulousErp.Payable.Models;
using FabulousErp.Receivable.Models;
using FabulousModels.Inventory;
using FabulousModels.Inventory;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.Models;
using Remotion.Logging;
using WebGrease.Css.Extensions;

namespace Inventory.Controllers
{
    public class RptController : Controller
    {
        DBContext db = new DBContext();
        // GET: Inventory/Rpt
        public ActionResult CompareQutation()
        {
            return View();
        }
        public PartialViewResult CompareRes(string Pr_no, int Compare)
        {
            List<CompareQutaion> InvPur = db.Inv_purchase.Where(x => x.Request == Pr_no)
                .Select(x => new CompareQutaion
                {
                    Id = x.Id,
                    Currency = "",

                }).ToList();
            return PartialView();
        }
        public ActionResult TrnsferItem()
        {
            return View();
        }



        public ActionResult CardRpt(CardRptTypr Type)
        {
            if (Type == CardRptTypr.Item)
            {
                ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            }
            ViewBag.Site_id = new SelectList(new List<string> { });
            ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");
            ViewBag.Item_group = new SelectList(db.Inv_item_group.ToList(), "Id", "Item_group_id");
            ViewBag.Title = Type;
            return View();
        }
        public ActionResult CardRptRes(/*CardRptTypr Type, int Store_id,*/ int Id)
        {
            List<CardRpt> Res = new List<CardRpt>();
            //GR
            try
            {
                Res.AddRange(db.Inv_receive_po_items.Include(x => x.Receive_po)
                    .Where(x => x.Item_id == Id &&
                      (x.Receive_po.Doc_type == Doc_type.Invoice || x.Receive_po.Doc_type == Doc_type.Return)
                      )
                         .Include(x => x.Receive_po)
                         .Include(x => x.Receive_po.Store)
                         .Include(x => x.UOM)
                         .Include(x => x.Receive_po.Payable)
                         .Include(x => x.Receive_po.Receivable)
                         .Include(x => x.Item.Unit_of_measure)
                         .ToList().Select(x => new CardRpt
                         {
                             Id = x.Item_id,
                             Qty = x.Quantity,
                             UnitCost = x.Unit_price,
                             Total = Convert.ToDecimal(x.Quantity) * x.Unit_price,
                             Avaliable = x.Quantity,
                             Action_date = x.Receive_po.Creation_date,
                             Action_num = x.Receive_po.Gr_num.Value,
                             Type = GRGO.GR,
                             Discount = x.Discount,
                             Fright = x.Fright,
                             Po_receive_id = x.Receive_po_id,
                             UOM = (x.UOM != null) ? x.UOM.Unit_id : x.Item.Unit_of_measure.Unit_id,
                             Store = x.Receive_po.Store.Store_id,
                             Doc_date =(x.Receive_po.Payable!=null)? x.Receive_po.Payable.Doc_date.ToShortDateString(): (x.Receive_po.Receivable!=null)?x.Receive_po.Receivable.Doc_date.ToShortDateString():"" /*x.Receive_po.Doc_date.ToShortDateString()*/
                         }));

            }
            catch
            {

            }
            // Transfer_in
            try
            {
              
                Res.AddRange(db.Inv_receive_po_items.Include(x => x.Receive_po)
                   .Where(x => x.Item_id == Id && x.Receive_po.Doc_type == Doc_type.Transfer)
                        .Include(x => x.Receive_po)
                        .Include(x => x.Receive_po.Transfer)
                        .Include(x => x.Receive_po.Store)
                        .Include(x => x.UOM)
                        .Include(x => x.Item.Unit_of_measure)
                        .ToList()
                         .Select(x => new CardRpt
                        {
                            Id = x.Item_id,
                            Qty = x.Quantity,
                            UnitCost = x.Unit_price,
                            Total = Convert.ToDecimal(x.Quantity) * x.Unit_price,
                            Avaliable = x.Quantity +
                                    Res.Where(z => z.Id == x.Item_id && z.Type == GRGO.GR && z.Po_receive_id.Value == x.Receive_po_id)
                                    .Sum(z => z.Qty),
                            Action_date = x.Receive_po.Creation_date,
                            Action_num = x.Receive_po.Gr_num.Value,
                            Type = GRGO.Transfer_in,
                            Discount = x.Discount,
                            Fright = x.Fright,
                            Po_receive_id = x.Receive_po_id,
                            UOM = (x.UOM != null) ? x.UOM.Unit_id : x.Item.Unit_of_measure.Unit_id,
                            Store = x.Receive_po.Store.Store_id,
                            Doc_date =(x.Receive_po.Transfer.Any())?x.Receive_po.Transfer.FirstOrDefault().Transfer_date.ToShortDateString():x.Receive_po.Creation_date.ToShortDateString()/*x.Receive_po.Doc_date.ToShortDateString()*/
                        }));


                //Res.AddRange(db.Inv_po_item_transfer.Include(x => x.Po)
                //.Where(x => x.Item_id == Id)
                //     .Include(x => x.Po.Items)
                //     .Include(x => x.Po.Store)
                //     .Include(x => x.Po.Items.Select(z => z.UOM))
                //     .Include(x => x.Relations)
                //     .Include(x => x.To_store)
                //     .ToList().Select(x => new CardRpt
                //     {
                //         Id = x.Item_id,
                //         Qty = x.Transfer_qty,
                //         UnitCost = x.Po.Items.ToList().FirstOrDefault(z => z.Item_id == Id && z.Receive_po_id == x.Relations.Where(y => y.Transfer_id == x.Id).FirstOrDefault().Receive_po_id).Unit_price,
                //         Total = Convert.ToDecimal(x.Po.Items.FirstOrDefault(z => z.Item_id == Id).Quantity) *
                //         x.Po.Items.FirstOrDefault(z => z.Item_id == Id).Unit_price,
                //         Avaliable = x.Transfer_qty +
                //        Res.Where(z => z.Id == x.Item_id && z.Type == GRGO.GR && z.Po_receive_id.Value == x.Relations.FirstOrDefault().Receive_po_id)
                //        .Sum(z => z.Qty),
                //         Action_date = x.Po.Creation_date,
                //         Action_num = x.Po.Gr_num.Value,
                //         Type = GRGO.Transfer_in,
                //         Discount = x.Po.Items.FirstOrDefault(z => z.Item_id == Id).Discount,
                //         Fright = x.Po.Items.FirstOrDefault(z => z.Item_id == Id).Fright,
                //         Po_receive_id = x.Relations.FirstOrDefault().Receive_po_id,
                //         UOM = x.Po.Items.FirstOrDefault(z => z.Item_id == Id).UOM.Unit_id,
                //         TakeUnit = true,
                //         Store = x.To_store.Store_id
                //     }));

            }
            catch
            {

            }
            //Adjustment_in
            try
            {
                Res.AddRange(db.Inv_receive_po_items.Include(x => x.Receive_po)
                  .Where(x => x.Item_id == Id && x.Receive_po.Doc_type == Doc_type.Adjustment)
                       .Include(x => x.Receive_po)
                       .Include(x => x.Receive_po.Adjustment)
                       .Include(x => x.Receive_po.Store)
                       .Include(x => x.UOM)
                       .Include(x => x.Item.Unit_of_measure)
                       .ToList().Select(x => new CardRpt
                       {
                           Id = x.Item_id,
                           Qty = x.Quantity,
                           UnitCost = x.Unit_price,
                           Total = Convert.ToDecimal(x.Quantity) * x.Unit_price,

                           Avaliable = x.Quantity
                            + Res.Where(z => z.Id == x.Item_id && z.Type == GRGO.GR && z.Po_receive_id.Value == x.Receive_po_id)
                            .Sum(z => z.Qty)
                            + Res.Where(z => z.Id == x.Item_id && z.Type == GRGO.Transfer_in && z.Po_receive_id.Value == x.Receive_po_id)
                            .Sum(z => z.Qty),

                           Action_date = x.Receive_po.Creation_date,
                           Action_num = x.Receive_po.Gr_num.Value,
                           Type = GRGO.Adjustment_in,
                           Discount = x.Discount,
                           Fright = x.Fright,
                           Po_receive_id = x.Receive_po_id,
                           UOM = (x.UOM != null) ? x.UOM.Unit_id : x.Item.Unit_of_measure.Unit_id,
                           Store = x.Receive_po.Store.Store_id,
                           Doc_date = (x.Receive_po.Adjustment.Any())? x.Receive_po.Adjustment.FirstOrDefault().Adjustment_date.ToShortDateString():""
                       }));

                //Res.AddRange(db.Inv_item_adjustment.Include(x => x.Po)
                //.Where(x => x.Item_id == Id)
                //     .Include(x => x.Po.Items)
                //     .Include(x => x.Po.Store)
                //     .Include(x => x.Po.Items.Select(z => z.UOM))
                //     .Include(x => x.Relations)
                //     .ToList().Select(x => new CardRpt
                //     {
                //         Id = x.Item_id,
                //         Qty = x.Earn_qty,
                //         UnitCost = x.Po.Items.FirstOrDefault(z => z.Item_id == Id).Unit_price,
                //         Total = Convert.ToDecimal(x.Po.Items.FirstOrDefault(z => z.Item_id == Id).Quantity) *
                //         x.Po.Items.FirstOrDefault(z => z.Item_id == Id).Unit_price,

                //         Avaliable = x.Earn_qty
                //            + Res.Where(z => z.Id == x.Item_id && z.Type == GRGO.GR && z.Po_receive_id.Value == x.Relations.FirstOrDefault().Receive_po_id)
                //            .Sum(z => z.Qty)
                //            + Res.Where(z => z.Id == x.Item_id && z.Type == GRGO.Transfer_in && z.Po_receive_id.Value == x.Relations.FirstOrDefault().Receive_po_id)
                //            .Sum(z => z.Qty),

                //         Action_date = x.Po.Creation_date,
                //         Action_num = x.Po.Gr_num.Value,
                //         Type = GRGO.Adjustment_in,
                //         Discount = x.Po.Items.FirstOrDefault(z => z.Item_id == Id).Discount,
                //         Fright = x.Po.Items.FirstOrDefault(z => z.Item_id == Id).Fright,
                //         Po_receive_id = x.Relations.FirstOrDefault().Receive_po_id,
                //         UOM = x.Po.Items.FirstOrDefault(z => z.Item_id == Id).UOM.Unit_id,
                //         TakeUnit = true,
                //         Store = x.Po.Store.Store_id
                //     }));

            }
            catch
            {

            }
            //GO
            try
            {
                foreach (Inv_sales_receivs_pos i in db.Inv_sales_receivs_pos
                .Include(x => x.Item).Include(x => x.Receive_po).Include(x => x.Sales)
                .Include(x => x.Receive_po.Store)
                .Include(x => x.Receive_po.Items)
                .Include(x => x.Receive_po.Receivable)
                .Include(x => x.Sales.Receivable)
                .Where(x => x.Item_id == Id
                //&& (x.Receive_po.Doc_type == Doc_type.Invoice || x.Receive_po.Doc_type == Doc_type.Return)
                //&&(x.Receive_po.Store_id==Store_id||x.Sales.Store_id==Store_id)
                )
                .Include(x => x.Sales).Include(x => x.Receive_po).ToList())
                {
                    try
                    {
                        decimal CostPrice = i.Sales.Inv_sales_item.Where(x => x.Item_id == i.Item_id)
                           .ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Cost_price = 0 })
                           .FirstOrDefault().Cost_price.Value;
                        
                        Res.Add(new FabulousModels.Inventory.CardRpt
                        {
                            Id = i.Item_id,
                            Qty = i.Quantity,
                            UnitCost = CostPrice,
                            Total = Convert.ToDecimal(i.Quantity) * CostPrice,
                            Avaliable =
                            i.Receive_po.Items.Where(x => x.Item_id == i.Item_id).ToList().DefaultIfEmpty(new Inv_receive_po_items { Quantity = 0 }).FirstOrDefault().Quantity
                            - Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.GO && x.Po_receive_id == i.Receive_po_id).Sum(x => x.Qty)
                            - i.Quantity,
                            Action_date = i.Sales.Creation_date,
                            Action_num = i.Sales.Go_num.Value,
                            Type = GRGO.GO,
                            Discount = i.Sales.Discount,
                            Fright = i.Sales.Inv_sales_item.FirstOrDefault(x => x.Item_id == i.Item_id).Fright,
                            Sales_id = i.Sales_id,
                            Po_receive_id = i.Receive_po_id,
                            ItemId = i.Item.Item_id,
                            UOM = db.Inv_sales_invoice_items.Include(x => x.UOM).FirstOrDefault(x => x.Sales_invoice_id == i.Sales_id && x.Item_id == i.Item.Id).UOM.Unit_id,
                            Store = i.Receive_po.Store.Store_id,
                            Doc_date = (i.Sales.Receivable != null) ? i.Sales.Receivable.Doc_date.ToShortDateString() : ""
                        });

                    }
                    catch
                    {

                    }
                    
                }
            }
            catch (Exception e)
            {

            }
            //Transfer_out
            try
            {
                foreach (Inv_transfer_relation i in db.Inv_transfer_relation
               .Include(x => x.Receive_po).Include(x => x.Receive_po.Items)
               .Include(x => x.Receive_po.Store)
               .Include(x => x.Item)
               .Include(x => x.Main_po).Include(x => x.Main_po.Items).Include(x => x.Transfer)
               .Where(x => x.Item_id == Id

               //&&(x.Receive_po.Store_id==Store_id||x.Sales.Store_id==Store_id)
               ).ToList())
                {

                    Res.Add(new FabulousModels.Inventory.CardRpt
                    {
                        Id = i.Item_id,
                        Qty = i.Quantity,
                        UnitCost = i.Receive_po.Items.FirstOrDefault(x => x.Item_id == Id).Unit_price,
                        Total = Convert.ToDecimal(i.Transfer.Transfer_qty) * i.Main_po.Items.FirstOrDefault(x => x.Item_id == Id).Unit_price,
                        Avaliable =
                        Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.GR && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        + Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.Transfer_in && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        + Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.Adjustment_in && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        - Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.GO && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        - i.Quantity
                        - Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.Transfer_out && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        ,
                        Action_date = i.Receive_po.Creation_date,
                        Action_num = i.Receive_po.Gr_num.Value,
                        Type = GRGO.Transfer_out,
                        Discount = i.Receive_po.Discount,
                        Fright = i.Receive_po.Items.Where(x => x.Item_id == i.Item_id).ToList().DefaultIfEmpty(new Inv_receive_po_items { Fright = 0 }).FirstOrDefault().Fright,
                        Sales_id = i.Receive_po_id,
                        Po_receive_id = i.Main_po_id,
                        ItemId = i.Item.Item_id,
                        UOM = i.Receive_po.Items.Where(z => z.Item_id == Id).ToList().DefaultIfEmpty(new Inv_receive_po_items { UOM = new Unit_of_measure { Unit_id = "" } }).FirstOrDefault().UOM.Unit_id,
                        TakeUnit = true,
                        Store = i.Main_po.Store.Store_id,
                        Doc_date =i.Transfer.Transfer_date.ToShortDateString() /*i.Receive_po.Doc_date.ToShortDateString()*/
                    });
                }
            }
            catch (Exception ex)
            {

            }
            //Adjustment_out
            try
            {
                foreach (Inv_adjustment_relation i in db.Inv_adjustment_relation
               .Include(x => x.Receive_po)
               .Include(x => x.Receive_po.Items)
               .Include(x => x.Receive_po.Store)
               .Include(x => x.Item)
               .Include(x => x.Main_po)
               .Include(x => x.Main_po.Items).Include(x => x.Adjustment)
               .Where(x => x.Item_id == Id

               //&&(x.Receive_po.Store_id==Store_id||x.Sales.Store_id==Store_id)
               ).ToList())
                {
                    Res.Add(new FabulousModels.Inventory.CardRpt
                    {
                        Id = i.Item_id,
                        Qty = i.Quantity,
                        UnitCost = i.Main_po.Items.FirstOrDefault(x => x.Item_id == Id).Unit_price,
                        Total = (Convert.ToDecimal(i.Adjustment.Damage_qty) + (decimal)i.Adjustment.Loss_qty) * i.Main_po.Items.FirstOrDefault(x => x.Item_id == Id).Unit_price,
                        Avaliable =
                        Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.GR && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        + Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.Transfer_in && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        + Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.Adjustment_in && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        - Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.GO && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        - i.Quantity
                        - Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.Transfer_out && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        - Res.Where(x => x.Id == i.Item_id && x.Type == GRGO.Adjustment_out && x.Po_receive_id == i.Main_po_id).Sum(x => x.Qty)
                        ,
                        Action_date = i.Receive_po.Creation_date,
                        Action_num = i.Receive_po.Gr_num.Value,
                        Type = GRGO.Adjustment_out,
                        Discount = i.Main_po.Discount,
                        Fright = i.Main_po.Items.FirstOrDefault(x => x.Item_id == i.Item_id).Fright,
                        Sales_id = i.Receive_po_id,
                        Po_receive_id = i.Main_po_id,
                        ItemId = i.Item.Item_id,
                        UOM = i.Main_po.Items.FirstOrDefault(z => z.Item_id == Id).UOM.Unit_id,
                        TakeUnit = true,
                        Store = i.Receive_po.Store.Store_id,
                        Doc_date = i.Receive_po.Doc_date.ToShortDateString()
                    });
                }
            }
            catch
            {

            }
            //.ThenByDescending(x => x.Action_date)
            return View(Res.OrderBy(x => x.Po_receive_id).ToList());
        }
        public ActionResult StockRpt(CardRptTypr Type)
        {
            if (Type == CardRptTypr.Item)
            {
                ViewBag.MainSel = new SelectList(db.Inv_item, "Id", "Item_id");
            }
            ViewBag.Site_id = new SelectList(new List<string> { });
            ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");
            ViewBag.Item_group = new SelectList(db.Inv_item_group.ToList(), "Id", "Item_group_id");
            ViewBag.Title = Type;
            return View();
        }
        public ActionResult StockRptRes(CardRptTypr Type, int Id)
        {
            List<StockRpt> Res = new List<StockRpt>();
            if (Type == CardRptTypr.Store)
            {
                try
                {

                    foreach (Inv_receive_po_items Item in db.Inv_receive_po_items.Include(x => x.Item)
                    .Include(x => x.Item.Unit_of_measure).Include(x => x.Receive_po)
                    .Where(x => x.Receive_po.Store_id == Id).ToList())
                    {
                        if (!Res.Any(x => x.Item_id == Item.Item.Item_id))
                        {
                            ItemDetails D = InvBus.CalcItemDetails(Item.Item_id,
                             InvBus.GetItemAvaliable(Item.Item_id),
                              Item.Receive_po.Store_id,
                              null, false, Item.UOM_id);
                            decimal UnitCost = 0;
                            float Qty = 0;

                            Res.Add(new FabulousModels.Inventory.StockRpt
                            {
                                Item_id = Item.Item.Item_id,
                                Item_name = Item.Item.Short_description,
                                UOM = Item.Item.Unit_of_measure.Unit_id,
                                Avaliable = InvBus.GetItemAvaliableStore(Item.Item.Id, Id), //x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity),
                            });
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            else if (Type == CardRptTypr.Site)
            {
                try
                {
                    foreach (Inv_receive_po_items Item in db.Inv_receive_po_items.Include(x => x.Item)
                             .Include(x => x.Item.Unit_of_measure).Include(x => x.Receive_po)
                             .Where(x => x.Receive_po.Site_id == Id).ToList())
                    {
                        if (!Res.Any(x => x.Item_id == Item.Item.Item_id))
                        {
                            decimal UnitCost = 0;
                            float Qty = 0;

                            Res.Add(new FabulousModels.Inventory.StockRpt
                            {
                                Item_id = Item.Item.Item_id,
                                Item_name = Item.Item.Short_description,
                                UOM = Item.Item.Unit_of_measure.Unit_id,
                                Avaliable = InvBus.GetItemAvaliableSite(Item.Item.Id, Id), //x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity),
                            });
                        }
                    }

                }
                catch (Exception ex)
                { }
            }


            Res.RemoveAll(x => x.Avaliable == 0);
            return View(Res.Distinct());


            //if (Type == CardRptTypr.Item)
            //{
            //    try
            //    {
            //        foreach (Inv_receive_po_items Item in db.Inv_receive_po_items.Include(x => x.Item)
            //            .Include(x => x.Item.Unit_of_measure).Include(x => x.Receive_po)
            //            .Distinct().Where(x => x.Item_id == Id).ToList())
            //        {
            //            if (!Res.Any(x => x.Item_id == Item.Item.Item_id))
            //            {
            //                ItemDetails D = InvBus.CalcItemDetails(Item.Item_id,
            //           InvBus.GetItemAvaliable(Item.Item_id),
            //            Item.Receive_po.Store_id,
            //            null, false, Item.UOM_id);
            //                decimal UnitCost = 0;
            //                float Qty = 0;
            //                Res.Add(new FabulousModels.Inventory.StockRpt
            //                {
            //                    Item_id = Item.Item.Item_id,
            //                    Item_name = Item.Item.Short_description,
            //                    UOM = Item.Item.Unit_of_measure.Unit_id,
            //                    Avaliable = InvBus.GetItemAvaliable(Id), //x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity),
            //                });
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    { }
            //}
            //else if (Type == CardRptTypr.Group)
            //{
            //    try
            //    {
            //        foreach (Inv_receive_po_items Item in db.Inv_receive_po_items.Include(x => x.Item)
            //          .Include(x => x.Item.Unit_of_measure).Include(x => x.Receive_po)
            //          .Where(x => x.Item.Item_group_id == Id).ToList())
            //        {
            //            ItemDetails D = InvBus.CalcItemDetails(Item.Item_id,
            //             InvBus.GetItemAvaliable(Item.Item_id),
            //              Item.Receive_po.Store_id,
            //              null, false, Item.UOM_id);
            //            decimal UnitCost = 0;
            //            float Qty = 0;
            //            Res.Add(new FabulousModels.Inventory.StockRpt
            //            {
            //                Item_id = Item.Item.Item_id,
            //                Item_name = Item.Item.Short_description,
            //                UOM = Item.Item.Unit_of_measure.Unit_id,
            //                Avaliable = D.Po_inv.Sum(x => x.Qty), //InvBus.GetItemAvaliable(Id), //x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity),
            //            });
            //        }
            //    }
            //    catch (Exception ex)
            //    { }
            //}

        }

        public ActionResult ItemStockDetails()
        {
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            return View();
        }
        [HttpPost]
        public ActionResult ItemStockDetailsRes(int Item_id)
        {
            List<ItemStockDetails> Res = db.Inv_receive_item_serial
                .Include(x => x.Item.Item).Where(x => x.Sold == false && x.Item.Item.Id == Item_id)
                .Include(x=>x.Expiry)
                .Select(x => new ItemStockDetails
                {
                    Expiery_date = x.Expiry.ToList(),
                    Serial_no = x.Serial,
                    Warranty_end = x.End_warranty,
                    Warranty_start = x.Start_warranty
                }).ToList();
            return View(Res);
        }

        public ActionResult ItemStockingInquery()
        {
            ViewBag.ItemSerial = new SelectList(db.Inv_receive_item_serial, "Serial", "Serial");
            return View();
        } 

        public ActionResult ItemStockingInqueryRes(string ItemSerial)
        {
            Inv_receive_item_serial IS = db.Inv_receive_item_serial
                .Include(x=>x.Po)
                .Include(x=>x.Item.Item)
                .Include(x=>x.Item.Item.Unit_of_measure)
                .FirstOrDefault(x => x.Serial == ItemSerial);
            Inv_sales_item_serial SS = db.Inv_sales_item_serial.Where(x => x.Serial == ItemSerial)
                .Include(x => x.Sales).ToList().DefaultIfEmpty(new Inv_sales_item_serial { Sales = new Inv_sales_invoice { Creation_date = new DateTime(1, 1, 1) } })
                .FirstOrDefault();
            DateTime SalesCreationDate = SS.Sales.Creation_date;
            
            DateTime? SalesDate = null;
            if (SalesCreationDate != new DateTime(1, 1, 1))
            {
                SalesDate = SalesCreationDate;
            }

            FabulousModels.Inventory.ItemStockingInquery Res = new ItemStockingInquery() 
            { 
                Avaliable=InvBus.GetItemAvaliable(IS.Item_id),
                Item_id=IS.Item.Item.Item_id,
                Item_name=IS.Item.Item.Short_description,
                PurchaseDate=IS.Po.Creation_date,
                SalesDate= SalesDate,
                UOM=IS.Item.Item.Unit_of_measure.Unit_id,
                Pur_we=IS.End_warranty,
                Pur_ws=IS.Start_warranty,
                Sales_we= SS.End_warranty,
                Sales_ws=SS.Start_warranty
            };
            return View(Res);
        }

        public ActionResult AjustmentInquery()
        {
            //ViewBag.Site_id = new SelectList(new List<string> { });
            //ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");

            ViewBag.Adjust_num = new SelectList(db.Inv_item_adjustment.GroupBy(x => x.Adjustment_num).SelectMany(x => x.ToList())
               .Select(x => new { x.Adjustment_num }).Distinct(), "Adjustment_num", "Adjustment_num");

            return View();
        }
        public ActionResult AjustmentInqueryRes(int Adjust_num)
        {
            List<Inv_item_adjustment> Trans = db.Inv_item_adjustment
                .Where(x => x.Adjustment_num == Adjust_num)
                .Include(x => x.Item)
                .Include(x => x.Relations)
                .Include(x => x.Relations.Select(z => z.Receive_po))
                .Include(x => x.Relations.Select(z => z.Receive_po.Items))
                .Include(x => x.UOM)
                .ToList();
            ViewBag.PostingNumber = Trans.FirstOrDefault().Posting_num;
            return View(Trans);
        }
        public ActionResult StockingInquery()
        {
            ViewBag.Site_id = new SelectList(new List<string> { });
            ViewBag.Item_id = new SelectList(db.Inv_item.ToList(), "Id", "Item_id");
            ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");


            return View();
        }
        public ActionResult StockingInqueryRes(int Item_id)
        {
            List<Inv_stocking> Stock = db.Inv_stocking
                .Where(x => x.Item_id == Item_id)
                .Include(x => x.Item)
                .Include(x => x.UOM)
                .Include(x => x.Site)
                .Include(x => x.Site.Store)
                .ToList();
            return View(Stock);
        }
        public ActionResult AdjustmentRes(int? Store_id, int? Site_id)
        {
            List<StockRpt> Res = new List<StockRpt>();
            if (Site_id.HasValue)
            {
                try
                {
                    Res.AddRange(db.Inv_receive_po_items.Include(x => x.Item)
                         .Where(x => x.Item.Item_store_site.Any(z => z.Site_id == Site_id.Value))
                         .Include(x => x.Receive_po)
                         .Include(x => x.UOM).ToList().GroupBy(x => x.Item_id)
                         .ToList().Select(x => new StockRpt
                         {
                             Id = x.FirstOrDefault().Item.Id,
                             Item_id = x.FirstOrDefault().Item.Item_id,
                             Item_name = x.FirstOrDefault().Item.Description,
                             UOM = (x.FirstOrDefault().UOM != null) ? x.FirstOrDefault().UOM.Unit_id : "",
                             Avaliable = x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity),
                             Unit_cost = (x.Sum(z => z.Fright) - x.Sum(z => z.Discount) + x.Sum(z => z.Total)) / (decimal)x.Sum(z => z.Quantity),
                             Amount = (decimal)(x.Sum(z => z.Fright) - x.Sum(z => z.Discount) + x.Sum(z => z.Total)), /// (decimal)x.Sum(z => z.Quantity) * (decimal)(x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity))
                             Po = x.FirstOrDefault().Receive_po_id
                         }));

                }
                catch (Exception ex)
                { }
            }
            else if (Store_id.HasValue)
            {
                try
                {
                    Res.AddRange(db.Inv_receive_po_items.Include(x => x.Item)
                         .Where(x => x.Item.Item_store_site.Any(z => z.Store_id == Store_id.Value))
                         .Include(x => x.Receive_po)
                         .Include(x => x.UOM).ToList().GroupBy(x => x.Item_id)
                         .ToList().Select(x => new StockRpt
                         {
                             Id = x.FirstOrDefault().Item.Id,
                             Item_id = x.FirstOrDefault().Item.Item_id,
                             Item_name = x.FirstOrDefault().Item.Description,
                             UOM = (x.FirstOrDefault().UOM != null) ? x.FirstOrDefault().UOM.Unit_id : "",
                             Avaliable = x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity),
                             Unit_cost = (x.Sum(z => z.Fright) - x.Sum(z => z.Discount) + x.Sum(z => z.Total)) / (decimal)x.Sum(z => z.Quantity),
                             Amount = (decimal)(x.Sum(z => z.Fright) - x.Sum(z => z.Discount) + x.Sum(z => z.Total)),
                             Po = x.FirstOrDefault().Receive_po_id
                             /// (decimal)x.Sum(z => z.Quantity) * (decimal)(x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity))
                         }));

                }
                catch (Exception ex)
                { }
            }
            return View(Res);
        }
        //public ActionResult Stocking()
        //{
        //    ViewBag.Site_id = new SelectList(new List<string> { });
        //    ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");
        //    return View();
        //}
        //public ActionResult StockingRes(int? Store_id, int? Site_id)
        //{
        //    List<StockRpt> Res = new List<StockRpt>();
        //    if (Site_id.HasValue)
        //    {
        //        try
        //        {
        //            Res.AddRange(db.Inv_receive_po_items.Include(x => x.Item)
        //                 .Where(x => x.Item.Item_store_site.Any(z => z.Site_id == Site_id.Value))
        //                 .Include(x => x.Receive_po)
        //                 .Include(x => x.UOM).ToList().GroupBy(x => x.Item_id)
        //                 .ToList().Select(x => new StockRpt
        //                 {
        //                     Id=x.FirstOrDefault().Item.Id,
        //                     Item_id = x.FirstOrDefault().Item.Item_id,
        //                     Item_name = x.FirstOrDefault().Item.Description,
        //                     UOM = (x.FirstOrDefault().UOM != null) ? x.FirstOrDefault().UOM.Unit_id : "",
        //                     Avaliable = x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity),
        //                     Unit_cost = (x.Sum(z => z.Fright) - x.Sum(z => z.Discount) + x.Sum(z => z.Total)) / (decimal)x.Sum(z => z.Quantity),
        //                     Amount = (decimal)(x.Sum(z => z.Fright) - x.Sum(z => z.Discount) + x.Sum(z => z.Total)), /// (decimal)x.Sum(z => z.Quantity) * (decimal)(x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity))
        //                     Po=x.FirstOrDefault().Receive_po_id
        //                 }));

        //        }
        //        catch (Exception ex)
        //        { }
        //    }
        //    else if (Store_id.HasValue)
        //    {
        //        try
        //        {
        //            Res.AddRange(db.Inv_receive_po_items.Include(x => x.Item)
        //                 .Where(x => x.Item.Item_store_site.Any(z => z.Store_id == Store_id.Value))
        //                 .Include(x => x.Receive_po)
        //                 .Include(x => x.UOM).ToList().GroupBy(x => x.Item_id)
        //                 .ToList().Select(x => new StockRpt
        //                 {
        //                     Id = x.FirstOrDefault().Item.Id,
        //                     Item_id = x.FirstOrDefault().Item.Item_id,
        //                     Item_name = x.FirstOrDefault().Item.Description,
        //                     UOM = (x.FirstOrDefault().UOM != null) ? x.FirstOrDefault().UOM.Unit_id : "",
        //                     Avaliable = x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity),
        //                     Unit_cost = (x.Sum(z => z.Fright) - x.Sum(z => z.Discount) + x.Sum(z => z.Total)) / (decimal)x.Sum(z => z.Quantity),
        //                     Amount = (decimal)(x.Sum(z => z.Fright) - x.Sum(z => z.Discount) + x.Sum(z => z.Total)),
        //                     Po=x.FirstOrDefault().Receive_po_id
        //                     /// (decimal)x.Sum(z => z.Quantity) * (decimal)(x.Sum(z => z.Quantity) - db.Inv_sales_invoice_items.Where(z => z.Item_id == x.Key).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity))
        //                 }));

        //        }
        //        catch (Exception ex)
        //        { }
        //    }
        //    return View(Res);
        //}
        public enum CardRptTypr
        {
            Item = 1,
            Group = 2,
            Site = 3,
            Store = 4
        }
        public ActionResult PoItems()
        {
            return View();
        }

        public ActionResult PoItemsPrint(int PoId, bool IsPo, Nullable<Doc_type> Doc_type = null, bool IsSales = false)
        {
            List<PoItemsPrint> Res = new List<PoItemsPrint>();
            if (IsPo)
            {
                if (Doc_type == FabulousDB.Models.Doc_type.Return && !IsSales)
                {
                    Inv_sales_invoice Po = db.Inv_sales_invoice
                  .Include(x => x.Payable).Include(x => x.Payable.Trans_doc_type).Include(x => x.Payable.Vendor)
                  .FirstOrDefault(x => x.Id == PoId);

                    ViewBag.Title = FabulousErp.BusController.Translate("Goods Out ") + Po.Payable.Trans_doc_type.Counter;
                    try
                    {
                        ViewBag.TrDate = Po.Payable.Transaction_date;

                    }
                    catch
                    {
                        try
                        {
                            ViewBag.TrDate = db.Inv_po_item_transfer.FirstOrDefault(x => x.Po_id == PoId).Transaction_date;
                        }
                        catch
                        {

                        }
                    }
                    ViewBag.VCName = FabulousErp.BusController.Translate("Customer Name");
                    ViewBag.VCNameV = Po.Payable.Vendor.Vendor_name;
                    ViewBag.DocDate = Po.Doc_date;
                    ViewBag.DocNum = Po.Payable.VDocument_number;
                    ViewBag.ShowDocNum = true;

                    Res.AddRange(db.Inv_sales_invoice_items.Where(x => x.Sales_invoice_id == PoId)
                                .Include(x => x.Item).Include(x => x.UOM).ToList().Select(x => new PoItemsPrint
                                {
                                    Item_id = x.Item.Item_id,
                                    Item_name = x.Item.Description,
                                    Qty = x.Quantity,
                                    UOM = x.UOM.Unit_id
                                }).ToList());
                    ViewBag.ItemSerial = db.Inv_sales_item_serial.Where(x => x.Sales_id == PoId)
                        .Include(x => x.Item.Item).Select(x => new ItemSerial
                        {
                            Item_id = x.Item.Item.Item_id,
                            Serial = x.Serial,
                           // Expiery_date = x.Expiry_date,
                            Warranty_start = x.Start_warranty,
                            Warranty_end = x.End_warranty,
                            Has_expiery = x.Item.Item.Has_expiry_date,
                            Has_warranty = x.Item.Item.Has_warranty
                        }).ToList();
                }
                else
                {
                    Inv_receive_po Po = db.Inv_receive_po.Include(x => x.Vendore)
                   .Include(x => x.Payable)
                   .Include(x => x.Receivable).Include(x => x.Receivable.Vendor)
                   .FirstOrDefault(x => x.Id == PoId);
                    ViewBag.Title = FabulousErp.BusController.Translate("Goods Receipts");

                    if (Po.Doc_type == FabulousDB.Models.Doc_type.Transfer)
                    {
                        ViewBag.Title = FabulousErp.BusController.Translate("Transfer");
                    }
                    ViewBag.GrGo = FabulousErp.BusController.Translate("GR Num");
                    ViewBag.GrGoV = Po.Gr_num;
                    if (Doc_type == FabulousDB.Models.Doc_type.Return)
                    {
                        ViewBag.TrDate = Po.Transaction_date;
                        ViewBag.VCName = FabulousErp.BusController.Translate("Customer Name");
                        ViewBag.DocDate = Po.Doc_date;
                        try
                        {
                            ViewBag.VCNameV = Po.Receivable.Vendor.Vendor_name;
                            ViewBag.DocNum = Po.Receivable.VDocument_number;
                        }
                        catch
                        {

                        }
                      
                    }
                    else
                    {
                        try
                        {
                            ViewBag.TrDate = Po.Payable.Transaction_date;

                        }
                        catch
                        {
                            try
                            {
                                ViewBag.TrDate = db.Inv_po_item_transfer.FirstOrDefault(x => x.Po_id == PoId).Transaction_date;
                            }
                            catch
                            {

                            }
                        }
                        try
                        {
                            ViewBag.VCNameV = Po.Vendore.Vendor_name;
                            ViewBag.DocNum = Po.Payable.VDocument_number;
                        }
                        catch
                        {

                        }
                        ViewBag.VCName = FabulousErp.BusController.Translate("Vendore Name");
                        ViewBag.DocDate = Po.Doc_date;
                    }

                    ViewBag.ShowDocNum = true;

                    FabulousDB.Models.Doc_type ThisDoc_type = db.Inv_receive_po.FirstOrDefault(x => x.Id == PoId).Doc_type.Value;

                    //if (ThisDoc_type == FabulousDB.Models.Doc_type.Invoice
                    //    || ThisDoc_type == FabulousDB.Models.Doc_type.Return)
                    {
                        Res.AddRange(db.Inv_receive_po_items.Where(x => x.Receive_po_id == PoId)
                              .Include(x => x.Item).Include(x => x.Item.Unit_of_measure).Include(x => x.UOM).ToList().Select(x => new PoItemsPrint
                              {
                                  Item_id = x.Item.Item_id,
                                  Item_name = x.Item.Description,
                                  Qty = InvBus.CalcItemEq(x.Item.Id, x.UOM_id, x.Quantity, false),
                                  UOM = (x.UOM != null) ? x.UOM.Unit_id : x.Item.Unit_of_measure.Unit_id
                              }).ToList());
                    }
                    //else if (ThisDoc_type == FabulousDB.Models.Doc_type.Transfer)
                    //{
                    //    foreach (var i in db.Inv_transfer_relation.Where(x => x.Receive_po_id == PoId)
                    //          .Include(x => x.Transfer)
                    //          .Include(x => x.Transfer.UOM)
                    //          .Include(x => x.Item).Include(x => x.Item.Unit_of_measure).ToList().GroupBy(x=>x.Item_id))
                    //    {
                    //        Res.Add( new PoItemsPrint
                    //         {
                    //             Item_id = i.FirstOrDefault().Item.Item_id,
                    //             Item_name = i.FirstOrDefault().Item.Description,
                    //             Qty = InvBus.CalcItemEq(i.FirstOrDefault().Item.Id, i.FirstOrDefault().Transfer.UOM_id, i.ToList().Sum(z=>z.Quantity), false),
                    //             UOM = i.FirstOrDefault().Transfer.UOM.Unit_id
                    //         });
                    //    };
                    //}
                    //else if (ThisDoc_type == FabulousDB.Models.Doc_type.Adjustment)
                    //{
                    //    Res.AddRange(db.Inv_adjustment_relation.Where(x => x.Receive_po_id == PoId)
                    //          .Include(x=>x.Adjustment)
                    //          .Include(x=>x.Adjustment.UOM_id)
                    //          .Include(x => x.Item).Include(x => x.Item.Unit_of_measure).ToList().Select(x => new PoItemsPrint
                    //          {
                    //              Item_id = x.Item.Item_id,
                    //              Item_name = x.Item.Description,
                    //              Qty = InvBus.CalcItemEq(x.Item.Id, x.Adjustment.UOM_id, x.Quantity, false),
                    //              UOM = x.Adjustment.UOM.Unit_id
                    //          }).ToList());
                    //}


                    ViewBag.ItemSerial = db.Inv_receive_item_serial.Where(x => x.Po_id == PoId)
                        .Include(x=>x.Expiry)
                        .Include(x => x.Item.Item).Select(x => new ItemSerial
                        {
                            Item_id = x.Item.Item.Item_id,
                            Serial = x.Serial,
                            Expiery_date = x.Expiry.ToList(),
                            Warranty_start = x.Start_warranty,
                            Warranty_end = x.End_warranty,
                            Has_expiery = x.Item.Item.Has_expiry_date,
                            Has_warranty = x.Item.Item.Has_warranty
                        }).ToList();
                }
            }
            else
            {
                Inv_sales_invoice Po = db.Inv_sales_invoice.Include(x => x.Receivable).Include(x => x.Customer)
                    .Include(x => x.Payable).Include(x => x.Payable.Vendor).FirstOrDefault(x => x.Id == PoId);
                ViewBag.Title = FabulousErp.BusController.Translate("Sales Items");

                ViewBag.GrGo = FabulousErp.BusController.Translate("GO Num");
                ViewBag.GrGoV = Po.Go_num;


                if (Doc_type == FabulousDB.Models.Doc_type.Return)
                {
                    ViewBag.TrDate = Po.Payable.Transaction_date;
                    ViewBag.VCName = FabulousErp.BusController.Translate("Vendore Name");
                    ViewBag.VCNameV = Po.Payable.Vendor.Vendor_name;
                    ViewBag.DocDate = Po.Doc_date;
                    ViewBag.DocNum = Po.Payable.VDocument_number;
                    ViewBag.ShowDocNum = true;

                    ViewBag.ItemSerial = db.Inv_receive_item_serial.Where(x => x.Po_id == PoId)
                   .Include(x => x.Item.Item).Select(x => new ItemSerial
                   {
                       Item_id = x.Item.Item.Item_id,
                       Serial = x.Serial,
                       Expiery_date = x.Expiry.ToList(),
                       Warranty_start = x.Start_warranty,
                       Warranty_end = x.End_warranty,
                       Has_expiery = x.Item.Item.Has_expiry_date,
                       Has_warranty = x.Item.Item.Has_warranty
                   }).ToList();

                    Res.AddRange(db.Inv_receive_po_items.Where(x => x.Receive_po_id == PoId)
                          .Include(x => x.Item).Include(x => x.UOM).ToList().Select(x => new PoItemsPrint
                          {
                              Item_id = x.Item.Item_id,
                              Item_name = x.Item.Description,
                              Qty = x.Quantity,
                              UOM = x.UOM.Unit_id
                          }).ToList());
                }
                else
                {
                    if (Po.Transaction_date.HasValue)
                    {
                        ViewBag.TrDate = Po.Transaction_date;
                    }
                    else
                    {
                        ViewBag.TrDate = Po.Receivable.Transaction_date;
                    }
                    try
                    {
                        ViewBag.VCNameV = Po.Receivable.Vendor.Vendor_name;
                        ViewBag.DocNum = Po.Receivable.VDocument_number;
                    }
                    catch
                    {

                    }
                    ViewBag.VCName = FabulousErp.BusController.Translate("Customer Name");
                    ViewBag.DocDate = Po.Doc_date;
                    ViewBag.ShowDocNum = true;

                    ViewBag.ItemSerial = db.Inv_sales_item_serial.Where(x => x.Sales_id == PoId)
                      .Include(x => x.Item).Select(x => new ItemSerial
                      {
                          Item_id = (x.Item.Custom_name != null) ? x.Item.Custom_name : x.Item.Item.Short_description,
                          Serial = x.Serial,
                          //Expiery_date = x.ex,
                          Warranty_start = x.Start_warranty,
                          Warranty_end = x.End_warranty,
                          Has_expiery = x.Item.Item.Has_expiry_date,
                          Has_warranty = x.Item.Item.Has_warranty
                      }).ToList();
                    Res.AddRange(db.Inv_sales_invoice_items.Where(x => x.Sales_invoice_id == PoId)
                            .Include(x => x.Item).Include(x => x.UOM).ToList().Select(x => new PoItemsPrint
                            {
                                Item_id = x.Item.Item_id,
                                Item_name = (x.Custom_name != null) ? x.Custom_name : x.Item.Short_description /*.Item.Description*/,
                                Qty = x.Quantity,
                                UOM = x.UOM.Unit_id
                            }).ToList());
                }




            }
            return View(Res);
        }

        public ActionResult GrandProfitRpt(bool IsGrandProfit = true)
        {
            if (IsGrandProfit)
            {
                ViewBag.FromItem_id = new SelectList(db.Inv_item, "Id", "Item_id");
                ViewBag.ToItemId = new SelectList(db.Inv_item, "Id", "Item_id");
                ViewBag.GroupItem = new SelectList(db.Inv_item_group, "Id", "Item_group_id");
                ViewBag.IsGrandProfit = IsGrandProfit;

            }
            else
            {
                ViewBag.ItemId = new SelectList(db.Inv_item, "Id", "Item_id");
            }
            ViewBag.IsGrandProfit = IsGrandProfit.ToString().ToLower();
            return View();
        }
        public ActionResult GrandProfitRes(int? FromItem_id, int? ToItemId, int? GroupItem,
            DateTime? DateFrom, DateTime? DateTo, int? ItemId, bool IsGrandProfit = true)
        {
            List<GrandProfit> Res = CalcGrandProfit(FromItem_id, ToItemId, GroupItem, DateFrom, DateTo, ItemId, IsGrandProfit);
            return View(Res);
        }

        public List<GrandProfit> CalcGrandProfit(int? FromItem_id, int? ToItemId, int? GroupItem, DateTime? DateFrom, DateTime? DateTo, int? ItemId, bool IsGrandProfit)
        {
            List<GrandProfit> Res = new List<GrandProfit>();
            if (IsGrandProfit)
            {
                if (GroupItem.HasValue)
                {
                    Res = db.Inv_sales_receivs_pos.Where(x => x.Item.Item_group_id == GroupItem.Value)
                      .Include(x => x.Sales).Include(x => x.Receive_po)
                      .Include(x => x.Receive_po.Payable)
                      .Include(x => x.Receive_po.Items)
                      .Include(x => x.Sales.Receivable)
                      .Include(x => x.Sales.Receivable.Vendor)
                  .Include(x => x.Item).Where(x => x.Sales.Receivable_id != null).ToList().Select(x => new GrandProfit
                  {
                      Id = x.Item_id,
                      Item_id = x.Item.Item_id,
                      Item_name = x.Item.Short_description,


                      Qty_out = InvBus.GetSoldItemsExReturn(x.Item_id, x.Sales_id),

                      Cost_of_qty_out =
                      InvBus.GetSoldItemUnitPrice(x.Item_id, x.Sales_id).Sum(z => z.CostPrice)
                      * (decimal)InvBus.GetSoldItemsExReturn(x.Item_id, x.Sales_id)
                            ,

                      Total_Revenue_from_sales =
                        InvBus.GetSoldItemUnitPrice(x.Item_id, x.Sales_id).Sum(z => z.UnitPrice)
                         * (decimal)InvBus.GetSoldItemsExReturn(x.Item_id, x.Sales_id)
                       ,
                      Transaction_date = x.Sales.Receivable.Transaction_date,
                      Doc_num = Convert.ToString(x.Sales.Go_num),
                      Doc_type = x.Sales.Doc_type,
                      Customer_name = x.Sales.Receivable.Vendor.Vendor_name
                  }).ToList();
                    Res = GetGrandProfitReturn(Res);

                    Res.GroupBy(x => x.Item_id).ForEach(x =>
                    {
                        x.FirstOrDefault().
                        Qty_out = x.Sum(z => z.Qty_out);
                        x.FirstOrDefault().
                        Cost_of_qty_out = x.Sum(z => z.Cost_of_qty_out);
                        x.FirstOrDefault().
                        Total_Revenue_from_sales = x.Sum(z => z.Total_Revenue_from_sales);
                    });

                    Res = Res.GroupBy(x => x.Item_id).Select(x => x.FirstOrDefault()).ToList();
                }
                else if (FromItem_id.HasValue && ToItemId.HasValue)
                {
                    Res = db.Inv_sales_receivs_pos.Where(x => x.Item_id >= FromItem_id && x.Item_id <= ToItemId)
                     .Include(x => x.Sales).Include(x => x.Receive_po)
                     .Include(x => x.Receive_po.Payable)
                     .Include(x => x.Receive_po.Items)
                     .Include(x => x.Sales.Receivable)
                     .Include(x => x.Sales.Receivable.Vendor)
                 .Include(x => x.Item).Where(x => x.Sales.Receivable_id != null).ToList().Select(x => new GrandProfit
                 {
                     Id = x.Item_id,
                     Item_id = x.Item.Item_id,
                     Item_name = x.Item.Short_description,


                     Qty_out = InvBus.GetSoldItemsExReturn(x.Item_id, x.Sales_id),

                     Cost_of_qty_out =
                     InvBus.GetSoldItemUnitPrice(x.Item_id, x.Sales_id).Sum(z => z.CostPrice)
                     * (decimal)InvBus.GetSoldItemsExReturn(x.Item_id, x.Sales_id)
                           ,

                     Total_Revenue_from_sales =
                       InvBus.GetSoldItemUnitPrice(x.Item_id, x.Sales_id).Sum(z => z.UnitPrice)
                        * (decimal)InvBus.GetSoldItemsExReturn(x.Item_id, x.Sales_id)
                      ,

                     Transaction_date = x.Sales.Receivable.Transaction_date,

                     Doc_num = Convert.ToString(x.Sales.Go_num),
                     Doc_type = x.Sales.Doc_type,
                     Customer_name = x.Sales.Receivable.Vendor.Vendor_name
                 }).ToList();

                    Res = GetGrandProfitReturn(Res);

                    Res.GroupBy(x => x.Item_id).ForEach(x =>
                    {
                        x.FirstOrDefault().
                        Qty_out = x.Sum(z => z.Qty_out);
                        x.FirstOrDefault().
                        Cost_of_qty_out = x.Sum(z => z.Cost_of_qty_out);
                        x.FirstOrDefault().
                        Total_Revenue_from_sales = x.Sum(z => z.Total_Revenue_from_sales);
                    });

                    Res = Res.GroupBy(x => x.Item_id).Select(x => x.FirstOrDefault()).ToList();
                }
            }
            else
            {
                Res = db.Inv_sales_receivs_pos.Where(x => x.Item_id == ItemId)
                      .Include(x => x.Sales).Include(x => x.Receive_po)
                      .Include(x => x.Receive_po.Payable)
                      .Include(x => x.Receive_po.Items)
                      .Include(x => x.Sales.Receivable)
                      .Include(x => x.Sales.Receivable.Vendor)
                  .Include(x => x.Item).Where(x => x.Sales.Receivable_id != null).ToList()
                  .Select(x => new GrandProfit
                  {
                      Id = x.Item_id,
                      Item_id = x.Item.Item_id,
                      Item_name = x.Item.Short_description,

                      Qty_out = InvBus.GetSoldItemsExReturn(x.Item_id, x.Sales_id),

                      Cost_of_qty_out =
                      InvBus.GetSoldItemUnitPrice(x.Item_id, x.Sales_id).Sum(z => z.CostPrice)
                      * (decimal)InvBus.GetSoldItemsExReturn(x.Item_id, x.Sales_id)
                            ,

                      Total_Revenue_from_sales =
                        InvBus.GetSoldItemUnitPrice(x.Item_id, x.Sales_id).Sum(z => z.UnitPrice)
                         * (decimal)InvBus.GetSoldItemsExReturn(x.Item_id, x.Sales_id)
                       ,

                      Transaction_date = x.Sales.Receivable.Transaction_date,

                      Doc_num = Convert.ToString(x.Sales.Go_num),
                      Doc_type = x.Sales.Doc_type,
                      Customer_name = x.Sales.Receivable.Vendor.Vendor_name
                  }).ToList();
                Res = GetGrandProfitReturn(Res);

            }
            if (DateFrom.HasValue && DateTo.HasValue)
            {
                Res = Res.Where(x => x.Transaction_date >= DateFrom
                && x.Transaction_date <= DateTo).ToList();
            }
            ViewBag.IsGrandProfit = IsGrandProfit;
            Res.RemoveAll(x => x.Qty_out == 0);
            return Res;
        }

        public List<GrandProfit> GetGrandProfitReturn(List<GrandProfit> Res)
        {
            List<GrandProfit> ThisRes = new List<GrandProfit>();
            foreach (GrandProfit item in Res.GroupBy(x=>x.Id).Select(x=>x.FirstOrDefault()).ToList())
            {
                
                foreach (Inv_receive_po_items i in InvBus.GetReturnItemsList(item.Id)
                    .SelectMany(x => x.Items)
                    .ToList())
                {
                    ThisRes.Add(new GrandProfit
                    {
                        Id = i.Item_id,
                        Item_id = i.Item.Item_id,
                        Item_name = i.Item.Short_description,
                        Qty_out = -InvBus.GetReturnItems(i.Item_id),

                        Cost_of_qty_out = -i.Unit_price,

                        Total_Revenue_from_sales = -i.Unit_price,

                        Transaction_date = i.Receive_po.Receivable.Transaction_date,

                        Doc_num = Convert.ToString(i.Receive_po.Gr_num),
                        Doc_type = Doc_type.Return,
                        Customer_name = i.Receive_po.Customer.Vendor_name
                    });
                }
            }
            Res.AddRange(ThisRes);
            return Res;
        }
        public ActionResult QtyAvaliable()
        {
            ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");
            return View();
        }
        public ActionResult QtyAvaliableRes()
        {
            List<QtyAvaliable> Res = CalcQtyAvaliable(false);
            return View(Res);
        }

        public List<QtyAvaliable> CalcQtyAvaliable(bool RemoveZero)
        {
            List<int> StoreItems = new List<int>();
            
            List<QtyAvaliable> Res = db.Inv_item
                //.Where(x => StoreItems.Any(z => z == x.Item_id))
                //.Include(x => x.Sales).Include(x => x.Receive_po).Include(x => x.Item)
                .Include(x => x.Unit_of_measure)
                .ToList()
                .Select(x => new FabulousModels.Inventory.QtyAvaliable
                {
                    Id = x.Id,
                    Item_id = x.Item_id,
                    Item_name = x.Short_description,
                    Total_qty_in = InvBus.GetItemEnterdStore(x.Id) /* x.Receive_po.Items.ToList().Where(y => StoreItems.Any(z => z == y.Item_id)).Sum(z => z.Quantity)*/,
                    Total_qty_out = InvBus.GetItemSoldInStore(x.Id)/*x.Quantity*/,
                    Qty_avaliable = 0,
                    UOM = x.Unit_of_measure.Unit_id
                }).ToList();
            if (RemoveZero)
            {
                Res.RemoveAll(x => x.Total_qty_in == 0);
            }
            Res.ForEach(x => x.Qty_avaliable = x.Total_qty_in - x.Total_qty_out);
            return Res;
        }

        public ActionResult DailySalesRpt()
        {
            ViewBag.YearId = new SelectList(db.NewFiscalYear_Table, "YearID", "Year");
            ViewBag.User = new SelectList(db.CreateAccount_Tables, "UserID", "UserName");
            return View();
        }
        public ActionResult DailySalesRes(bool IsPay, bool IsReturn,
            int YearId, string Ftype = "Yearlly", DateTime? Start = null,
            DateTime? End = null, string User = null)
        {
            ViewBag.IsPay = IsPay;
            ViewBag.IsReturn = IsReturn;
            IEnumerable<Receivable_transaction> Recs = Enumerable.Empty<Receivable_transaction>();
            IEnumerable<Payable_transaction> Pays = Enumerable.Empty<Payable_transaction>();
            if (!IsPay)
            {
                if (IsReturn)
                {
                    Recs = db.Receivable_transactions
                         .Join(db.Inv_receive_po, R => R.Id, S => S.Receivable_id, (R, S) => new { R, S })
                         .Select(x => x.R).Where(x=>x.Doc_type==Doc_type.Return)
                         .Include(x => x.Vendor).Include(x => x.Sales_invoice)
                         .Include(x => x.Sales_invoice.Select(z => z.Profit_user));
                }
                else
                {
                    Recs = db.Receivable_transactions
                         .Join(db.Inv_sales_invoice, R => R.Id, S => S.Receivable_id, (R, S) => new { R, S })
                         .Select(x => x.R)
                         .Include(x => x.Vendor).Include(x => x.Sales_invoice)
                         .Include(x => x.Sales_invoice.Select(z => z.Profit_user));
                }
             
            }
            else
            {
                if (IsReturn)
                {
                    Pays = db.Payable_transactions.Join(db.Inv_sales_invoice, Pay => Pay.Id, Po => Po.Payable_id, (Pay, Po) => new { Pay, Po })
                       .Select(x => x.Pay).Where(x=>x.Doc_type==Doc_type.Return).Include(x => x.Vendor).Include(x => x.Purchase_invoice)
                            .Include(x => x.Purchase_invoice.Select(z => z.Profit_user))
                       ;
                }
                else
                {
                    Pays = db.Payable_transactions.Join(db.Inv_receive_po, Pay => Pay.Id, Po => Po.Payable_id, (Pay, Po) => new { Pay, Po })
                    .Select(x => x.Pay).Include(x => x.Vendor).Include(x => x.Purchase_invoice)
                         .Include(x => x.Purchase_invoice.Select(z => z.Profit_user))
                    ;
                }

            }
            List<DailySales> Res = new List<DailySales>();
            if (Ftype == "Periodic")
            {
                List<FiscalYear_Table> Periods = db.FiscalYear_Tables.Where(x => x.YearID == YearId).ToList();
                foreach (FiscalYear_Table i in Periods)
                {
                    DateTime PStartDate = Convert.ToDateTime(i.Period_Start_Date);
                    DateTime PEndDate = Convert.ToDateTime(i.Period_End_Date);
                    if (!IsPay)
                    {
                        foreach (Receivable_transaction Rec in Recs
                            .Where(x => x.Transaction_date >= PStartDate && x.Transaction_date <= PEndDate).ToList())
                        {
                            Res.Add(new DailySales
                            {
                                Customer_name = Rec.Vendor.Vendor_name,
                                Date = Rec.Transaction_date,
                                Discount = Rec.Taken_discount,
                                Doc_no = Rec.VDocument_number,
                                Doc_type = (Doc_type)Rec.Doc_type,
                                Net_amount = Rec.Purchase - Rec.Taken_discount,
                                Sales = Rec.Purchase,
                                Period_no = i.Period_No.ToString(),
                                UserId = Rec.Sales_invoice.FirstOrDefault().Profit_user_id,
                                UserName = Rec.Sales_invoice.FirstOrDefault().Profit_user.UserName
                            });
                        }
                    }
                    else
                    {
                        foreach (Payable_transaction Rec in
                           Pays.Where(x => x.Transaction_date >= PStartDate && x.Transaction_date <= PEndDate).ToList())
                        {
                            Res.Add(new DailySales
                            {
                                Customer_name = Rec.Vendor.Vendor_name,
                                Date = Rec.Transaction_date,
                                Discount = Rec.Taken_discount,
                                Doc_no = Rec.VDocument_number,
                                Doc_type = (Doc_type)Rec.Doc_type,
                                Net_amount = Rec.Purchase - Rec.Taken_discount,
                                Sales = Rec.Purchase,
                                Period_no = i.Period_No.ToString(),
                                UserId = Rec.Purchase_invoice.FirstOrDefault().Profit_user_id,
                                UserName = Rec.Purchase_invoice.FirstOrDefault().Profit_user.UserName
                            });
                        }
                    }

                }
            }
            else if (Ftype == "Month")
            {
                int Year = Convert.ToInt32(db.NewFiscalYear_Table.Find(YearId).Year);
                for (int i = 1; i <= 12; i++)
                {
                    DateTime SPeriod = new DateTime(Year, i, 1);
                    DateTime EPeriod = SPeriod.AddMonths(1).AddDays(-1);
                    if (!IsPay)
                    {
                        foreach (Receivable_transaction Rec in Recs
                                 .Where(x => x.Transaction_date >= SPeriod && x.Transaction_date <= EPeriod)
                            )
                        {
                            Res.Add(new DailySales
                            {
                                Customer_name = Rec.Vendor.Vendor_name,
                                Date = Rec.Transaction_date,
                                Discount = Rec.Taken_discount,
                                Doc_no = Rec.VDocument_number,
                                Doc_type = (Doc_type)Rec.Doc_type,
                                Net_amount = Rec.Purchase - Rec.Taken_discount,
                                Sales = Rec.Purchase,
                                Period_no = i.ToString(),
                                UserId = Rec.Sales_invoice.ToList().FirstOrDefault().Profit_user_id,
                                //UserName = Rec.Sales_invoice.FirstOrDefault().Profit_user.UserName

                            });
                        }
                    }
                    else
                    {
                        foreach (Payable_transaction Rec in Pays
                                             .Where(x => x.Transaction_date >= SPeriod && x.Transaction_date <= EPeriod).ToList())
                        {
                            Res.Add(new DailySales
                            {
                                Customer_name = Rec.Vendor.Vendor_name,
                                Date = Rec.Transaction_date,
                                Discount = Rec.Taken_discount,
                                Doc_no = Rec.VDocument_number,
                                Doc_type = (Doc_type)Rec.Doc_type,
                                Net_amount = Rec.Purchase - Rec.Taken_discount,
                                Sales = Rec.Purchase,
                                Period_no = i.ToString(),
                                UserId = Rec.Purchase_invoice.FirstOrDefault().Profit_user_id,
                                // UserName = Rec.Purchase_invoice.FirstOrDefault().Profit_user.UserName
                            });
                        }
                    }

                }
            }
            else if (Ftype == "Quarter")
            {
                int Year = Convert.ToInt32(db.NewFiscalYear_Table.Find(YearId).Year);
                for (int i = 1; i <= 12; i += 3)
                {
                    DateTime SPeriod = new DateTime(Year, i, 1);
                    DateTime EPeriod = new DateTime(Year, i, 1).AddMonths(3).AddDays(-1);
                    if (!IsPay)
                    {
                        foreach (Receivable_transaction Rec in Recs
                                               .Where(x => x.Transaction_date >= SPeriod && x.Transaction_date <= EPeriod).ToList())
                        {
                            Res.Add(new DailySales
                            {
                                Customer_name = Rec.Vendor.Vendor_name,
                                Date = Rec.Transaction_date,
                                Discount = Rec.Taken_discount,
                                Doc_no = Rec.VDocument_number,
                                Doc_type = (Doc_type)Rec.Doc_type,
                                Net_amount = Rec.Purchase - Rec.Taken_discount,
                                Sales = Rec.Purchase,
                                Period_no = SPeriod.Month.ToString() + "-" + EPeriod.Month.ToString(),
                                UserId = Rec.Sales_invoice.FirstOrDefault()?.Profit_user_id,
                                //  UserName = Rec.Sales_invoice.FirstOrDefault()?.Profit_user.UserName

                            });
                        }
                    }
                    else
                    {
                        foreach (Payable_transaction Rec in Pays
                                              .Where(x => x.Transaction_date >= SPeriod && x.Transaction_date <= EPeriod).ToList())
                        {
                            Res.Add(new DailySales
                            {
                                Customer_name = Rec.Vendor.Vendor_name,
                                Date = Rec.Transaction_date,
                                Discount = Rec.Taken_discount,
                                Doc_no = Rec.VDocument_number,
                                Doc_type = (Doc_type)Rec.Doc_type,
                                Net_amount = Rec.Purchase - Rec.Taken_discount,
                                Sales = Rec.Purchase,
                                Period_no = SPeriod.Month.ToString() + "-" + EPeriod.Month.ToString(),
                                UserId = Rec.Purchase_invoice.FirstOrDefault()?.Profit_user_id,
                                //   UserName = Rec.Purchase_invoice.FirstOrDefault()?.Profit_user.UserName
                            });
                        }
                    }

                }
            }
            else if (Ftype == "Yearlly")
            {
                int Year = Convert.ToInt32(db.NewFiscalYear_Table.Find(YearId).Year);

                DateTime PStart = new DateTime(Year, 1, 1);
                DateTime PEnd = new DateTime(Year, 12, 31);
                if (!IsPay)
                {
                    foreach (Receivable_transaction Rec in Recs
                                       .Where(x => x.Transaction_date >= PStart && x.Transaction_date <= PEnd))
                    {
                        Res.Add(new DailySales
                        {
                            Customer_name = Rec.Vendor.Vendor_name,
                            Date = Rec.Transaction_date,
                            Discount = Rec.Taken_discount,
                            Doc_no = Rec.VDocument_number,
                            Doc_type = (Doc_type)Rec.Doc_type,
                            Net_amount = Rec.Purchase - Rec.Taken_discount,
                            Sales = Rec.Purchase,
                            Period_no = ""//Year.ToString(),
                            ,
                            UserId = Rec.Sales_invoice.FirstOrDefault()?.Profit_user_id,
                            //UserName = Rec.Sales_invoice.FirstOrDefault()?.Profit_user.UserName

                        });
                    }
                }
                else
                {
                    foreach (Payable_transaction Rec in Pays
                                    .Where(x => x.Transaction_date >= PStart && x.Transaction_date <= PEnd).ToList())
                    {
                        Res.Add(new DailySales
                        {
                            Customer_name = Rec.Vendor.Vendor_name,
                            Date = Rec.Transaction_date,
                            Discount = Rec.Taken_discount,
                            Doc_no = Rec.VDocument_number,
                            Doc_type = (Doc_type)Rec.Doc_type,
                            Net_amount = Rec.Purchase - Rec.Taken_discount,
                            Sales = Rec.Purchase,
                            Period_no = ""//Year.ToString(),
                            ,
                            UserId = Rec.Purchase_invoice.FirstOrDefault()?.Profit_user_id,
                            // UserName = Rec.Purchase_invoice.FirstOrDefault()?.Profit_user.UserName
                        });
                    }
                }

            }
            else if (Ftype == "StartEnd")
            {
                if (!IsPay)
                {
                    foreach (Receivable_transaction Rec in Recs
                                      .Where(x => x.Transaction_date >= Start.Value && x.Transaction_date <= End.Value).ToList())
                    {
                        Res.Add(new DailySales
                        {
                            Customer_name = Rec.Vendor.Vendor_name,
                            Date = Rec.Transaction_date,
                            Discount = Rec.Taken_discount,
                            Doc_no = Rec.VDocument_number,
                            Doc_type = (Doc_type)Rec.Doc_type,
                            Net_amount = Rec.Purchase - Rec.Taken_discount,
                            Sales = Rec.Purchase,
                            Period_no = ""//$"{Start.Value.ToShortDateString()} - {End.Value.ToShortDateString()}"
                            ,
                            UserId = Rec.Sales_invoice.FirstOrDefault()?.Profit_user_id,
                            // UserName = Rec.Sales_invoice.FirstOrDefault()?.Profit_user.UserName
                        });
                    }
                }
                else
                {
                    foreach (Payable_transaction Rec in Pays
                                    .Where(x => x.Transaction_date >= Start.Value && x.Transaction_date <= End.Value).ToList())
                    {
                        Res.Add(new DailySales
                        {
                            Customer_name = Rec.Vendor.Vendor_name,
                            Date = Rec.Transaction_date,
                            Discount = Rec.Taken_discount,
                            Doc_no = Rec.VDocument_number,
                            Doc_type = (Doc_type)Rec.Doc_type,
                            Net_amount = Rec.Purchase - Rec.Taken_discount,
                            Sales = Rec.Purchase,
                            Period_no = ""//$"{Start.Value.ToShortDateString()} - {End.Value.ToShortDateString()}"
                        ,
                            UserId = Rec.Purchase_invoice.FirstOrDefault()?.Profit_user_id,
                            //UserName = Rec.Purchase_invoice.FirstOrDefault()?.Profit_user.UserName
                        });
                    }
                }

            }
            if (!string.IsNullOrEmpty(User))
            {
                Res = Res.Where(x => x.UserId == User).ToList();
            }
            return View(Res.OrderByDescending(x => x.Date));
        }
        public ActionResult MonthlySalesRpt()
        {
            ViewBag.YearId = new SelectList(db.NewFiscalYear_Table, "YearID", "Year");
            return View();
        }
        public ActionResult MonthlySalesRes(bool IsPay, bool IsReturn, int YearId, string Ftype, DateTime? Start = null, DateTime? End = null)
        {
            ViewBag.IsPay = IsPay;
            ViewBag.IsReturn = IsReturn;
            List<string> Columns;
            List<MonthlySalesRpt> Res=CalcMonthlySalesRes(IsPay, IsReturn, YearId, Ftype, Start, End, out Columns);
            ViewBag.Columns = Columns;
            ViewBag.Res = Res;
            ViewBag.Ftype = Ftype;
            return View();
        }

        public List<MonthlySalesRpt> CalcMonthlySalesRes(bool IsPay, bool IsReturn, int YearId, string Ftype, DateTime? Start, DateTime? End, out List<string> Columns)
        {
            Columns = new List<string>();
            List<MonthlySalesRpt> Res = new List<MonthlySalesRpt>();
            if (Ftype == "Periodic")
            {
                List<FiscalYear_Table> Periods = db.FiscalYear_Tables.Where(x => x.YearID == YearId).ToList();
                foreach (FiscalYear_Table i in Periods)
                {
                    Columns.Add(i.Period_No.ToString());
                    DateTime PStartDate = Convert.ToDateTime(i.Period_Start_Date);
                    DateTime PEndDate = Convert.ToDateTime(i.Period_End_Date);
                    if (!IsPay)
                    {
                        decimal ThisSales = 0;
                        decimal ThisDiscount = 0;
                        if (IsReturn == true)
                        {
                            foreach (Receivable_transaction Rec in db.Receivable_transactions.Include(x => x.Vendor)
                                                           .Where(x => x.Transaction_date >= PStartDate && x.Transaction_date <= PEndDate
                                                                       && x.Doc_type == Doc_type.Return).ToList())
                            {
                                ThisSales += Rec.Purchase;
                                ThisDiscount += Rec.Taken_discount;
                            }
                            Res.Add(new MonthlySalesRpt
                            {
                                Col = i.Period_No.ToString(),
                                Discount = ThisDiscount,
                                Sales = ThisSales,
                                Doc_type = Doc_type.Return
                            });
                        }
                        else
                        {
                            foreach (Receivable_transaction Rec in db.Receivable_transactions.Include(x => x.Vendor)
                               .Where(x => x.Transaction_date >= PStartDate && x.Transaction_date <= PEndDate
                                           && x.Transaction_date.Month == i.Period_No
                                           && x.Doc_type == Doc_type.Invoice).ToList())
                            {
                                ThisSales += Rec.Purchase;
                                ThisDiscount += Rec.Taken_discount;
                            }
                            Res.Add(new MonthlySalesRpt
                            {
                                Col = i.Period_No.ToString(),
                                Discount = ThisDiscount,
                                Sales = ThisSales,
                                Doc_type = Doc_type.Invoice
                            });
                        }
                    }
                    else
                    {
                        decimal ThisSales = 0;
                        decimal ThisDiscount = 0;
                        if (IsReturn == true)
                        {
                            foreach (Payable_transaction Rec in db.Payable_transactions.Include(x => x.Vendor)
                                             .Where(x => x.Transaction_date >= PStartDate
                                             && x.Transaction_date <= PEndDate
                                                 && x.Doc_type == Doc_type.Return).ToList())
                            {
                                ThisSales += Rec.Purchase;
                                ThisDiscount += Rec.Taken_discount;

                            }
                            Res.Add(new MonthlySalesRpt
                            {
                                Col = i.Period_No.ToString(),
                                Discount = ThisDiscount,
                                Sales = ThisSales,
                                Doc_type = Doc_type.Return
                            });
                        }
                        else
                        {
                            foreach (Payable_transaction Rec in db.Payable_transactions.Include(x => x.Vendor)
                                            .Where(x => x.Transaction_date >= PStartDate
                                            && x.Transaction_date <= PEndDate
                                                && x.Doc_type == Doc_type.Invoice).ToList())
                            {
                                ThisSales += Rec.Purchase;
                                ThisDiscount += Rec.Taken_discount;

                            }
                            Res.Add(new MonthlySalesRpt
                            {
                                Col = i.Period_No.ToString(),
                                Discount = ThisDiscount,
                                Sales = ThisSales,
                                Doc_type = Doc_type.Invoice
                            });
                        }
                    }
                }
            }
            else if (Ftype == "Month")
            {
                int Year = Convert.ToInt32(db.NewFiscalYear_Table.Find(YearId).Year);
                for (int i = 1; i <= 12; i++)
                {
                    DateTime SPeriod = new DateTime(Year, i, 1);
                    DateTime EPeriod = SPeriod.AddMonths(1).AddDays(-1);
                    string MonthName = new DateTime(Year, i, 1).ToString("MMMM", CultureInfo.InvariantCulture);
                    Columns.Add(MonthName);
                    if (!IsPay)
                    {
                        if (IsReturn)
                        {
                            decimal ThisSales = 0;
                            decimal ThisDiscount = 0;
                            foreach (Receivable_transaction Rec in db.Receivable_transactions.Include(x => x.Vendor)
                                                  .Where(x => x.Transaction_date >= SPeriod && x.Transaction_date <= EPeriod
                                                  && x.Doc_type == Doc_type.Return).ToList())
                            {
                                ThisSales += Rec.Purchase;
                                ThisDiscount += Rec.Taken_discount;

                            }
                            Res.Add(new MonthlySalesRpt
                            {
                                Col = MonthName,
                                Discount = ThisDiscount,
                                Sales = ThisSales,
                                Doc_type = Doc_type.Return
                            });
                        }
                        else
                        {
                            decimal ThisSales = 0;
                            decimal ThisDiscount = 0;
                            foreach (Receivable_transaction Rec in db.Receivable_transactions.Include(x => x.Vendor)
                                                  .Where(x => x.Transaction_date >= SPeriod && x.Transaction_date <= EPeriod
                                                  && x.Doc_type == Doc_type.Invoice).ToList())
                            {
                                ThisSales += Rec.Purchase;
                                ThisDiscount += Rec.Taken_discount;

                            }
                            Res.Add(new MonthlySalesRpt
                            {
                                Col = MonthName,
                                Discount = ThisDiscount,
                                Sales = ThisSales,
                                Doc_type = Doc_type.Invoice
                            });
                        }
                    }
                    else
                    {
                        decimal ThisSales = 0;
                        decimal ThisDiscount = 0;
                        if (IsReturn)
                        {
                            foreach (Payable_transaction Rec in db.Payable_transactions.Include(x => x.Vendor)
                                            .Where(x => x.Transaction_date >= SPeriod && x.Transaction_date <= EPeriod
                                            && x.Doc_type == Doc_type.Return).ToList())
                            {
                                ThisSales += Rec.Purchase;
                                ThisDiscount += Rec.Taken_discount;

                            }
                            Res.Add(new MonthlySalesRpt
                            {
                                Col = MonthName,
                                Discount = ThisDiscount,
                                Sales = ThisSales,
                                Doc_type = Doc_type.Return
                            });
                        }
                        else
                        {
                            foreach (Payable_transaction Rec in db.Payable_transactions.Include(x => x.Vendor)
                                            .Where(x => x.Transaction_date >= SPeriod && x.Transaction_date <= EPeriod
                                            && x.Doc_type == Doc_type.Invoice).ToList())
                            {
                                ThisSales += Rec.Purchase;
                                ThisDiscount += Rec.Taken_discount;

                            }
                            Res.Add(new MonthlySalesRpt
                            {
                                Col = MonthName,
                                Discount = ThisDiscount,
                                Sales = ThisSales,
                                Doc_type = Doc_type.Invoice
                            });
                        }

                    }


                }
            }
            else if (Ftype == "Quarter")
            {
                int Year = Convert.ToInt32(db.NewFiscalYear_Table.Find(YearId).Year);
                for (int i = 1; i <= 12; i += 3)
                {
                    DateTime SPeriod = new DateTime(Year, i, 1);
                    DateTime EPeriod = new DateTime(Year, i, 1).AddMonths(3).AddDays(-1);

                    string StartMonthName = SPeriod.ToString("MMMM", CultureInfo.InvariantCulture);
                    string EndMonthName = EPeriod.ToString("MMMM", CultureInfo.InvariantCulture);
                    string ThisQurater = StartMonthName + " - " + EndMonthName;
                    Columns.Add(ThisQurater);
                    if (IsReturn)
                    {
                        decimal ThisSales = 0;
                        decimal ThisDiscount = 0;
                        foreach (Receivable_transaction Rec in db.Receivable_transactions.Include(x => x.Vendor)
                                              .Where(x => x.Transaction_date >= SPeriod && x.Transaction_date <= EPeriod
                                              && x.Doc_type == Doc_type.Return).ToList())
                        {
                            ThisSales += Rec.Purchase;
                            ThisDiscount += Rec.Taken_discount;

                        }
                        Res.Add(new MonthlySalesRpt
                        {
                            Col = ThisQurater,
                            Discount = ThisDiscount,
                            Sales = ThisSales,
                            Doc_type = Doc_type.Return,
                            Similer = i
                        });
                    }
                    else
                    {
                        decimal ThisSales = 0;
                        decimal ThisDiscount = 0;
                        foreach (Receivable_transaction Rec in db.Receivable_transactions.Include(x => x.Vendor)
                                              .Where(x => x.Transaction_date >= SPeriod && x.Transaction_date <= EPeriod
                                              && x.Doc_type == Doc_type.Invoice).ToList())
                        {
                            ThisSales += Rec.Purchase;
                            ThisDiscount += Rec.Taken_discount;

                        }
                        Res.Add(new MonthlySalesRpt
                        {
                            Col = ThisQurater,
                            Discount = ThisDiscount,
                            Sales = ThisSales,
                            Doc_type = Doc_type.Invoice,
                            Similer = i
                        });
                    }

                }
            }
            else if (Ftype == "Yearlly")
            {
                int Year = Convert.ToInt32(db.NewFiscalYear_Table.Find(YearId).Year);

                DateTime PStart = new DateTime(Year, 1, 1);
                DateTime PEnd = new DateTime(Year, 12, 31);
                Columns.Add(Year.ToString());
                if (!IsPay)
                {
                    foreach (Receivable_transaction Rec in db.Receivable_transactions.Include(x => x.Vendor)
                  .Where(x => x.Transaction_date >= PStart && x.Transaction_date <= PEnd).ToList())
                    {
                        Res.Add(new MonthlySalesRpt
                        {
                            Col = Year.ToString(),
                            Discount = Rec.Taken_discount,
                            Sales = Rec.Purchase,
                            Doc_type = Rec.Doc_type,
                            Similer = Year
                        });
                    }
                }
                else
                {
                    foreach (Payable_transaction Rec in db.Payable_transactions.Include(x => x.Vendor)
                 .Where(x => x.Transaction_date >= PStart && x.Transaction_date <= PEnd).ToList())
                    {
                        Res.Add(new MonthlySalesRpt
                        {
                            Col = Year.ToString(),
                            Discount = Rec.Taken_discount,
                            Sales = Rec.Purchase,
                            Doc_type = Rec.Doc_type,
                            Similer = Year
                        });
                    }
                }

            }
            else if (Ftype == "StartEnd")
            {
                Columns.Add(Start.Value.ToShortDateString() + " " + End.Value.ToShortDateString());
                if (!IsPay)
                {
                    foreach (Receivable_transaction Rec in db.Receivable_transactions.Include(x => x.Vendor)
                                        .Where(x => x.Transaction_date >= Start.Value && x.Transaction_date <= End.Value).ToList())
                    {
                        Res.Add(new MonthlySalesRpt
                        {
                            Col = Start.Value.ToShortDateString() + " " + End.Value.ToShortDateString(),
                            Discount = Rec.Taken_discount,
                            Sales = Rec.Purchase,
                            Doc_type = Rec.Doc_type,
                            Similer = 1
                        });
                    }
                }
                else
                {
                    foreach (Payable_transaction Rec in db.Payable_transactions.Include(x => x.Vendor)
                                       .Where(x => x.Transaction_date >= Start.Value && x.Transaction_date <= End.Value).ToList())
                    {
                        Res.Add(new MonthlySalesRpt
                        {
                            Col = Start.Value.ToShortDateString() + " " + End.Value.ToShortDateString(),
                            Discount = Rec.Taken_discount,
                            Sales = Rec.Purchase,
                            Doc_type = Rec.Doc_type,
                            Similer = 1
                        });
                    }
                }

            }
            return Res;
        }

        public ActionResult CustomerSalesRpt()
        {
            ViewBag.IsPay = Convert.ToBoolean(Request["IsPay"]);
            ViewBag.IsB = Convert.ToBoolean(Request["IsB"]);
            if (ViewBag.IsPay)
            {
                ViewBag.CusVendTo = ViewBag.CusVendFrom = new SelectList(db.Payable_creditor_setting.Select(x => new { x.Id, Vendor_id = x.Vendor_id + " - " + x.Vendor_name }), "Id", "Vendor_id");

            }
            else
            {
                ViewBag.CusVendTo = ViewBag.CusVendFrom = new SelectList(db.Receivable_vendore_settings.Select(x => new { x.Id, Vendor_id = x.Vendor_id + " - " + x.Vendor_name }), "Id", "Vendor_id");
            }
            return View();
        }
        public ActionResult CustomerSalesRes(bool IsPay, bool IsB,
            DateTime? Start, DateTime? End, int CusVendFrom, int CusVendTo)
        {

            ViewBag.IsB = IsB;
            ViewBag.IsPay = IsPay;
            List<SalesAndDebit> Res = new List<SalesAndDebit>();
            Res = CalcCustomerSales(IsPay, Start, End, CusVendFrom, CusVendTo);
            return View(Res);
        }

        public List<SalesAndDebit> CalcCustomerSales(bool IsPay, DateTime? Start, DateTime? End, int CusVendFrom, int CusVendTo,bool GetTopThirty=false)
        {
            List<SalesAndDebit> Res = new List<SalesAndDebit>();
            List<InvCSRpt> Debit = new List<InvCSRpt>();
            List<InvCustSalesRpt> Sales = new List<InvCustSalesRpt>();

            if (!IsPay)
            {
                List<Receivable_transaction> RecTrans = db.Receivable_transactions.Include(x => x.Vendor)
                                   .Where(x => x.Vendor_id >= CusVendFrom && x.Vendor_id <= CusVendTo)
                                   .ToList().GroupJoin(db.Assign_Receivable_docs, T => T.Trans_doc_type_id, A => A.Trans_doc_type_id, (T, A) => new { T, A = A.DefaultIfEmpty() }).Select(x => x.T).ToList();
                if (GetTopThirty)
                {
                    RecTrans = RecTrans.OrderByDescending(x => x.Purchase).Take(30).ToList();
                }
                foreach (Receivable_transaction Rec in RecTrans)
                {
                    Debit.Add(new InvCSRpt
                    {
                        Customer_name = Rec.Vendor.Vendor_name,
                        Date = Rec.Transaction_date,
                        Amount = Rec.Purchase - Rec.Taken_discount + Rec.Tax,
                    });
                }
                List<Receivable_transaction> RecTrans2 = db.Receivable_transactions.Include(x => x.Vendor)
                       .Where(x => x.Vendor_id >= CusVendFrom && x.Vendor_id <= CusVendTo).ToList();
                if (GetTopThirty)
                {
                    RecTrans2 = RecTrans2.OrderByDescending(x => x.Purchase).Take(30).ToList();
                }
                foreach (Receivable_transaction Rec in RecTrans2)
                {
                    Sales.Add(new InvCustSalesRpt
                    {
                        Customer_name = Rec.Vendor.Vendor_name,
                        Discount = Rec.Taken_discount,
                        Amount = Rec.Purchase,
                        Date = Rec.Transaction_date,
                        Doc_type = Rec.Doc_type,
                        Doc_no = Rec.VDocument_number
                    });
                }
            }
            else
            {
                List<Payable_transaction> PayTrans = db.Payable_transactions.Include(x => x.Vendor)
                                  .Where(x => x.Vendor_id >= CusVendFrom && x.Vendor_id <= CusVendTo)
                                  .ToList().GroupJoin(db.Assign_Receivable_docs, T => T.Trans_doc_type_id, A => A.Trans_doc_type_id_to, (T, A) => new { T, A = A.DefaultIfEmpty() }).Select(x => x.T).ToList();
                if (GetTopThirty)
                {
                    PayTrans = PayTrans.OrderByDescending(x => x.Purchase).Take(30).ToList();
                }

                foreach (Payable_transaction Rec in PayTrans)
                {
                    Debit.Add(new InvCSRpt
                    {
                        Customer_name = Rec.Vendor.Vendor_name,
                        Date = Rec.Transaction_date,
                        Amount = Rec.Purchase - Rec.Taken_discount + Rec.Tax,

                    });
                }
                List<Payable_transaction> PayTrans2 = db.Payable_transactions.Include(x => x.Vendor)
                       .Where(x => x.Vendor_id >= CusVendFrom && x.Vendor_id <= CusVendTo).ToList();
                if (GetTopThirty)
                {
                    PayTrans2 = PayTrans2.OrderByDescending(x => x.Purchase).ToList();
                }
                foreach (Payable_transaction Rec in PayTrans2)
                {
                    Sales.Add(new InvCustSalesRpt
                    {
                        Customer_name = Rec.Vendor.Vendor_name,
                        Discount = Rec.Taken_discount,
                        Amount = Rec.Purchase,
                        Date = Rec.Transaction_date,
                        Doc_no = Rec.VDocument_number,
                        Doc_type = Rec.Doc_type
                    });
                }
            }
            if (Start.HasValue && End.HasValue)
            {
                Debit = Debit.Where(x => Start >= x.Date && End <= x.Date).ToList();
                Debit = Debit.Where(x => Start >= x.Date && End <= x.Date).ToList();
            }
            Res.Add(new SalesAndDebit
            {
                Debit = Debit,
                Sales = Sales
            });
            return Res;
        }

        public ActionResult Stoking()
        {
            ViewBag.Site_id = new SelectList(new List<string> { });
            ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");

            ViewBag.Item_id = new SelectList(new List<string>());
            ViewBag.UOM_id = new SelectList(new List<string>());
            return View();
        }
        [HttpPost]
        public ActionResult Stoking(List<Inv_stocking> Stocking)
        {
            db.Inv_stocking.AddRange(Stocking);
            db.SaveChanges();
            return Json(1);
        }
        public ActionResult PurchaseInvoice(bool Sales = false, bool GoodsRecipts = false)
        {
            if (Sales == false)
            {
                ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting.Select(x => new { x.Id, Vendor_id = x.Vendor_id.ToString() + " - " + x.Vendor_name }), "Id", "Vendor_id");
                ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");
                ViewBag.Po = new SelectList(Inv_receive_poController.GetVendoreSelect(),
                        "Id", "Trx");
            }
            else
            {
                ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");
                ViewBag.Customer_id = new SelectList(db.Receivable_vendore_settings.Select(x => new { x.Id, Vendor_id = x.Vendor_id.ToString() + " - " + x.Vendor_name }), "Id", "Vendor_id");
                ViewBag.InvNum = new SelectList(Inv_sales_invoiceController.CustomerInvoiceTrx(),
                        "Id", "Trx");
            }
            ViewBag.Gr=ViewBag.Go =  new SelectList(new List<string>());
            ViewBag.Sales = Sales;
            ViewBag.GoodsRecipts = GoodsRecipts;
            return View();
        }
        public JsonResult GetStoreGr(int StoreId)
        {
            return Json(db.Inv_receive_po.Where(x => x.Store_id == StoreId && x.Doc_type == Doc_type.Invoice).Select(x => new { Id = x.Id, Gr = x.Gr_num }));
        }
        public JsonResult GetStoreGo(int StoreId)
        {
            return Json(db.Inv_sales_invoice.Where(x => x.Store_id == StoreId && x.Doc_type == Doc_type.Invoice).Select(x => new { Id = x.Id, Go = x.Go_num }));
        }
        public JsonResult ReOrganizeJv()
        {
            List<int> Cost = new List<int>()
            {
                46,
                47,
                48,
                54
            };
            List<int> UpdateGl = db.C_GeneralLedger_Tables.Where(x => x.C_GeneralJournalEntry_Table.C_PostingKey == "InvSale"
            && x.C_AID == null).Select(x => x.C_GL).ToList();

            db.C_GeneralLedger_Tables.Where(x => UpdateGl.Contains(x.C_GL))
                .ToList().ForEach(x => x.C_AID = 2);
            db.SaveChanges();

            var asd = db.C_GeneralLedger_Tables.Where(x => x.C_GeneralJournalEntry_Table.C_PostingKey == "InvSale"
            && Cost.Contains(x.C_AID.Value));

            var Id = db.Inv_item_gl_accounts
                .Select(x => x.Id);


            return Json("true", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Purchase()
        {
            return View(db.Inv_receive_po
                .Where(x=>x.Doc_type==Doc_type.Invoice)
                .Include(x=>x.Payable)
                .Include(x=>x.Store)
                .Include(x=>x.Site)
                .Include(x=>x.Payable.Vendor)
                .Include(x=>x.Payable.Trans_doc_type).ToList());
        } 
        public ActionResult Sales()
        {
            return View(db.Inv_sales_invoice
                .Where(x=>x.Doc_type==Doc_type.Invoice)
                .Include(x=>x.Receivable)
                 .Include(x => x.Store)
                .Include(x => x.Site)
                .Include(x=>x.Receivable.Vendor)
                .Include(x=>x.Receivable.Trans_doc_type).ToList());
        }
    }
}