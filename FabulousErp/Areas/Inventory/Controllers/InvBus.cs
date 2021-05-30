using FabulousDB.DB_Context;
using FabulousDB.Models;
using FabulousErp.Bussiness;
using FabulousModels.Inventory;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FabulousErp.Bussiness;
using FabulousErp;
using FabulousDB.Models;

namespace Inventory.Controllers
{
    public class InvBus:Business
    {
        public static float GetItemAvaliable(int ItemId)
        {
            using (DBContext db = new DBContext())
            {
                return
                   db.Inv_sales_receivs_pos.Where(x => ItemId == ItemId).Include(x => x.Receive_po).Include(x => x.Sales).Include(x => x.Item)
                    .ToList().Select(x => x.Sales.Inv_sales_item.ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity)).FirstOrDefault()
                    - db.Inv_transfer_relation.Include(x => x.Main_po).Where(x => x.Item_id == ItemId).Include(x => x.Item)
                   .ToList().
                   Sum(x => x.Quantity)
                    -
                    GetAdjustOutNumber(ItemId);
                   //db.Inv_adjustment_relation.Include(x => x.Main_po).Where(x => x.Item_id == ItemId).Include(x => x.Item)
                   //.ToList().
                   //Sum(x => x.Quantity);
            }
        } 
        public static float GetItemAvaliable(int ItemId, int PoID)
        {
            using (DBContext db = new DBContext())
            {
                return db.Inv_sales_receivs_pos.Where(x => ItemId == ItemId && x.Receive_po_id == PoID).Include(x => x.Receive_po).Include(x => x.Sales).Include(x => x.Item)
                     .Select(x => x.Receive_po.Items.Sum(z => z.Quantity)).FirstOrDefault()
                     -
                     db.Inv_sales_receivs_pos.Where(x => ItemId == ItemId && x.Receive_po_id == PoID).Include(x => x.Receive_po).Include(x => x.Sales).Include(x => x.Item)
                     .ToList().Select(x => x.Sales.Inv_sales_item.ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(z => z.Quantity)).FirstOrDefault()
                     - db.Inv_transfer_relation.Include(x => x.Main_po).Where(x => x.Main_po_id == PoID && x.Item_id == ItemId).Include(x => x.Item)
                    .ToList().
                    Sum(x => x.Quantity)
                     -
                     GetAdjustOutNumberByPo(ItemId, PoID);
                   //db.Inv_adjustment_relation.Include(x => x.Main_po).Where(x => x.Item_id == ItemId && x.Main_po_id == PoID).Include(x => x.Item)
                   //.ToList().
                   //Sum(x => x.Quantity);
            }
        }
 
        public static float GetItemAvaliableStore(int ItemId,int Store)
        {
            using (DBContext db = new DBContext())
            {
                return db.Inv_receive_po_items.Include(x => x.Receive_po).Where(x => x.Item_id == ItemId && x.Receive_po.Store_id == Store)
                     .Sum(x => x.Quantity) -
                   db.Inv_sales_receivs_pos.Include(x => x.Receive_po).Where(x => x.Item_id == ItemId && x.Receive_po.Store_id == Store).Include(x => x.Sales).Include(x => x.Item)
                   .ToList().
                   Sum(x => x.Quantity)
                   - db.Inv_transfer_relation.Include(x => x.Main_po).Where(x => x.Item_id == ItemId && x.Main_po.Store_id == Store).Include(x => x.Item)
                   .ToList().
                   Sum(x => x.Quantity)
                   - GetAdjustOutNumber(ItemId, Store);
                //db.Inv_adjustment_relation.Include(x => x.Main_po).Where(x => x.Item_id == ItemId && x.Main_po.Store_id == Store).Include(x => x.Item)
                //.ToList().
                //Sum(x => x.Quantity);
            }
        }  
        public static float GetItemAvaliableSite(int ItemId,int Site)
        {
            using (DBContext db = new DBContext())
            {
                return db.Inv_receive_po_items.Include(x => x.Receive_po).Where(x => x.Item_id == ItemId && x.Receive_po.Site_id == Site)
                     .Sum(x => x.Quantity) -
                   db.Inv_sales_receivs_pos.Include(x => x.Receive_po).Where(x => x.Item_id == ItemId && x.Receive_po.Site_id == Site).Include(x => x.Sales).Include(x => x.Item)
                   .ToList().
                   Sum(x => x.Quantity)
                   - db.Inv_transfer_relation.Include(x => x.Main_po).Where(x => x.Item_id == ItemId && x.Main_po.Site_id == Site).Include(x => x.Item)
                   .ToList().
                   Sum(x => x.Quantity)
                   - GetAdjustOutNumberBySite(ItemId, Site);
                   //db.Inv_adjustment_relation.Include(x => x.Main_po).Where(x => x.Item_id == ItemId && x.Main_po.Site_id == Site).Include(x => x.Item)
                   //.ToList().
                   //Sum(x => x.Quantity);
            }
        }
        ///<summary>
        ///Get Items That Not In The Store
        ///</summary>
        public static float GetItemSoldInStore(int ItemId,int ReceivePo,int Store)
        {
            using (DBContext db = new DBContext())
            {
               return
                    db.Inv_sales_receivs_pos.Include(x => x.Item).Where(x => x.Receive_po_id == ReceivePo && x.Item_id == ItemId)
                                    .ToList().DefaultIfEmpty(new Inv_sales_receivs_pos { Quantity = 0 }).Sum(x => x.Quantity)
                      +
                      db.Inv_transfer_relation.Include(x => x.Main_po).Where(x => x.Main_po_id == ReceivePo && x.Item_id == ItemId && x.Main_po.Store_id == Store).Include(x => x.Item)
                       .ToList().
                       DefaultIfEmpty(new FabulousDB.Models.Inventory.Inv_transfer_relation { Quantity = 0 })
                       .ToList().Sum(x => x.Quantity)
                       + GetAdjustOutNumber(ItemId,ReceivePo,Store);
            }
        }
        public static float GetAdjustOutNumber(int ItemId, int ReceivePo, int StoreId)
        {
            using (DBContext db = new DBContext())
            {
                return db.Inv_adjustment_relation.Include(x => x.Adjustment).Include(x => x.Main_po)
                 .Include(x => x.Item)
                      .Include(x => x.Receive_po)
                      .Include(x => x.Main_po)
                      .Include(x => x.Main_po.Items)
                .Where(x => x.Main_po_id == ReceivePo && x.Item_id == ItemId && x.Main_po.Store_id == StoreId)
                      .ToList().
                      DefaultIfEmpty(new Inv_adjustment_relation { Adjustment = new Inv_item_adjustment { Damage_qty = 0, Loss_qty = 0 }, Quantity = 0 })
                      .ToList().Sum(x => (x.Item_id == 0) ? 0 : InvBus.CalcItemEq(x.Item_id, x.Adjustment.UOM_id, x.Quantity));
            }
        }
        public static float GetAdjustOutNumber(int ItemId, int StoreId)
        {
            using (DBContext db = new DBContext())
            {
                return db.Inv_adjustment_relation.Include(x => x.Adjustment).Include(x => x.Main_po)
                .Where(x => x.Item_id == ItemId && x.Main_po.Store_id == StoreId).Include(x => x.Item)
                      .Include(x => x.Receive_po)
                      .Include(x => x.Main_po)
                      .Include(x => x.Main_po.Items)
                      .ToList().
                      DefaultIfEmpty(new Inv_adjustment_relation { Adjustment = new Inv_item_adjustment { Damage_qty = 0, Loss_qty = 0 }, Quantity = 0 })
                      .ToList().Sum(x => (x.Item_id == 0) ? 0 : InvBus.CalcItemEq(x.Item_id, x.Adjustment.UOM_id, x.Quantity));

            }
        }
        public static float GetAdjustOutNumberBySite(int ItemId,int SiteId)
        {
            using (DBContext db = new DBContext())
            {
                return db.Inv_adjustment_relation.Include(x => x.Adjustment).Include(x => x.Main_po)
                .Where(x => x.Item_id == ItemId && x.Main_po.Site_id == SiteId).Include(x => x.Item)
                      .Include(x => x.Receive_po)
                      .Include(x => x.Main_po)
                      .Include(x => x.Main_po.Items)
                      .ToList().
                      DefaultIfEmpty(new Inv_adjustment_relation { Adjustment = new Inv_item_adjustment { Damage_qty = 0, Loss_qty = 0 }, Quantity = 0 })
                      .ToList().Sum(x => (x.Item_id == 0) ? 0 : InvBus.CalcItemEq(x.Item_id, x.Adjustment.UOM_id, x.Quantity));
            }
        }
        public static float GetAdjustOutNumberByPo(int ItemId,int Po)
        {
            using (DBContext db = new DBContext())
            {
                return db.Inv_adjustment_relation.Include(x => x.Adjustment).Include(x => x.Main_po)
                .Where(x => x.Item_id == ItemId && x.Main_po_id == Po).Include(x => x.Item)
                      .Include(x => x.Receive_po)
                      .Include(x => x.Main_po)
                      .Include(x => x.Main_po.Items)
                      .ToList().
                      DefaultIfEmpty(new Inv_adjustment_relation { Adjustment = new Inv_item_adjustment { Damage_qty = 0, Loss_qty = 0 }, Quantity = 0 })
                      .ToList().Sum(x => (x.Item_id == 0) ? 0 : InvBus.CalcItemEq(x.Item_id, x.Adjustment.UOM_id, x.Quantity));
            }
        }
        public static float GetAdjustOutNumber(int ItemId)
        {
            using (DBContext db = new DBContext())
            {
                return db.Inv_adjustment_relation.Include(x => x.Adjustment).Include(x => x.Main_po)
               .Where(x => x.Item_id == ItemId).Include(x => x.Item)
                     .Include(x => x.Receive_po)
                     .Include(x => x.Main_po)
                     .Include(x => x.Main_po.Items)
                     .ToList().
                     DefaultIfEmpty(new Inv_adjustment_relation { Adjustment = new Inv_item_adjustment { Damage_qty = 0, Loss_qty = 0 }, Quantity = 0 })
                     .ToList().Sum(x => (x.Item_id == 0) ? 0 : InvBus.CalcItemEq(x.Item_id, x.Adjustment.UOM_id, x.Quantity));
            }
               
        }
        ///<summary>
        ///Get Items That Not In The Store
        ///</summary>
        public static float GetItemSoldInStore(int ItemId,int Store)
        {
            using (DBContext db = new DBContext())
            {
                float Sales = db.Inv_sales_receivs_pos.Include(x => x.Item).Where(x => x.Item_id == ItemId)
                                    .Include(x => x.Sales)
                                    .ToList().DefaultIfEmpty(new Inv_sales_receivs_pos { Quantity = 0 })
                                    .Sum(x => x.Quantity);
                
                float Transfer = db.Inv_transfer_relation.Include(x => x.Main_po)
                    .Where(x => x.Item_id == ItemId && x.Main_po.Store_id == Store)
                    .Include(x => x.Item)
                    .Include(x=>x.Receive_po)
                       .ToList().
                       Sum(x => x.Quantity) ;
               
                float Adjustment =
                    db.Inv_adjustment_relation.Include(x => x.Adjustment)
                     .Include(x => x.Main_po)
                     .Where(x => x.Item_id == ItemId && x.Main_po.Store_id == Store)
                     .Include(x => x.Item)
                     .Include(x=>x.Receive_po)
                     .Include(x=>x.Main_po)
                     .Include(x=>x.Main_po.Items)
                       .ToList().
                       DefaultIfEmpty(new Inv_adjustment_relation { Adjustment = new Inv_item_adjustment { Damage_qty = 0, Loss_qty = 0 },Quantity = 0 })
                       .Sum(x => (x.Item_id == 0) ? 0 : InvBus.CalcItemEq(x.Item_id, x.Adjustment.UOM_id, x.Quantity)
                       );

                return Sales + Transfer + Adjustment;


            }
        }  
        public static float GetItemEnterdStore(int ItemId,int Store)
        {
            using (DBContext db = new DBContext())
            {
                return
                    db.Inv_receive_po_items.Include(x => x.Item).Where(x => x.Item_id == ItemId)
                       // .Include(x => x.Receive_po)
                         .Where(x=> x.Receive_po.Store_id == Store)

                        .ToList().DefaultIfEmpty(new Inv_receive_po_items { Quantity = 0 })
                        .Sum(x => x.Quantity);

                //float Transfer = db.Inv_transfer_relation.Include(x => x.Main_po)
                //    .Where(x => x.Item_id == ItemId && x.Main_po.Store_id == Store)
                //    .Include(x => x.Item)
                //    .Include(x => x.Receive_po)
                //       .ToList().
                //       Sum(x => x.Quantity);

                //float Adjustment =
                //    db.Inv_adjustment_relation.Include(x => x.Adjustment)
                //     .Include(x => x.Main_po)
                //     .Where(x => x.Item_id == ItemId && x.Main_po.Store_id == Store)
                //     .Include(x => x.Item)
                //     .Include(x => x.Receive_po)
                //     .Include(x => x.Main_po)
                //       .ToList().
                //       Sum(x => InvBus.CalcItemEq(x.Item_id, x.Main_po.Items.FirstOrDefault(z => z.Item_id == ItemId
                //       ).UOM_id, x.Adjustment.Damage_qty + x.Adjustment.Loss_qty)
                //       );

                //return Purchase + Transfer + Adjustment;


            }
        }
       public static float GetItemEnterdStore(int ItemId)
        {
            using (DBContext db = new DBContext())
            {
            
                return
                    db.Inv_receive_po_items.Include(x => x.Item).Where(x => x.Item_id == ItemId)
                        .ToList().DefaultIfEmpty(new Inv_receive_po_items { Quantity = 0 })
                        .Sum(x => x.Quantity);
            }
        }
        public static float GetItemSoldInStore(int ItemId)
        {
            using (DBContext db = new DBContext())
            {
                float Sales = db.Inv_sales_receivs_pos.Include(x => x.Item).Where(x => x.Item_id == ItemId)
                                    .Include(x => x.Sales)
                                    .ToList().DefaultIfEmpty(new Inv_sales_receivs_pos { Quantity = 0 })
                                    .Sum(x => x.Quantity);

                float Transfer = db.Inv_transfer_relation.Include(x => x.Main_po)
                    .Where(x => x.Item_id == ItemId )
                    .Include(x => x.Item)
                    .Include(x => x.Receive_po)
                       .ToList().
                       Sum(x => x.Quantity);

                float Adjustment =
                    db.Inv_adjustment_relation.Include(x => x.Adjustment)
                     .Include(x => x.Main_po)
                     .Where(x => x.Item_id == ItemId )
                     .Include(x => x.Item)
                     .Include(x => x.Receive_po)
                     .Include(x => x.Main_po)
                     .Include(x => x.Main_po.Items)
                       .ToList().
                       DefaultIfEmpty(new Inv_adjustment_relation { Adjustment = new Inv_item_adjustment { Damage_qty = 0, Loss_qty = 0 }, Quantity = 0 })
                       .Sum(x => (x.Item_id == 0) ? 0 : InvBus.CalcItemEq(x.Item_id, x.Adjustment.UOM_id, x.Quantity)
                       );

                return Sales + Transfer + Adjustment;


            }
        }

        ///<summary>
        ///Get Items That Not In The Stores
        ///</summary>
        public static float GetItemSold(int ItemId)
        {
            using (DBContext db = new DBContext())
            {
                float Return = db.Inv_receive_po_items.Include(z => z.Receive_po).Where(z => z.Item_id == ItemId &&
                       z.Receive_po.Doc_type == Doc_type.Return).ToList().DefaultIfEmpty(new Inv_receive_po_items { Quantity = 0 }).Sum(z => z.Quantity);
                return
                    db.Inv_sales_receivs_pos.Include(x => x.Item).Where(x =>  x.Item_id == ItemId)
                                    .ToList().DefaultIfEmpty(new Inv_sales_receivs_pos { Quantity = 0 }).Sum(x => x.Quantity)
                      +db.Inv_transfer_relation.Include(x => x.Main_po).Where(x =>x.Item_id == ItemId).Include(x => x.Item)
                       .ToList().
                       Sum(x => x.Quantity)
                       + db.Inv_adjustment_relation.Include(x => x.Adjustment).Include(x => x.Main_po).Where(x => x.Item_id == ItemId).Include(x => x.Item)
                       .ToList().
                       Sum(x => x.Adjustment.Earn_qty)
                       - Return;
            }
        }  
        public static List<int> GetSoldInvoices(int ItemId)
        {
            
            using (DBContext db = new DBContext())
            {
                return db.Inv_sales_invoice_items
                     .Include(x => x.Sales_invoice)
                     .Where(x => x.Item_id == ItemId &&
                     x.Sales_invoice.Doc_type == Doc_type.Invoice).Select(x => x.Id).ToList();
            }

        }
        public static List<CostUnitPrice> GetSoldItemUnitPrice(int ItemId)
        {
            DBContext db = new DBContext();

                return db.Inv_sales_invoice_items
                     .Include(x => x.Sales_invoice)
                     .Where(x => x.Item_id == ItemId &&
                     x.Sales_invoice.Doc_type == Doc_type.Invoice)
                     .Select(x=>new CostUnitPrice {
                         CostPrice=x.Cost_price.Value,
                         UnitPrice=x.Unit_price 
                     }).ToList();
        } 
        public static List<CostUnitPrice> GetSoldItemUnitPrice(int ItemId,int SalesInvoice)
        {
            DBContext db = new DBContext();

            return db.Inv_sales_invoice_items
                     .Include(x => x.Sales_invoice)
                     .Where(x => x.Item_id == ItemId &&
                     x.Sales_invoice.Doc_type == Doc_type.Invoice
                     &&x.Sales_invoice_id==SalesInvoice)
                     .Select(x=>new CostUnitPrice {
                         CostPrice=x.Cost_price.Value,
                         UnitPrice=x.Unit_price 
                     }).ToList();
        }
        ///<summary>
        ///Get Items That Not Sold For Customers
        ///</summary>
        public static float GetSoldItems(int ItemId)
        {
            using (DBContext db = new DBContext())

            {
                float Return = db.Inv_receive_po_items.Include(z => z.Receive_po).Where(z => z.Item_id == ItemId &&
                       z.Receive_po.Doc_type == Doc_type.Return).ToList().DefaultIfEmpty(new Inv_receive_po_items { Quantity = 0 }).Sum(z => z.Quantity);
                return
                    db.Inv_sales_receivs_pos.Include(x => x.Item).Where(x => x.Item_id == ItemId)
                                    .ToList().DefaultIfEmpty(new Inv_sales_receivs_pos { Quantity = 0 })
                                    .Sum(x => x.Quantity)
                       - Return;
            }
        } 
        ///<summary>
        ///Get Items That Not Sold For Customers
        ///</summary>
        public static float GetSoldItems(int ItemId,int SalesInvoice)
        {
            using (DBContext db = new DBContext())

            {
                float Return = db.Inv_receive_po_items.Include(z => z.Receive_po).Where(z =>
                       z.Item_id == ItemId &&
                       z.Receive_po.Doc_type == Doc_type.Return).ToList().DefaultIfEmpty(new Inv_receive_po_items { Quantity = 0 }).Sum(z => z.Quantity);
                return
                    FabulousErp.Business.PostiveSubtract(db.Inv_sales_receivs_pos.Include(x => x.Item).Where(x => x.Item_id == ItemId && x.Sales_id == SalesInvoice)
                                    .ToList().DefaultIfEmpty(new Inv_sales_receivs_pos { Quantity = 0 })
                                    .Sum(x => x.Quantity)
                                    , Return);
            }
        }
        public static float GetSoldItemsExReturn(int ItemId,int SalesInvoice)
        {
            using (DBContext db = new DBContext())

            {
                return
                    db.Inv_sales_receivs_pos.Include(x => x.Item).Where(x => x.Item_id == ItemId && x.Sales_id == SalesInvoice)
                                    .ToList().DefaultIfEmpty(new Inv_sales_receivs_pos { Quantity = 0 })
                                    .Sum(x => x.Quantity);
            }
        } 
        public static float GetReturnItems(int ItemId)
        {
            using (DBContext db = new DBContext())

            {
                return
                    db.Inv_receive_po_items.Include(z => z.Receive_po).Where(z =>
                       z.Item_id == ItemId &&
                       z.Receive_po.Doc_type == Doc_type.Return)
                      .ToList().DefaultIfEmpty(new Inv_receive_po_items { Quantity = 0 })
                       .Sum(z => z.Quantity);
            }
        }
        public static List<Inv_receive_po> GetReturnItems()
        {
            using (DBContext db = new DBContext())

            {
                return
                    db.Inv_receive_po.Include(z => z.Items).Where(z =>
                       z.Items.Any(x=>
                       z.Doc_type == Doc_type.Return)
                       )
                      .ToList();
            }
        } 
        public static List<Inv_receive_po> GetReturnItemsList(int ItemId)
        {
            using (DBContext db = new DBContext())

            {
                List<Inv_receive_po> Res= 
                    db.Inv_receive_po.Include(z => z.Items)
                      .Where(z =>
                       z.Items.Any(x=>x.Item_id== ItemId &&
                       z.Doc_type == Doc_type.Return)
                       )
                      .Include(x=>x.Receivable)
                      .Include(x=>x.Customer)
                      .Include(x => x.Items.Select(z => z.Item))
                      .ToList();
                Res.ForEach(x => x.Items = x.Items.Where(z => z.Item_id == ItemId).ToList());
                    return Res;
            }
        }
        public static List<InvoiceItems> GetItemAvaliableByPoId(int PoId, bool WithItems = false)
        {
            using (DBContext db = new DBContext())

            {
                db.Configuration.ProxyCreationEnabled = false;
               
                List<InvoiceItems> AvItem = db.Inv_receive_po.Include(x => x.Payable).Include(x => x.Payable.Trans_doc_type)
                 .Include(x=>x.Items).Where(x => x.Id == PoId)
                 .Select(x => new InvoiceItems { Id = x.Id, Trx = x.Payable.Trans_doc_type.Trx_num.ToString(), PoId = x.Id, Purchase_items = x.Items.ToList() }).ToList();
                List<int> RemoveId = new List<int>();
                foreach (InvoiceItems I in AvItem)
                {
                    foreach (var Items in I.Purchase_items.GroupBy(x => x.Item_id).Select(x => x.ToList()))
                    {
                        if (InvBus.GetItemAvaliable(Items.FirstOrDefault().Item_id, I.PoId) <= 0)
                        {
                            RemoveId.Add(I.Id);
                        }
                    }
                }
                AvItem.RemoveAll(x => RemoveId.Any(z => z == x.Id));
                if (WithItems)
                {
                    foreach (List<Inv_receive_po_items> I in AvItem.SelectMany(x => x.Purchase_items).GroupBy(x=>x.Item_id).Select(x=>x.ToList()))
                    {
                        foreach (Inv_receive_po_items Is in I)
                        {
                            Is.Quantity = GetItemAvaliable(Is.Item_id,Is.Receive_po_id.Value);
                        }
                    }
                    AvItem.RemoveAll(x =>x.Purchase_items.Any(z=>z.Quantity==0));
                }

                return AvItem.ToList();
            }
        }
        public static List<InvoiceItems> GetItemAvaliableByVendore(int VendoreId)
        {
            using (DBContext db = new DBContext())

            {
                db.Configuration.ProxyCreationEnabled = false;
                List<InvoiceItems> AvItem = db.Inv_receive_po.Include(x => x.Payable).Include(x => x.Payable.Trans_doc_type)
                 .Where(x => x.Payable.Vendor_id == VendoreId)
                 .Select(x => new InvoiceItems { Id = x.Id, Trx = x.Payable.Trans_doc_type.Trx_num.ToString(), PoId = x.Id, Purchase_items = x.Items.ToList() }).ToList();
                List<int> RemoveId = new List<int>();
                foreach (var I in AvItem)
                {
                    foreach (var Items in I.Purchase_items.GroupBy(x => x.Item_id).Select(x => x.ToList()))
                    {
                        if (InvBus.GetItemAvaliable(Items.FirstOrDefault().Item_id, I.PoId) <= 0)
                        {
                            RemoveId.Add(I.Id);
                        }
                    }
                }
                AvItem.RemoveAll(x => RemoveId.Any(z => z == x.Id));
                return AvItem.ToList();
            }

        }
        public static List<InvoiceItems> GetItemAvaliableByCustomer(int CustomerId)
        {
            using (DBContext db = new DBContext())
            
                {
                db.Configuration.ProxyCreationEnabled = false;
                List<InvoiceItems> AvItem = db.Inv_sales_invoice.Include(x => x.Receivable).Include(x => x.Receivable.Trans_doc_type)
                 .Where(x => x.Receivable.Vendor_id == CustomerId)
                 .Select(x => new InvoiceItems { Id = x.Id, Trx = x.Receivable.Trans_doc_type.Trx_num.ToString(), PoId = x.Id, Sales_items = x.Inv_sales_item.ToList() }).ToList();
                List<int> RemoveId = new List<int>();
                foreach (var I in AvItem)
                {
                    foreach (var Items in I.Sales_items.GroupBy(x => x.Item_id).Select(x => x.ToList()))
                    {
                        if (InvBus.GetItemAvaliable(Items.FirstOrDefault().Item_id, I.PoId) <= 0)
                        {
                            RemoveId.Add(I.Id);
                        }
                    }
                }
                AvItem.RemoveAll(x => RemoveId.Any(z => z == x.Id));
                return AvItem.ToList();
            }
        }
        public static ItemDetails CalcItemDetails(int ItemId, float Qty, int StoreId, float? SoldItems = 0, bool GetAvaliable = false, int? UOM = null)
        {
           DBContext db = new DBContext();
            Inv_item ThisItem = db.Inv_item.Find(ItemId);

            Validation_method ItemVM = ThisItem.Validation_method;
            decimal UP = 0;
            decimal Total = 0;
            List<InvSalesPo> Po_inv = new List<InvSalesPo>();
            bool Avaliable = true;
            if (!SoldItems.HasValue)
            {
                SoldItems = 0;
            }
            float SoldItem = db.Inv_sales_invoice_items.Where(x => x.Item_id == ItemId).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(x => x.Quantity)+ SoldItems.Value;

            if (ThisItem.Martial_or_service == MartialService.Service)
            {
                return new ItemDetails { CostPrice = 0, Po_inv = new List<InvSalesPo> { new InvSalesPo { item_id = ItemId, Po_id = 0, Qty = 1 } }, TotalPrice = 0, Avaliable = Avaliable };
            }
            //if (SoldItem > 0
            //    && !GetAvaliable)
            //{
            //    float AvaliableItem = Unit_of_measureController.ItemEq(ItemId, ThisItem.Unit_of_measure_id.Value, db.Inv_receive_po_items.Where(x => x.Item_id == ItemId)
            //        .Sum(x => x.Quantity)) - Unit_of_measureController.ItemEq(ItemId, ThisItem.Unit_of_measure_id.Value, SoldItem);
            //    if (AvaliableItem < Qty)
            //    {
            //        Avaliable = false;
            //    }
            //}
            if (ItemVM == Validation_method.FIFO)
            {
                List<IGrouping<int?, Inv_receive_po_items>> I = db.Inv_receive_po_items
                          .Where(x => x.Item_id == ItemId && x.Receive_po.Store_id == StoreId).OrderBy(x => x.Receive_po_id)
                          .GroupBy(x => x.Receive_po_id).ToList();
                if (I.Count() == 0)
                {
                    Avaliable = false;
                }
                CalcDetilsVM(I, ItemId, Qty, StoreId, ref UP, ref Po_inv, ref Total, ref Avaliable, UOM, SoldItems);
            }
            else if (ItemVM == Validation_method.LIFO)
            {
                List<IGrouping<int?, Inv_receive_po_items>> I = db.Inv_receive_po_items.Include(x => x.Receive_po)
              .Where(x => x.Item_id == ItemId && x.Receive_po.Store_id == StoreId).OrderByDescending(x => x.Receive_po_id)
              .GroupBy(x => x.Receive_po_id).ToList();

                if (I.Count() == 0)
                {
                    Avaliable = false;
                }
                CalcDetilsVM(I, ItemId, Qty, StoreId, ref UP, ref Po_inv, ref Total, ref Avaliable, UOM, SoldItems);

            }
            else if (ItemVM == Validation_method.WA)
            {
                List<Inv_receive_po_items> I = db.Inv_receive_po_items.Include(x => x.Receive_po)
                 .Include(x => x.Receive_po).Where(x => x.Item_id == ItemId && x.Receive_po.Store_id == StoreId).ToList();

                float SelledQty = 0;
                bool SameStock = true;
                bool Break = false;
                float SumOfQtyRecipe = 0;
                bool OneRecipe = true;
                if (ItemHasRecipe(ItemId))
                {
                    var RecipeList = db.Inv_receive_po_items
                         .Where(x => db.Inv_item_recipe.Any(z => z.Recipe_item_id == x.Item_id)).OrderBy(x => x.Receive_po_id)
                         .ToList();
                    I.AddRange(RecipeList);
                    if (RecipeList.Count() > 1)
                    {
                        OneRecipe = false;
                    }
                    SumOfQtyRecipe = db.Inv_item_recipe.Where(x => x.Main_item_id == ItemId).Sum(x => x.Qty);
                }
                if (UOM.HasValue)
                {
                    Qty = CalcItemEq(ItemId, UOM.Value, Qty);
                }

                foreach (Inv_receive_po_items Item in I)
                {

                    float TQOut = db.Inv_sales_invoice_items.Where(x => x.Item_id == ItemId).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Quantity = 0 }).Sum(x => x.Quantity);
                    float ThisAv = I.Sum(x => x.Quantity)- SoldItems.Value;
                    if (ThisAv <= 0)
                    {
                        ThisAv = 1;
                    }
                    Item.Quantity -= InvBus.GetItemSoldInStore(Item.Item.Id, Item.Receive_po_id.Value, StoreId);
                    //db.Inv_sales_receivs_pos.Include(x => x.Item).Where(x => x.Receive_po_id == Item.Receive_po_id && x.Item_id == Item.Item.Id)
                    //                .ToList().DefaultIfEmpty(new Inv_sales_receivs_pos { Quantity = 0 }).Sum(x => x.Quantity);

                    Item.Quantity -= SoldItems.Value;

                    if (Item.Quantity <= 0)
                    {
                        continue;
                    }
                    decimal TICA = FabulousErp.Business.PostiveSubtract(I.Sum(x => x.Total) - I.Sum(x => x.Discount) + I.Sum(x => x.Fright)
                                   , db.Inv_sales_invoice_items.Where(x => x.Item_id == ItemId).ToList().DefaultIfEmpty(new Inv_sales_invoice_items { Total = 0 }).Sum(x => x.Total));

                    if (ItemHasRecipe(ItemId))
                    {
                        List<int> RecipeItems = InvRecipeItems(ItemId);
                        Qty = db.Inv_item_recipe.Where(x => x.Main_item_id == ItemId).ToList().Where(x => x.Recipe_item_id == I.FirstOrDefault().Item_id).FirstOrDefault().Qty;
                    }

                    if (OneRecipe)
                    {
                        if (SelledQty >= Qty)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (SelledQty >= SumOfQtyRecipe)
                        {
                            break;
                        }
                    }

                    if (Item.Quantity >= Qty && SameStock && OneRecipe)
                    {
                        UP += (TICA / (decimal)ThisAv) * (decimal)Qty;
                        Total += (TICA / (decimal)ThisAv) * (decimal)Qty;

                        Po_inv.Add(new InvSalesPo
                        {
                            Po_id = Item.Receive_po_id.Value,
                            Qty = Qty,
                            item_id = ItemId
                        });
                        Break = true;
                        break;
                    }
                    else
                    {
                        if (SelledQty + Item.Quantity > Qty)
                        {
                            Item.Quantity = (Qty - SelledQty);
                        }
                        SelledQty += Item.Quantity;

                        Po_inv.Add(new InvSalesPo
                        {
                            Po_id = Item.Receive_po_id.Value,
                            Qty = Item.Quantity,
                            item_id = ItemId
                        });
                        UP += (TICA / (decimal)ThisAv) * (decimal)Qty;
                        Total += (TICA / (decimal)ThisAv) * (decimal)Qty;
                        SameStock = false;
                    }
                    if (Break)
                    {
                        break;
                    }
                }
                if (I.Sum(x => x.Quantity) < Qty)
                {
                    return new ItemDetails { CostPrice = -1 };
                }
                UP = I.Sum(x => x.Total) / Convert.ToDecimal(I.Sum(x => x.Quantity));
            }
            if (UP == -1)
            {
                Avaliable = false;
            }
            return new ItemDetails { CostPrice = UP, Po_inv = Po_inv, TotalPrice = Total, Avaliable = Avaliable };
        }
        public static void CalcDetilsVM(List<IGrouping<int?, Inv_receive_po_items>> I, int ItemId, float Qty
        , int StoreId, ref decimal UP, ref List<InvSalesPo> Po_inv, ref decimal Total, ref bool Avaliable, int? UOM, float? SoldItems = 0)
        {
            if (!SoldItems.HasValue)
            {
                SoldItems = 0;
            }
           DBContext db = new DBContext();
            float SumOfQtyRecipe = 0;
            bool OneRecipe = true;
            if (ItemHasRecipe(ItemId))
            {
                var RecipeList = db.Inv_receive_po_items
                     .Where(x => db.Inv_item_recipe.Any(z => z.Recipe_item_id == x.Item_id)).OrderBy(x => x.Receive_po_id)
                     .GroupBy(x => x.Receive_po_id).ToList();
                I.AddRange(RecipeList);
                if (RecipeList.Count() > 1)
                {
                    OneRecipe = false;
                }
                SumOfQtyRecipe = db.Inv_item_recipe.Where(x => x.Main_item_id == ItemId).Sum(x => x.Qty);
            }
            
            float SelledQty = 0;
            bool SameStock = true;
            bool Break = false;
            int Count = 0;
            if (UOM.HasValue)
            {
                Qty = CalcItemEq(ItemId, UOM.Value, Qty);
            }
            foreach (List<Inv_receive_po_items> Fi in I.Select(x => x.ToList()))
            {
                List<Inv_receive_po_items> ThisFi = Fi.Where(x => x.Item_id == ItemId).ToList();
                if (ItemHasRecipe(ItemId))
                {
                    List<int> RecipeItems = InvRecipeItems(ItemId);
                    ThisFi = I.ToArray()[Count].Select(x => x).ToList();//.AddRange(Fi.Where(x => x.Item_id == ItemId).ToList());
                    ThisFi.ForEach(x => x.Item = db.Inv_item.Find(x.Item_id));
                    Qty = db.Inv_item_recipe.Where(x => x.Main_item_id == ItemId).ToList().Where(x => x.Recipe_item_id == ThisFi.FirstOrDefault().Item_id).FirstOrDefault().Qty;
                }


                foreach (Inv_receive_po_items Item in ThisFi)
                {


                    float MainQty = Item.Quantity;

                    Item.Quantity -=
                        InvBus.GetItemSoldInStore(Item.Item.Id, Item.Receive_po_id.Value, StoreId);
                    Item.Quantity -= SoldItems.Value;

                    //Unit_of_measureController.ItemEq(ItemId, Item.UOM_id.Value,
                    //db.Inv_sales_receivs_pos.Include(x => x.Item).Where(x => x.Receive_po_id == Item.Receive_po_id && x.Item_id == Item.Item.Id)
                    //            .ToList().DefaultIfEmpty(new Inv_sales_receivs_pos { Quantity = 0 }).Sum(x => x.Quantity);
                    //);


                    if (Item.Quantity <= 0)
                    {
                        continue;
                    }
                    //db.Inv_sales_invoice.Include(x => x.Inv_sales_item).Where(x => x.PO_id == Item.Receive_po_id
                    //              && x.Inv_sales_item.Any(z => z.Item_id == Item.Item_id)).ToList().DefaultIfEmpty(new Inv_sales_invoice { Inv_sales_item = new List<Inv_sales_invoice_items> { new Inv_sales_invoice_items { Quantity = 0 } } }).Select(x => x.Inv_sales_item.Sum(z => z.Quantity)).FirstOrDefault();
                    if (OneRecipe)
                    {
                        if (SelledQty >= Qty)
                        {
                            break;
                            Break = true;
                        }
                    }
                    else
                    {
                        if (SelledQty >= SumOfQtyRecipe)
                        {
                            break;
                            Break = true;
                        }
                    }

                    if (Item.Quantity >= Qty && SameStock && OneRecipe)
                    {
                        UP += (Item.Total - Item.Discount + Item.Fright) / (decimal)Item.Quantity; //Convert.ToDecimal(MainQty);
                        Po_inv.Add(new InvSalesPo
                        {
                            Po_id = Item.Receive_po_id.Value,
                            Qty = Qty,
                            item_id = ItemId
                        });
                        Break = true;
                        SelledQty = Item.Quantity;
                        Total += (decimal)Qty * UP;
                        break;
                    }
                    else
                    {
                        if (SelledQty + Item.Quantity > Qty)
                        {
                            if (OneRecipe)
                            {
                                Item.Quantity = (Qty - SelledQty);
                            }
                            else
                            {
                                Item.Quantity = (Qty);

                            }
                        }
                        SelledQty += Item.Quantity;

                        Po_inv.Add(new InvSalesPo
                        {
                            Po_id = Item.Receive_po_id.Value,
                            Qty = Item.Quantity,
                            item_id = ItemId
                        });
                        UP += Item.Unit_price; //(Item.Total - Item.Discount + Item.Fright) / Convert.ToDecimal(Item.Quantity);
                        Total += Item.Unit_price * (decimal)Item.Quantity; //(decimal)Item.Quantity * ((Item.Total - Item.Discount + Item.Fright) / Convert.ToDecimal(MainQty));
                        SameStock = false;
                    }
                }
                if (Break)
                {
                    break;
                }
                Count++;
            }
            if (SelledQty < Qty)
            {
                Avaliable = false;
                //UP = -1;
            }
            //return new ItemDetails { CostPrice = UP, Po_inv = Po_inv, TotalPrice = Total, Avaliable = Avaliable };
        }
        public static void GetUnitPrice(List<IGrouping<int?, Inv_receive_po_items>> I, int ItemId, float Qty
       , int StoreId, ref decimal UP, ref decimal Total, int? UOM, float? SoldItems = 0)
        {
            List<InvSalesPo> Po_inv = new List<InvSalesPo> { };
            if (!SoldItems.HasValue)
            {
                SoldItems = 0;
            }
           DBContext db = new DBContext();
            float SumOfQtyRecipe = 0;
            bool OneRecipe = true;
            if (ItemHasRecipe(ItemId))
            {
                var RecipeList = db.Inv_receive_po_items
                     .Where(x => db.Inv_item_recipe.Any(z => z.Recipe_item_id == x.Item_id)).OrderBy(x => x.Receive_po_id)
                     .GroupBy(x => x.Receive_po_id).ToList();
                I.AddRange(RecipeList);
                if (RecipeList.Count() > 1)
                {
                    OneRecipe = false;
                }
                SumOfQtyRecipe = db.Inv_item_recipe.Where(x => x.Main_item_id == ItemId).Sum(x => x.Qty);
            }

            float SelledQty = 0;
            bool SameStock = true;
            bool Break = false;
            int Count = 0;
            if (UOM.HasValue)
            {
                Qty = CalcItemEq(ItemId, UOM.Value, Qty);
            }
            foreach (List<Inv_receive_po_items> Fi in I.Select(x => x.ToList()))
            {
                List<Inv_receive_po_items> ThisFi = Fi.Where(x => x.Item_id == ItemId).ToList();
                if (ItemHasRecipe(ItemId))
                {
                    List<int> RecipeItems = InvRecipeItems(ItemId);
                    ThisFi = I.ToArray()[Count].Select(x => x).ToList();//.AddRange(Fi.Where(x => x.Item_id == ItemId).ToList());
                    ThisFi.ForEach(x => x.Item = db.Inv_item.Find(x.Item_id));
                    Qty = db.Inv_item_recipe.Where(x => x.Main_item_id == ItemId).ToList().Where(x => x.Recipe_item_id == ThisFi.FirstOrDefault().Item_id).FirstOrDefault().Qty;
                }


                foreach (Inv_receive_po_items Item in ThisFi)
                {


                    float MainQty = Item.Quantity;

                    Item.Quantity -=
                        InvBus.GetItemSoldInStore(ItemId, Item.Receive_po_id.Value, StoreId);
                    Item.Quantity -= SoldItems.Value;

                    //Unit_of_measureController.ItemEq(ItemId, Item.UOM_id.Value,
                    //db.Inv_sales_receivs_pos.Include(x => x.Item).Where(x => x.Receive_po_id == Item.Receive_po_id && x.Item_id == Item.Item.Id)
                    //            .ToList().DefaultIfEmpty(new Inv_sales_receivs_pos { Quantity = 0 }).Sum(x => x.Quantity);
                    //);

                    if (Item.Quantity <= 0)
                    {
                        continue;
                    }
                    //db.Inv_sales_invoice.Include(x => x.Inv_sales_item).Where(x => x.PO_id == Item.Receive_po_id
                    //              && x.Inv_sales_item.Any(z => z.Item_id == Item.Item_id)).ToList().DefaultIfEmpty(new Inv_sales_invoice { Inv_sales_item = new List<Inv_sales_invoice_items> { new Inv_sales_invoice_items { Quantity = 0 } } }).Select(x => x.Inv_sales_item.Sum(z => z.Quantity)).FirstOrDefault();
                    if (OneRecipe)
                    {
                        if (SelledQty >= Qty)
                        {
                            break;
                            Break = true;
                        }
                    }
                    else
                    {
                        if (SelledQty >= SumOfQtyRecipe)
                        {
                            break;
                            Break = true;
                        }
                    }

                    if (Item.Quantity >= Qty && SameStock && OneRecipe)
                    {
                        UP += (Item.Total - Item.Discount + Item.Fright) / (decimal)Item.Quantity; //Convert.ToDecimal(MainQty);
                        Po_inv.Add(new InvSalesPo
                        {
                            Po_id = Item.Receive_po_id.Value,
                            Qty = Qty,
                            item_id = ItemId
                        });
                        Break = true;
                        SelledQty = Item.Quantity;
                        Total += (decimal)Qty * UP;
                        break;
                    }
                    else
                    {
                        if (SelledQty + Item.Quantity > Qty)
                        {
                            if (OneRecipe)
                            {
                                Item.Quantity = (Qty - SelledQty);
                            }
                            else
                            {
                                Item.Quantity = (Qty);

                            }
                        }
                        SelledQty += Item.Quantity;

                        Po_inv.Add(new InvSalesPo
                        {
                            Po_id = Item.Receive_po_id.Value,
                            Qty = Item.Quantity,
                            item_id = ItemId
                        });
                        UP += Item.Unit_price; //(Item.Total - Item.Discount + Item.Fright) / Convert.ToDecimal(Item.Quantity);
                        Total += Item.Unit_price * (decimal)Item.Quantity; //(decimal)Item.Quantity * ((Item.Total - Item.Discount + Item.Fright) / Convert.ToDecimal(MainQty));
                        SameStock = false;
                    }
                }
                if (Break)
                {
                    break;
                }
                Count++;
            }
            //return new ItemDetails { CostPrice = UP, Po_inv = Po_inv, TotalPrice = Total, Avaliable = Avaliable };
        }

        public static float CalcItemEq(int Id, int? OtherUnit, bool IsMulti = true)
        {
            if (!OtherUnit.HasValue)
            {
                return 1;
            }
           DBContext db = new DBContext();
            Inv_item Item = db.Inv_item.Include(x => x.Unit_of_measure).FirstOrDefault(x => x.Id == Id);
            Unit_of_measure UOM = Item.Unit_of_measure;
            float Qty = 1;
            if (OtherUnit == Item.Unit_of_measure_id)
            {
                return Qty;
            }
            Unit_of_measure EUOM = db.Unit_of_measures.Find(OtherUnit);
            if (IsMulti)
            {
                while (EUOM.Equivalante_id.HasValue)
                {
                    OtherUnit = EUOM.Equivalante_id.Value;
                    Qty *= EUOM.Equivalante_quantity;
                    EUOM = db.Unit_of_measures.Find(OtherUnit);
                }
                return EUOM.Equivalante_quantity * Qty;
            }
            else
            {
                while (EUOM.Equivalante_id.HasValue)
                {
                    OtherUnit = EUOM.Equivalante_id.Value;
                    Qty /= EUOM.Equivalante_quantity;
                    EUOM = db.Unit_of_measures.Find(OtherUnit);
                }
                return Qty;
            }
        }
        public static float CalcItemEq(int Id, int? OtherUnit, float Qty, bool IsMulti = true)
        {
            try
            {
                if (!OtherUnit.HasValue)
                {
                    return Qty;
                }
                DBContext db = new DBContext();
                Inv_item Item = db.Inv_item.Include(x => x.Unit_of_measure).FirstOrDefault(x => x.Id == Id);
                Unit_of_measure UOM = Item.Unit_of_measure;
                if (OtherUnit == Item.Unit_of_measure_id)
                {
                    return Qty;
                }
                Unit_of_measure EUOM = db.Unit_of_measures.Find(OtherUnit);

                if (IsMulti)
                {
                    while (EUOM.Equivalante_id.HasValue)
                    {
                        OtherUnit = EUOM.Equivalante_id.Value;
                        Qty *= EUOM.Equivalante_quantity;
                        EUOM = db.Unit_of_measures.Find(OtherUnit);
                    }
                    return EUOM.Equivalante_quantity * Qty;
                }
                else
                {
                    while (EUOM.Equivalante_id.HasValue)
                    {
                        OtherUnit = EUOM.Equivalante_id.Value;
                        Qty /= EUOM.Equivalante_quantity;
                        EUOM = db.Unit_of_measures.Find(OtherUnit);
                    }
                    return Qty;
                }
            }
            catch
            {
                return Qty;
            }
           
        }
        public static decimal CalcItemEqUnitPrice(int ItemId, int OtherUnit, float Qty, decimal UnitPrice, bool IsMulti = false)
        {
           DBContext db = new DBContext();
            Inv_item Item = db.Inv_item.Include(x => x.Unit_of_measure).FirstOrDefault(x => x.Id == ItemId);
            Unit_of_measure UOM = Item.Unit_of_measure;
            if (OtherUnit == Item.Unit_of_measure_id)
            {
                return UnitPrice;
            }
            Unit_of_measure EUOM = db.Unit_of_measures.Find(OtherUnit);
            if (IsMulti)
            {
                while (EUOM.Equivalante_id.HasValue)
                {
                    OtherUnit = EUOM.Equivalante_id.Value;
                    UnitPrice *= (decimal)EUOM.Equivalante_quantity;
                    EUOM = db.Unit_of_measures.Find(OtherUnit);
                }

            }
            else
            {
                while (EUOM.Equivalante_id.HasValue)
                {
                    OtherUnit = EUOM.Equivalante_id.Value;
                    UnitPrice /= (decimal)EUOM.Equivalante_quantity;
                    EUOM = db.Unit_of_measures.Find(OtherUnit);
                }
            }

            return UnitPrice;
            // return UnitPrice * Qty;
        }
        public static bool ItemHasRecipe(int ItemId)
        {
          
            using (DBContext db = new DBContext())
            {
                if (db.Inv_item_recipe.Any(x => x.Main_item_id == ItemId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static List<int> InvRecipeItems(int ItemId)
        {
            using (DBContext db = new DBContext())
            {
                return db.Inv_item_recipe.Where(x => x.Main_item_id == ItemId).Select(x => x.Recipe_item_id).ToList();
            }
        }
        public static bool HasGs()
        {

            using (DBContext db = new DBContext())
            {
                return db.Inv_movment_GS.Any();
            }
        }
        public static int GetFullPayTrxNum(int SalesId)
        {
            return MyDbContext.Instance
                 .Inv_receivable_num
             .Include(x => x.Rec_inv).Where(x => x.Inv_sales_id == SalesId)
             .ToList().DefaultIfEmpty(new FabulousDB.Models.Inventory.Inv_receivable_num { })
             .FirstOrDefault().Rec_inv.Trx_num;
        }
    }

}