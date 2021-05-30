using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousDB.Models;
using FabulousErp.Payable.Models;
using FabulousDB.DB_Context;
using FabulousErp.Receivable.Models;
using FabulousDB.DB_Context;
using Payable.Controllers;
using Receivable.Controllers;
using FabulousModels.Inventory;
using FabulousErp;
using FixedAssets.Business;

namespace Inventory.Controllers
{
    public class Inv_receive_poController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext Pdb = new DBContext();
        private DBContext Rdb = new DBContext();
        string CompanyId = FabulousErp.Business.GetCompanyId();

        #region Crud
        // GET: Inventory/Inv_receive_po
        public ActionResult Index()
        {
            var inv_receive_po = db.Inv_receive_po.Include(i => i.Currency).Include(i => i.PO).Include(i => i.Site).Include(i => i.Store).Include(i => i.Vendore);
            return View(inv_receive_po.ToList());
        }

        // GET: Inventory/Inv_receive_po/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_receive_po inv_receive_po = db.Inv_receive_po.Find(id);
            if (inv_receive_po == null)
            {
                return HttpNotFound();
            }
            return View(inv_receive_po);
        }
        public JsonResult GetVendoreInvoice(int VendoreId)
        {
            List<InvoiceItems> AvItem = db.Inv_receive_po.Include(x => x.Payable).Include(x => x.Payable.Trans_doc_type)
               .Where(x => x.Payable.Vendor_id == VendoreId)
               .Select(x => new InvoiceItems { Id = x.Id, Trx = x.Payable.VDocument_number, PoId = x.Id }).ToList();

            return Json(AvItem);
            //List<InvoiceItems> AvItem = InvBus.GetItemAvaliableByVendore(VendoreId);
            //AvItem.ForEach(x => x.Purchase_items = null);
            //return Json(AvItem);
        }
        // GET: Inventory/Inv_receive_po/Create
        public ActionResult Create(bool Sales = false)
        {
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode", db.CurrenciesDefinition_Tables.FirstOrDefault(x => x.CurrencyID == CompanyId).CurrencyID);
            ViewBag.PO_id = new SelectList(db.Inv_po, "Id", "Po_num");
            ViewBag.Site_id = new SelectList(new List<string> { }/*db.Inv_store_site, "Id", "Site_id"*/);
            ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");
            ViewBag.FullPay = false;
            if (Request["FullPay"] == "true")
            {
                ViewBag.FullPay = true;
                ViewBag.Checkbook_id= new SelectList(db.C_CheckBookSetting_Tables.ToList(), "C_CBSID", "C_CheckbookID");
            }
            if (Sales == false)
            {
                ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting.Select(x => new { x.Id, Vendor_id = x.Vendor_id.ToString() + " - " + x.Vendor_name }), "Id", "Vendor_id");
                ViewBag.GNums = new SelectList(new List<string>());
                ViewBag.ShowJv = db.Inv_po_GS.ToList().
                    DefaultIfEmpty(new Inv_po_GS { Allow_View_jv = false })
                    .FirstOrDefault().Allow_View_jv?"":"hide";
            }
            else
            {
                ViewBag.Vendore_id = new SelectList(Rdb.Receivable_vendore_settings.Select(x => new { x.Id, Vendor_id = x.Vendor_id.ToString() + " - " + x.Vendor_name }), "Id", "Vendor_id");

                ViewBag.GNums = new SelectList(new List<string>());
                ViewBag.ShowJv = db.Inv_sales_GS.ToList().
                   DefaultIfEmpty(new Inv_sales_GS { Allow_View_jv = false })
                   .FirstOrDefault().Allow_View_jv ? "" : "hide";
            }

            ViewBag.Doc_type = new SelectList(Enum.GetValues(typeof(Doc_type))
                                                .Cast<Doc_type>()
                                                .Select(x => new { Text = x.ToString(), Value = (int)x })
                                                .Where(x => x.Text == Doc_type.Invoice.ToString() || x.Text == Doc_type.Return.ToString())
                                                .ToList(), "Value", "Text");

            ViewBag.Payment_terms_id = new SelectList(Pdb.Payment_terms
                                                .ToList(), "Id", "Terms_id");
            ViewBag.Shipping_method_id = new SelectList(Pdb.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method");
            ViewBag.Sales = Sales;
            ViewBag.Profit_user_id = new SelectList(db.CreateAccount_Tables, "UserID", "UserName");
            ViewBag.HasTax = db.C_TaxSetting_Tables.Any();
            if (Sales)
            {
                Inv_sales_GS SalesGs = db.Inv_sales_GS.FirstOrDefault();
                if (SalesGs.Override_price_in_price_list
                    && !SalesGs.Allow_edit_sales_price)
                {
                    SalesGs.Allow_edit_sales_price = true;
                }
                ViewBag.SalesGs = db.Inv_sales_GS.ToList().DefaultIfEmpty(new Inv_sales_GS { Allow_edit_sales_price = false }).FirstOrDefault();
                ViewBag.ShowCostPrice = db.Inv_sales_GS.ToList().DefaultIfEmpty().FirstOrDefault()
                .Show_cost_price.ToString().ToLower().Replace("false", "hide");
            }
            else
            {
             
                ViewBag.SalesGs = new Inv_sales_GS {  };
            }
            return View();
        }

        public JsonResult GetVName(int SV)
        {
            return Json(
                 db.Payable_creditor_setting.ToList()
                 .Select(x => new { x.Id, Name = FabulousErp.Business.GetVendoreByDV(SV, x) })
                 );
        }
        public JsonResult GetCName(int SV)
        {
            return Json(db.Receivable_vendore_settings.ToList()
                 .Select(x => new { x.Id, Name = FabulousErp.Business.GetCustomerByDV(SV, x) })
                 );
        }
        // POST: Inventory/Inv_receive_po/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inv_receive_po inv_receive_po, bool Sales = false, bool Piece = false,bool FullPay=false)
        {
            if (inv_receive_po.Doc_type == Doc_type.Return && Sales == false)
            {
                Sales = true;
            }
            else if (inv_receive_po.Doc_type == Doc_type.Return && Sales == true)
            {
                Sales = false;
            }
            if (Sales == true && (inv_receive_po.Doc_type != Doc_type.Return))
            {
                inv_receive_po.Doc_date = inv_receive_po.Transaction_date.Value;
                inv_receive_po.Po_inv_list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InvSalesPo>>(inv_receive_po.Po_inv);
            }
            if (ModelState.IsValid)
            {
                if (Sales == false)
                {
                    int Max = 1;
                    try
                    {
                        Max = db.Inv_store.Where(x => x.Id == inv_receive_po.Store_id)
                              .Max(x => x.Next_gr_no);
                        db.Inv_store.Find(inv_receive_po.Store_id).Next_gr_no = Max + 1;
                    }
                    catch
                    { }

                    if (inv_receive_po.Doc_type == Doc_type.Return)
                    {
                        inv_receive_po.Customer_id = inv_receive_po.Vendore_id;
                        inv_receive_po.Vendore_id = null;
                    }

                    inv_receive_po.Gr_num = Max;
                    db.Inv_receive_po.Add(inv_receive_po);

                    db.SaveChanges();
                    // Add To Payable Or Receivable
                    if (!FullPay)
                    {
                        dynamic PayableRes;
                        if (Doc_type.Invoice == inv_receive_po.Doc_type)
                        {
                            using (Payable_transactionController P = new Payable_transactionController())
                            {
                                PayableRes = P.AddPayableTransaction(new Payable_transaction
                                {
                                    Currency_id = inv_receive_po.Currency_id,
                                    Vendor_id = inv_receive_po.Vendore_id,
                                    Doc_date = inv_receive_po.Doc_date,
                                    Due_date = inv_receive_po.Due_date,
                                    Transaction_date = inv_receive_po.Transaction_date.Value,
                                    Posting_date = inv_receive_po.Posting_date.Value,
                                    Doc_type = (Doc_type)inv_receive_po.Doc_type,
                                    Desc = inv_receive_po.Desc,
                                    Creation_date = DateTime.Now,
                                    Journal_number = inv_receive_po.Journal_number,
                                    Shipping_method_id = inv_receive_po.Shipping_method_id,
                                    Purchase = inv_receive_po.Net_amount,
                                    Taken_discount = inv_receive_po.Discount,
                                    Tax = inv_receive_po.Tax,
                                    System_rate = inv_receive_po.System_rate,
                                    Transaction_rate = inv_receive_po.Transaction_rate,
                                    VDocument_number = inv_receive_po.Vendore_doc_number
                                });
                                inv_receive_po.Payable_id = PayableRes.PayId;
                                db.SaveChanges();
                                return Json(new { Id = inv_receive_po.Id, GR_no = Max, Trx_num =Convert.ToInt32(PayableRes.Trx_num)+1, PayId = inv_receive_po.Payable_id });
                            }
                        }
                        else
                        {
                            dynamic ReseveRes;
                            using (Receivable_transactionController P = new Receivable_transactionController())
                            {
                                ReseveRes = P.AddReceivableTransaction(new Receivable_transaction
                                {
                                    Currency_id = inv_receive_po.Currency_id,
                                    Vendor_id = inv_receive_po.Customer_id,
                                    Doc_date = inv_receive_po.Doc_date,
                                    Due_date = inv_receive_po.Due_date,
                                    Transaction_date = inv_receive_po.Transaction_date.Value,
                                    Posting_date = inv_receive_po.Posting_date.Value,
                                    Doc_type = (Doc_type)inv_receive_po.Doc_type,
                                    Desc = inv_receive_po.Desc,
                                    Creation_date = DateTime.Now,
                                    Journal_number = inv_receive_po.Journal_number,
                                    Shipping_method_id = inv_receive_po.Shipping_method_id,
                                    Purchase = inv_receive_po.Net_amount,
                                    Taken_discount = inv_receive_po.Discount,
                                    Tax = inv_receive_po.Tax,
                                    System_rate = inv_receive_po.System_rate,
                                    Transaction_rate = inv_receive_po.Transaction_rate,
                                }, true);
                                int BookId = 0;
                                string BookName = "";
                                try
                                {
                                    if (Piece)
                                    {
                                        dynamic Payment = AddRecPayment(ReseveRes.TrxId, inv_receive_po.Profit_user_id
                                             , inv_receive_po.Transaction_date.Value
                                             , inv_receive_po.Posting_date.Value,
                                             inv_receive_po.Customer_id.Value,
                                             inv_receive_po.Desc,
                                             inv_receive_po.Journal_number,
                                             inv_receive_po.Payamnet, inv_receive_po.Transaction_rate, inv_receive_po.System_rate);
                                        BookId = Payment.Data.BookId;
                                        BookName = Payment.Data.BookName;
                                    }
                                }
                                catch
                                {

                                }

                                inv_receive_po.Receivable_id = ReseveRes.RecId;
                                db.SaveChanges();
                                return Json(new { Id = inv_receive_po.Id, RecId = inv_receive_po.Receivable_id, GR_no = Max, Trx_num = Convert.ToInt32(ReseveRes.Trx_num)+1, Trx_counter = ReseveRes.Counter , BookId , BookName});
                            }
                        }
                    }
                    else
                    {
                        using (Payable_paymentController P = new Payable_paymentController())
                        {
                            var CheckBook = P.AddToCheckBook(new Payable_payment
                            {
                                Cash_type = Cash_type.Cash,
                                Currency_id = inv_receive_po.Currency_id,
                                Due_date = null,
                                Journal_number = inv_receive_po.Journal_number,
                                PostingNumber = FabulousErp.Business.GetPotingNumber(inv_receive_po.Journal_number),
                                Orginal_amount = inv_receive_po.Total,
                                ReciptAmount = 0,
                                Transaction_date = inv_receive_po.Transaction_date.Value,
                                Posting_date = inv_receive_po.Posting_date.Value,
                                Reference = inv_receive_po.Desc,
                                System_rate = inv_receive_po.System_rate,
                                Transaction_rate = inv_receive_po.Transaction_rate,
                                Taken_discount = inv_receive_po.Discount,
                                UserId = inv_receive_po.Profit_user_id,
                                Check_book_id = inv_receive_po.Checkbook_id,
                                Payment_To_Recieved_From = "Cash"
                            }, "");
                            return Json(new { Id = inv_receive_po.Id, GR_no = Max, BookId = CheckBook.C_DocumentNumber });
                        }
                    }
                }
                else
                {
                    int Max = 1;
                    try
                    {
                        Max = db.Inv_store.Where(x => x.Id == inv_receive_po.Store_id)
                              .Max(x => x.Next_goods_no);

                        db.Inv_store.Find(inv_receive_po.Store_id).Next_goods_no = Max + 1;
                        db.SaveChanges();

                    }
                    catch
                    { }
                    Inv_sales_invoice inv_sales_invoice = new Inv_sales_invoice
                    {
                        //Batch_id= inv_receive_po.Batch_id,
                        //Buyer= inv_receive_po.Buyer,
                        Currency_id = inv_receive_po.Currency_id,
                        // Customer_id = inv_receive_po.Vendore_id,
                        Desc = inv_receive_po.Desc,
                        Doc_date = inv_receive_po.Doc_date,
                        Doc_type = inv_receive_po.Doc_type,
                        Discount = inv_receive_po.Discount,
                        Journal_number = inv_receive_po.Journal_number,
                        Net_amount = inv_receive_po.Net_amount,
                        Shipping_method_id = inv_receive_po.Shipping_method_id,
                        Payment_terms_id = inv_receive_po.Payment_terms_id,
                        Posting_date = inv_receive_po.Posting_date,
                        Site_id = inv_receive_po.Site_id,
                        Store_id = inv_receive_po.Store_id,
                        Tax = inv_receive_po.Tax,
                        Transaction_date = inv_receive_po.Transaction_date,
                        Vendore_doc_number = inv_receive_po.Vendore_doc_number,
                        //Inv_po_Id= inv_receive_po.PO_id.Value
                        Profit_user_id= inv_receive_po.Profit_user_id,
                        Posting_number= inv_receive_po.Posting_number
                    };
                    if (inv_receive_po.Doc_type == Doc_type.Invoice)
                    {
                        inv_sales_invoice.Customer_id = inv_receive_po.Vendore_id;
                    }
                    else
                    {
                        inv_sales_invoice.Vendore_id = inv_receive_po.Vendore_id;
                    }
                    db.Inv_sales_invoice.Add(inv_sales_invoice);


                    inv_sales_invoice.Go_num = Max;
                    List<Inv_sales_receivs_pos> POS = new List<Inv_sales_receivs_pos>();
                    if (inv_receive_po.Doc_type != Doc_type.Return)
                    {
                        foreach (InvSalesPo i in inv_receive_po.Po_inv_list)
                        {
                            if (i.Po_id != 0)
                            {
                                POS.Add(new Inv_sales_receivs_pos
                                {
                                    Receive_po_id = i.Po_id,
                                    Sales_id = inv_sales_invoice.Id,
                                    Item_id = i.item_id,
                                    Quantity = i.Qty
                                });
                            }

                        }
                        db.Inv_sales_receivs_pos.AddRange(POS);
                    }

                    db.SaveChanges();
                    if (!FullPay)
                    {
                        if (inv_receive_po.Doc_type == Doc_type.Invoice)
                        {
                            dynamic ReseveRes;
                            using (Receivable_transactionController P = new Receivable_transactionController())
                            {
                                ReseveRes = P.AddReceivableTransaction(new Receivable_transaction
                                {
                                    Currency_id = inv_receive_po.Currency_id,
                                    Vendor_id = inv_receive_po.Vendore_id,
                                    Doc_date = inv_receive_po.Doc_date,
                                    Due_date = inv_receive_po.Due_date,
                                    Transaction_date = inv_receive_po.Transaction_date.Value,
                                    Posting_date = inv_receive_po.Posting_date.Value,
                                    Doc_type = (Doc_type)inv_receive_po.Doc_type,
                                    Desc = inv_receive_po.Desc,
                                    Creation_date = DateTime.Now,
                                    Journal_number = inv_receive_po.Journal_number,
                                    Shipping_method_id = inv_receive_po.Shipping_method_id,
                                    Purchase = inv_receive_po.Net_amount,
                                    Taken_discount = inv_receive_po.Discount,
                                    Tax = inv_receive_po.Tax,
                                    System_rate = inv_receive_po.System_rate,
                                    Transaction_rate = inv_receive_po.Transaction_rate,
                                }, true);

                                inv_sales_invoice.Receivable_id = ReseveRes.RecId;
                                db.SaveChanges();
                                int BookId = 0;
                                string BookName = "";

                                try
                                {

                                    if (Piece && inv_receive_po.Payamnet!=null)
                                    {
                                        dynamic Payment = AddRecPayment(ReseveRes.TrxId, inv_receive_po.Profit_user_id
                                       , inv_receive_po.Transaction_date.Value
                                       , inv_receive_po.Posting_date.Value,
                                       inv_receive_po.Vendore_id.Value,
                                       inv_receive_po.Desc,
                                       inv_receive_po.Journal_number,
                                       inv_receive_po.Payamnet, inv_receive_po.Transaction_rate, inv_receive_po.System_rate);
                                        BookId = Payment.Data.BookId;
                                        BookName = Payment.Data.BookName;
                                    }

                                }
                                catch
                                {

                                }


                                return Json(new { Id = inv_sales_invoice.Id, RecId = inv_sales_invoice.Receivable_id, GR_no = Max, Trx_num = Convert.ToInt32(ReseveRes.Trx_num)+1, Trx_counter = ReseveRes.Counter, BookId, BookName });
                            }

                        }
                        else
                        {
                            dynamic PayableRes;
                            using (Payable_transactionController P = new Payable_transactionController())
                            {
                                PayableRes = P.AddPayableTransaction(new Payable_transaction
                                {
                                    Currency_id = inv_receive_po.Currency_id,
                                    Vendor_id = inv_receive_po.Vendore_id,
                                    Doc_date = inv_receive_po.Doc_date,
                                    Due_date = inv_receive_po.Due_date,
                                    Transaction_date = inv_receive_po.Transaction_date.Value,
                                    Posting_date = inv_receive_po.Posting_date.Value,
                                    Doc_type = (Doc_type)inv_receive_po.Doc_type,
                                    Desc = inv_receive_po.Desc,
                                    Creation_date = DateTime.Now,
                                    Journal_number = inv_receive_po.Journal_number,
                                    Shipping_method_id = inv_receive_po.Shipping_method_id,
                                    Purchase = inv_receive_po.Net_amount,
                                    Taken_discount = inv_receive_po.Discount,
                                    Tax = inv_receive_po.Tax,
                                    System_rate = inv_receive_po.System_rate,
                                    Transaction_rate = inv_receive_po.Transaction_rate,
                                    VDocument_number = inv_receive_po.Vendore_doc_number
                                });
                                inv_sales_invoice.Payable_id = PayableRes.PayId;
                                inv_sales_invoice.Go_num = Max;
                                db.SaveChanges();
                                return Json(new { Id = inv_sales_invoice.Id, GR_no = Max, Trx_num = Convert.ToInt32(PayableRes.Trx_num), PayId = inv_sales_invoice.Payable_id });
                            }
                        }
                    }
                    else
                    {
                        using (Receivable_paymentController R=new Receivable_paymentController())
                        {
                            if (db.Receivable_payments.Any())
                            {
                                Max = (int)db.Receivable_payments.Max(x => x.Payment_no) + 1;
                            }
                            Receivable_transactions_types PTY = new Receivable_transactions_types
                            {
                                Counter =Receivable.Controllers.Business.GetNextDocNumber(Doc_type.Payment),
                                Doc_type = Doc_type.Payment,
                                Trx_num = Receivable.Controllers.Business.TrxNum(),
                                Origin = TrxPay.Pay
                            };
                            db.Receivable_transactions_types.Add(PTY);
                           
                            db.SaveChanges();

                            var CheckBook= R.AddToCheckBook(new Receivable_payment
                            {
                                Cash_type=Cash_type.Cash,
                                Currency_id= inv_receive_po.Currency_id,
                                Due_date=null,
                                Journal_number = inv_receive_po.Journal_number,
                                PostingNumber = FabulousErp.Business.GetPotingNumber(inv_receive_po.Journal_number),
                                Orginal_amount= inv_receive_po.Total,
                                Transaction_date= inv_receive_po.Transaction_date.Value,
                                Posting_date= inv_receive_po.Posting_date.Value,
                                Profitable_user= inv_receive_po.Profit_user_id,
                                Reference= inv_receive_po.Desc,
                                System_rate= inv_receive_po.System_rate,
                                Transaction_rate= inv_receive_po.Transaction_rate,
                                Taken_discount= inv_receive_po.Discount,
                                UserId= inv_receive_po.Profit_user_id,
                                Check_book_id= inv_receive_po.Checkbook_id,
                                Payment_To_Recieved_From="Cash",
                                
                           } ,"");
                            db.Inv_receivable_num.Add(new FabulousDB.Models.Inventory.Inv_receivable_num
                            {
                                Checkbook_id = CheckBook.C_CBT,
                                Inv_sales_id= inv_sales_invoice.Id,
                                Inv_num_id= PTY.Id
                            });
                            db.SaveChanges();
                            return Json(new { Id = inv_sales_invoice.Id, GR_no = Max, BookId = CheckBook.C_DocumentNumber});
                        }
                    }
                }

            }

            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode", inv_receive_po.Currency_id);
            ViewBag.PO_id = new SelectList(db.Inv_po, "Id", "Po_num", inv_receive_po.PO_id);
            ViewBag.Site_id = new SelectList(db.Inv_store_site, "Id", "Site_id", inv_receive_po.Site_id);
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_receive_po.Store_id);
            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", inv_receive_po.Vendore_id);
            return View(inv_receive_po);
        }
        public dynamic AddRecPayment(int Trans_doc_type_id, string ProfitableUser, DateTime TrxDate, DateTime PostDate
            , int CustomerId, string Ref, int Jn, decimal? Orginal_amount = 0,decimal Transaction_rate=1
            ,decimal System_rate=1)
        {
            using (Receivable_paymentController RP = new Receivable_paymentController())
            {
                int CheckBookId = db.C_CheckBookSetting_Tables.FirstOrDefault(x => x.C_UserIDAccess == ProfitableUser)
                    .C_CBSID;
                return RP.Create(new Receivable_payment
                {
                    Cash_type = Cash_type.Cash,
                    Check_book_id = CheckBookId,
                    Transaction_date = TrxDate,
                    Posting_date = PostDate,
                    Vendor_id = CustomerId,
                    Reference = Ref,
                    Creation_date = DateTime.Now,
                    System_rate = System_rate,
                    Transaction_rate = Transaction_rate,
                    Orginal_amount = Orginal_amount.Value,
                    Trx_trans_doc_type_id = Trans_doc_type_id,
                    Journal_number = Jn,
                    Profitable_user = ProfitableUser,
                });
            }
        }
        public JsonResult GetUserCheckBook(string ProfitableUser)
        {
            return Json(db.C_CheckBookSetting_Tables.Where(x => x.C_UserIDAccess == ProfitableUser)
                .ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook.C_CheckBookSetting_table { 
                 C_CBSID=0
                }).FirstOrDefault().C_CBSID);
        }

        // GET: Inventory/Inv_receive_po/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_receive_po inv_receive_po = db.Inv_receive_po.Find(id);
            if (inv_receive_po == null)
            {
                return HttpNotFound();
            }
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode", inv_receive_po.Currency_id);
            ViewBag.PO_id = new SelectList(db.Inv_po, "Id", "Po_num", inv_receive_po.PO_id);
            ViewBag.Site_id = new SelectList(db.Inv_store_site, "Id", "Site_id", inv_receive_po.Site_id);
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_receive_po.Store_id);
            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", inv_receive_po.Vendore_id);
            return View(inv_receive_po);
        }

        // POST: Inventory/Inv_receive_po/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GR_no,Doc_type,PO_id,Store_id,Site_id,Date,Vendore_id,Batch_id,Buyer,Vendore_inv_number,Currency_id")] Inv_receive_po inv_receive_po)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_receive_po).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode", inv_receive_po.Currency_id);
            ViewBag.PO_id = new SelectList(db.Inv_po, "Id", "Po_num", inv_receive_po.PO_id);
            ViewBag.Site_id = new SelectList(db.Inv_store_site, "Id", "Site_id", inv_receive_po.Site_id);
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_receive_po.Store_id);
            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", inv_receive_po.Vendore_id);
            return View(inv_receive_po);
        }

        // GET: Inventory/Inv_receive_po/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_receive_po inv_receive_po = db.Inv_receive_po.Find(id);
            if (inv_receive_po == null)
            {
                return HttpNotFound();
            }
            return View(inv_receive_po);
        }

        // POST: Inventory/Inv_receive_po/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_receive_po inv_receive_po = db.Inv_receive_po.Find(id);
            db.Inv_receive_po.Remove(inv_receive_po);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult GetPoInvoice(int Po,bool GoodsRecipts=false)
        {
            ViewBag.Po = Po;
            Inv_receive_po Inv_rece = db.Inv_receive_po.Where(x => x.Id == Po).Include(x => x.Store)
                .Include(x => x.Site)
                .Include(x => x.Profit_user)
                .Include(x => x.Shipping_methods)
                .Include(x => x.Payable)
                .Include(x => x.Payable.Vendor)
                .Include(x => x.Payment_terms).FirstOrDefault();

            ViewBag.PostringNumber =FabulousErp.Business.GetPotingNumber(Inv_rece.Payable.Journal_number);
            ViewBag.Jn =Inv_rece.Payable.Journal_number;
            ViewBag.GoodsRecipts = GoodsRecipts;
            return View(Inv_rece);
        } 
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        public JsonResult GetPoJvNumber(int Po,bool Sales)
        {
            if (Sales == false)
            {
                return Json(FabulousErp.Business.GetPotingNumber(db.Inv_receive_po.Include(x => x.Payable).FirstOrDefault(x => x.Id == Po).Payable.Journal_number));
            }
            else
            {
                return Json(FabulousErp.Business.GetPotingNumber(db.Inv_sales_invoice.Include(x => x.Receivable).FirstOrDefault(x => x.Id == Po).Receivable.Journal_number));
            }
        }
        public JsonResult GetPoJn(int Po, bool Sales)
        {
            if (Sales == false)
            {
                return Json(db.Inv_receive_po.Include(x => x.Payable).FirstOrDefault(x => x.Id == Po).Payable.Journal_number);
            }
            else
            {
                return Json(db.Inv_sales_invoice.Include(x => x.Receivable).FirstOrDefault(x => x.Id == Po).Receivable.Journal_number);
            }
        }
        public JsonResult GetVendoreInvoice(int VendoreId, bool IsInstallment = false)
        {
            IEnumerable<Inv_receive_po> Invs = db.Inv_receive_po.Include(x => x.Payable)
                .Include(x => x.Payable.Trans_doc_type)
                .Include(x => x.Items.Select(z => z.Item_serial))
                .Include(x => x.Purchase_Installment_contract_invoice)
               .Where(x => x.Payable.Vendor_id == VendoreId&&x.Payable_id!=null);
            if (!IsInstallment)
            {
                return Json(Invs.Select(x => new { x.Id, Trx = x.Payable.Trans_doc_type.Trx_num }).ToList());
            }
            else
            {
                return Json(Invs
                    .Where(x => !x.Purchase_Installment_contract_invoice.Any(z => z.Invoice_id == x.Id))
                    .Select(x => new { x.Id, Trx = x.Payable.Trans_doc_type.Trx_num }).ToList());
            }

            //List<InvoiceItems> AvItem = InvBus.GetItemAvaliableByCustomer(CustomerId);
            //AvItem.ForEach(x => x.Sales_items = null);
            //return Json(AvItem);

        }
        public static dynamic GetVendoreSelect()
        {
            return MyDbContext.Instance.Inv_receive_po.Include(x => x.Payable)
               .Include(x => x.Payable.Trans_doc_type)
               .Include(x => x.Items.Select(z => z.Item_serial))
               .Include(x => x.Purchase_Installment_contract_invoice)
               .Where(x=>x.Payable_id!=null)
              .Select(x=>new 
              {
                  Id=x.Id,
                  Trx = x.Payable.Trans_doc_type.Trx_num
              }).ToList();
        }
    }
}
