using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousErp.Receivable.Models; using FabulousDB.DB_Context;
using FabulousDB.Models;

namespace Receivable.Controllers
{
    public class Assign_Receivable_docController : Controller
    {
        private DBContext db = new DBContext();
        private FabulousDB.DB_Context.DBContext dbM = new FabulousDB.DB_Context.DBContext();

        // GET: Receivable/Assign_Receivable_doc
        public ActionResult Index()
        {

            if (Request["section"] == "Inquiry")
            {
                var assign_Receivable_docs = db.Assign_Receivable_docs
                    .Include(a => a.Trans_doc_type).Include(a => a.Vendor)
                    .ToList().Where(x => x.Applay_date.ToShortDateString() == DateTime.Now.ToShortDateString());

                return View(assign_Receivable_docs.ToList());

            }
            else
            {
                var assign_Receivable_docs = db.Assign_Receivable_docs.Include(a => a.Trans_doc_type).Include(a => a.Vendor);
                return View(assign_Receivable_docs.ToList());

            }
        }

        public ActionResult IndexPartial(string section,int SortBy, DateTime? From = null, DateTime? To = null)
        {
            List<Assign_Receivable_doc> Res = db.Assign_Receivable_docs
              .Include(a => a.Trans_doc_type).Include(a => a.Vendor).ToList();

            ViewBag.section = section;
            if (section != "void")
            {
                if ((From == null || To == null) && SortBy != 1)
                {
                    SortBy = 4;
                }
                if (SortBy == 1)
                {
                    Res = Res.Where(x => x.Creation_date.ToShortDateString() == DateTime.Now.ToShortDateString()).ToList();
                }
                else if (SortBy == 2)
                {
                    Res = Res.Where(x => Convert.ToDateTime(x.Creation_date.ToShortDateString()) >=
                    Convert.ToDateTime(From.Value.ToShortDateString()) &&
                    Convert.ToDateTime(x.Creation_date.ToShortDateString()) <=
                    Convert.ToDateTime(To.Value.ToShortDateString())).ToList();
                }
                else if (SortBy == 3)
                {
                    Res = Res.Where(x => Convert.ToDateTime(x.Applay_date.ToShortDateString()) >=
                   Convert.ToDateTime(From.Value.ToShortDateString()) &&
                   Convert.ToDateTime(x.Applay_date.ToShortDateString()) <=
                   Convert.ToDateTime(To.Value.ToShortDateString())).ToList();
                }
            }
            return View(Res);

        }
        // GET: Receivable/Assign_Receivable_doc/Details/5
        public ActionResult Details(int? id)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.companyID = companyID;

            ViewBag.id = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assign_Receivable_doc assign_Receivable_doc = db.Assign_Receivable_docs.Find(id);
            if (assign_Receivable_doc == null)
            {
                return HttpNotFound();
            }
            return View(assign_Receivable_doc);
        }

        // GET: Receivable/Assign_Receivable_doc/Create
        public ActionResult Create()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.companyID = companyID;
            //ViewBag.Currency_id = new SelectList(Mdb.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Trans_doc_type_id = new SelectList(Enumerable.Empty<Receivable_transactions_types>().ToList());//new SelectList(db.Receivable_transactions_types, "Id", "Trx_num");
            ViewBag.Vendor_id = Business.GetCustomerReceivableSelect();

            if (companyID == null)
            {
                return Redirect("/");
            }
            ViewBag.CompCurr = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).FirstOrDefault().CurrencyID;

            ViewBag.Doc_type = new SelectList(
                db.Receivable_general_settings.Where(x => x.Doc_type == Doc_type.Payment ||
                x.Doc_type == Doc_type.Credit_Memo|| x.Doc_type == Doc_type.Return)
                , "Id", "Doc_type");

            return View(new Assign_Receivable_doc { Applay_date = DateTime.Now });
        }
        public JsonResult GetTransaction(Doc_type doc_type)
        {
            try
            {
                return Json(db.Receivable_general_settings.Where(x => x.Doc_type == doc_type)
                    .Select(x => new
                    {
                        Name = x.Doc_type,
                        Id = x.Id
                    }));
            }
            catch
            {
                return Json(null);
            }
        }
        public JsonResult GetVendorIncoice(int VendorId, Doc_type doc_type)
        {

            List<AssignSel> UnPaidInvoice = Receivable.Controllers.Business.GetUnAssignPayment(VendorId, doc_type)
                .Select(x => new AssignSel
                {
                    Id = x.Trans_doc_type.Id,
                    Trx_num = x.Trans_doc_type.Trx_num,
                    Counter= x.Trans_doc_type.Counter,
                    UnAssigned = Math.Round(db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Unassign_amount = -1 }).OrderByDescending(z => z.Id).ToList().LastOrDefault().Unassign_amount, 2),
                    Assigned = db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = -1 }).OrderByDescending(z => z.Id).ToList().LastOrDefault().Applay_assign,
                    AssignId = db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Id = -1 }).FirstOrDefault().Id,
                    Doc_type_id = x.Trans_doc_type_id
                }).ToList();
            List<int> Remove = new List<int>();
            foreach (AssignSel i in UnPaidInvoice)
            {
                TransactionPay ThisTrx = GetTransactionFunc(i.Doc_type_id);
                if (ThisTrx.OA == ThisTrx.AA)
                {
                    Remove.Add(i.Trx_num);
                }
            }
            UnPaidInvoice.RemoveAll(x => Remove.Contains(x.Trx_num));
            return Json(UnPaidInvoice);
        }
        public JsonResult GetTransactionDetails(int TransTypeId)
        {
            return Json(GetTransactionFunc(TransTypeId));
        }

        private TransactionPay GetTransactionFunc(int TransTypeId)
        {
            List<TransactionPay> PT = Enumerable.Empty<TransactionPay>().ToList();
            string Currency_id = "";
            string Iso = "";
            string Doc_Num = "";
            decimal Rate = 1;
            decimal OA = 1;
            decimal AssignAmount = 0;
            Receivable_payment PP = db.Receivable_payments.Where(x => x.Trans_doc_type_id == TransTypeId).ToList().DefaultIfEmpty(new Receivable_payment { Orginal_amount = 0, Taken_discount = 0, Transaction_rate = 1, Transaction_id = 0 }).FirstOrDefault();

            PT.AddRange(db.Receivable_transactions.Where(x => x.Id == PP.Transaction_id)
                 .Include(x => x.Trans_doc_type)
                 .Include(x => x.Currency)
                .ToList().Select(x => new TransactionPay
                {
                    Doc_Num = "",
                    OA = 0,//(x.Trans_doc_type.Payable_payment.FirstOrDefault().Orginal_amount - x.Trans_doc_type.Payable_payment.FirstOrDefault().Taken_discount),
                    ISOCode = x.Currency.ISOCode,
                    AA = (x.Purchase - x.Taken_discount + x.Tax) /** x.Transaction_rate*/,//db.Assign_payable_docs.Where(z=>z.Trans_doc_type_id==x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc {Applay_assign=0 }).Sum(z=>z.Applay_assign),
                    Rate = x.Transaction_rate,// x.Trans_doc_type.Payable_payment.FirstOrDefault().Transaction_rate,
                    Currency_id = x.Currency_id,
                    AssignId = x.Id//db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc {Id=0 }).FirstOrDefault().Id
                }).ToList());

            if (db.Receivable_transactions.Any(x => x.Trans_doc_type_id == TransTypeId))
            {

                PT.AddRange(db.Assign_Receivable_docs.Where(x => x.Trans_doc_type_id == TransTypeId)
                    .Include(x => x.Trans_doc_type)
                    .Include(x => x.Currency)
                    .ToList().Select(x => new TransactionPay
                    {
                        Doc_Num = x.Trans_doc_type.Receivable_transaction.FirstOrDefault().VDocument_number,
                        OA = 0,
                        ISOCode = x.Currency.ISOCode,
                        AA = x.Applay_assign + db.Receivable_transactions.Where(z => z.Id == PP.Transaction_id).ToList().DefaultIfEmpty(new Receivable_transaction { Taken_discount = 0, Tax = 0, Transaction_rate = 1, Purchase = 0 }).Sum(z => (z.Purchase - z.Taken_discount + z.Tax) * z.Transaction_rate),//db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc { Applay_assign = 0 }).Sum(z => z.Applay_assign),
                        Rate = x.Trans_doc_type.Receivable_transaction.FirstOrDefault().Transaction_rate,
                        Currency_id = x.Currency_id,
                        AssignId = x.Id //db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc { Id = 0 }).FirstOrDefault().Id
                    }).ToList());
                Currency_id = db.Receivable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Currency_id;
                Iso = db.Receivable_transactions.Include(x => x.Currency).FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Currency.ISOCode;
                Rate = db.Receivable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Transaction_rate;
                Doc_Num = db.Receivable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).VDocument_number;

                Receivable_transaction MPT = db.Receivable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId);
                if (MPT.Doc_type == Doc_type.Return
                    || MPT.Doc_type == Doc_type.Credit_Memo)
                {
                    OA = -(MPT.Purchase - MPT.Taken_discount + MPT.Tax);
                }
                else
                {
                    OA = (MPT.Purchase - MPT.Taken_discount + MPT.Tax);

                }
                int Transaction_id = db.Receivable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Id;
                AssignAmount = db.Receivable_payments.Where(x => x.Transaction_id == Transaction_id).ToList().DefaultIfEmpty(new Receivable_payment { Orginal_amount = 0 }).Sum(x => x.Orginal_amount);
            }
            else
            {
                PT.AddRange(db.Assign_Receivable_docs.Where(x => x.Trans_doc_type_id == TransTypeId)
                    .Include(x => x.Trans_doc_type)
                    .Include(x => x.Currency)
                   .ToList().Select(x => new TransactionPay
                   {
                       Doc_Num = "",
                       OA = (x.Trans_doc_type.Receivable_payment.FirstOrDefault().Orginal_amount - x.Trans_doc_type.Receivable_payment.FirstOrDefault().Taken_discount),
                       ISOCode = x.Currency.ISOCode,
                       AA = x.Applay_assign + db.Receivable_transactions.Where(z => z.Id == PP.Transaction_id).ToList().DefaultIfEmpty(new Receivable_transaction { Taken_discount = 0, Tax = 0, Transaction_rate = 1, Purchase = 0 }).Sum(z => (z.Purchase - z.Taken_discount + z.Tax) * z.Transaction_rate),//db.Assign_payable_docs.Where(z=>z.Trans_doc_type_id==x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc {Applay_assign=0 }).Sum(z=>z.Applay_assign),
                       Rate = x.Trans_doc_type.Receivable_payment.FirstOrDefault().Transaction_rate,
                       Currency_id = x.Currency_id,
                       AssignId = x.Id//db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc {Id=0 }).FirstOrDefault().Id
                   }).ToList());
                Currency_id = db.Receivable_payments.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Currency_id;
                Iso = db.Receivable_payments.Include(x => x.Currency).FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Currency.ISOCode;
                Rate = db.Receivable_payments.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Transaction_rate;

                Receivable_payment MPT = db.Receivable_payments.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId);

                OA = (MPT.Orginal_amount - MPT.Taken_discount);

            }

            foreach (TransactionPay i in PT)
            {
                decimal TransactionRate, TransactionRateTo;
                string TrxCurr, TrxToCurr;
                Assign_Receivable_doc ThisI = db.Assign_Receivable_docs.Find(i.AssignId);
                if (ThisI != null)
                {
                    CalcRate(db.Assign_Receivable_docs.Find(i.AssignId), out TransactionRate, out TransactionRateTo, out TrxCurr, out TrxToCurr);
                }
                else
                {
                    TrxCurr = TrxToCurr = i.Currency_id;
                    TransactionRateTo = TransactionRate = 1;
                }
                //if (TrxCurr != TrxToCurr)
                //{
                //    i.AA = i.AA * TransactionRateTo / TransactionRate;
                //}
            }

            TransactionPay PTT = new TransactionPay
            {
                Doc_Num = Doc_Num,
                OA = OA,
                ISOCode = Iso,
                AA = PT.DefaultIfEmpty(new TransactionPay { AA = 0 }).Sum(x => x.AA) + AssignAmount,//db.Assign_payable_docs.Where(z=>z.Trans_doc_type_id==x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc {Applay_assign=0 }).Sum(z=>z.Applay_assign),
                Rate = Rate,
                Currency_id = Currency_id,
                AssignId = 0 //db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc {Id=0 }).FirstOrDefault().Id
            };
            return PTT;
        }

        public PartialViewResult BelowTable(int VendoreId)
        {
            //List<Receivable_transaction> Res = db.Receivable_transactions.Include(x => x.Trans_doc_type)
            //     .Include(x => x.Currency)
            //     .Where(x =>
            //   x.Is_void==false&&
            //   x.Vendor_id == VendoreId &&
            //   (x.Doc_type == Doc_type.Invoice || x.Doc_type == Doc_type.Debit_Memo)).ToList()
            //   .Where(x =>
            //   (x.Purchase - x.Taken_discount + x.Tax -
            //     db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id)
            //   .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Taken_discount = 0 })
            //   .Sum(y => y.Taken_discount)) >
            //   db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id)
            //   .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0 })
            //   .Sum(y => y.Applay_assign)).ToList()
            //   .Select(x =>
            //   {
            //       x.Purchase = x.Purchase - db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id)
            //   .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Taken_discount = 0 }).FirstOrDefault().Taken_discount;
            //       return x;
            //   }).ToList();


            List<Receivable_transaction> Res = Business.GetUnpaidTransaction(VendoreId).Where(x=>(x.Doc_type == Doc_type.Invoice || x.Doc_type == Doc_type.Debit_Memo))
                .ToList();
            Res.RemoveAll(x => x.Purchase - x.Taken_discount + x.Tax == 0);
            decimal Assigned = 0;
            List<Receivable_transaction> Remove = new List<Receivable_transaction>();

            foreach (Receivable_transaction i in Res)
            {
                Assigned = db.Assign_Receivable_docs.Where(x => x.Trans_doc_type_id_to == i.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0 }).Sum(x => x.Applay_assign);
                if (Assigned >= i.Purchase - i.Taken_discount + i.Tax)
                {
                    Remove.Add(i);
                }

            }
            Res.RemoveAll(x => Remove.Any(z => z.Id == x.Id));
            return PartialView(Res);
        }
        public PartialViewResult BelowTableRes(int VendoreId,int To, bool IsEdit = false)
        {
            List<Receivable_transaction> Res = db.Receivable_transactions.Include(x => x.Trans_doc_type)
                 .Include(x => x.Currency)
                 .Where(x =>
                  x.Is_void == false &&
               x.Vendor_id == VendoreId &&
               (x.Doc_type == Doc_type.Invoice || x.Doc_type == Doc_type.Debit_Memo)).ToList()
               .Where(x => db.Assign_Receivable_docs.Any(z => z.Trans_doc_type_id_to == To)).ToList()
               .Select(x =>
               {
                   x.Purchase = x.Purchase - db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id)
               .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Taken_discount = 0 }).FirstOrDefault().Taken_discount;
                   return x;
               }).ToList();
            if (IsEdit)
            {
                Res.Where(x => x.Trans_doc_type_id == To).ToList().ForEach(x => x.Purchase = Res.Where(z => z.Trans_doc_type_id == To).Sum(z => z.Purchase));
                Res.RemoveAll(x => x.Trans_doc_type_id != To);
            }

            return PartialView("BelowTable", Res);
        }
        // POST: Receivable/Assign_Receivable_doc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(List<Assign_Receivable_doc> assign_Receivable_doc)
        {
            try
            {
                assign_Receivable_doc.ForEach(x => x.Creation_date = DateTime.Now);
                int AssignNum = db.Assign_Receivable_docs.ToList().DefaultIfEmpty(new Assign_Receivable_doc { Assign_no = 0 }).Max(x => x.Assign_no) + 1;
                assign_Receivable_doc.ForEach(x => x.Assign_no = AssignNum);
                //foreach (Assign_Receivable_doc i in assign_Receivable_doc)
                //{
                //    decimal TransactionRate, TransactionRateTo;
                //    string TrxCurr, TrxToCurr;
                //    CalcRate(i, out TransactionRate, out TransactionRateTo, out TrxCurr, out TrxToCurr);
                //    if (TrxCurr != TrxToCurr)
                //    {
                //        i.Applay_assign = (i.Applay_assign * TransactionRate / TransactionRateTo);
                //        //i. = i.Applay_assign * TransactionRateTo / 1;
                //    }
                //}

                db.Assign_Receivable_docs.AddRange(assign_Receivable_doc);
                db.SaveChanges();
                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }

        }

        private void CalcRate(Assign_Receivable_doc i, out decimal TransactionRate, out decimal TransactionRateTo, out string TrxCurr, out string TrxToCurr)
        {
            TransactionRate = 1;
            TransactionRateTo = 1;
            Receivable_transactions_types TrxTo = db.Receivable_transactions_types.Include(x => x.Receivable_payment).Include(x => x.Receivable_transaction)
.FirstOrDefault(x => x.Id == i.Trans_doc_type_id_to);
            Receivable_transactions_types Trx = db.Receivable_transactions_types.Include(x => x.Receivable_transaction).Include(x => x.Receivable_payment)
                .FirstOrDefault(x => x.Id == i.Trans_doc_type_id);
            TrxCurr = "";
            TrxToCurr = "";
            if (Trx != null)
            {
                if (Trx.Receivable_payment.Any())
                {
                    TrxCurr = Trx.Receivable_payment.FirstOrDefault().Currency_id;
                    TransactionRate = Trx.Receivable_payment.FirstOrDefault().Transaction_rate;
                }
                else if (Trx.Receivable_transaction.Any())
                {
                    TrxCurr = Trx.Receivable_transaction.FirstOrDefault().Currency_id;
                    TransactionRate = Trx.Receivable_transaction.FirstOrDefault().Transaction_rate;
                }
            }
            if (TrxTo != null)
            {
                if (TrxTo.Receivable_payment.Any())
                {
                    TrxToCurr = TrxTo.Receivable_payment.FirstOrDefault().Currency_id;
                    TransactionRateTo = TrxTo.Receivable_payment.FirstOrDefault().Transaction_rate;
                }
                else if (TrxTo.Receivable_transaction.Any())
                {
                    TrxToCurr = TrxTo.Receivable_transaction.FirstOrDefault().Currency_id;
                    TransactionRateTo = TrxTo.Receivable_transaction.FirstOrDefault().Transaction_rate;
                }
            }
        }

        // GET: Receivable/Assign_Receivable_doc/Edit/5
        public ActionResult Edit(int? id,bool Partial=false)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.companyID = companyID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assign_Receivable_doc assign_Receivable_doc = db.Assign_Receivable_docs.Find(id);
            if (assign_Receivable_doc == null)
            {
                return HttpNotFound();
            }
            ViewBag.Partial = Partial;
            ViewBag.Doc_type = new SelectList(db.Receivable_general_settings.Where(x => x.Doc_type == Doc_type.Payment || x.Doc_type == Doc_type.Credit_Memo), "Id", "Doc_type");
            ViewBag.Trans_doc_type_id = new SelectList(db.Receivable_transactions_types, "Id", "Trx_num", assign_Receivable_doc.Trans_doc_type_id);
            ViewBag.Vendor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", assign_Receivable_doc.Vendor_id);
            return View(assign_Receivable_doc);
        }

        // POST: Receivable/Assign_Receivable_doc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Assign_Receivable_doc assign_Receivable_doc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assign_Receivable_doc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Trans_doc_type_id = new SelectList(db.Receivable_transactions_types, "Id", "Trx_num", assign_Receivable_doc.Trans_doc_type_id);
            ViewBag.Vendor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", assign_Receivable_doc.Vendor_id);
            return View(assign_Receivable_doc);
        }

      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetTransactionAmount(int Id)
        {
            try
            {
                decimal OrginalAmount = 0;
                Receivable_transactions_types PT = db.Receivable_transactions_types.Include(x => x.Receivable_transaction).
                    Include(x => x.Receivable_payment).FirstOrDefault(x => x.Id == Id);
                if (PT.Receivable_transaction != null)
                {
                    OrginalAmount = PT.Receivable_transaction.FirstOrDefault().Purchase - PT.Receivable_transaction.FirstOrDefault().Taken_discount + PT.Receivable_transaction.FirstOrDefault().Tax;
                }
                else if (PT.Receivable_payment != null)
                {
                    OrginalAmount = PT.Receivable_payment.FirstOrDefault().Orginal_amount - PT.Receivable_payment.FirstOrDefault().Taken_discount;
                }
                return Json(OrginalAmount);

            }
            catch
            {
                return Json(0);

            }
        }
    }
    public class AssignSel
    {
        public int Id { get; set; }
        public int Trx_num { get; set; }
        public int Counter { get; set; }
        public int Doc_type_id { get; set; }
        public decimal UnAssigned { get; set; }
        public decimal Assigned { get; set; }
        public int AssignId { get; set; }
        public string Currency_id { get; set; }
    }
    public class TransactionPay
    {
        public string Doc_Num { get; set; }
        public decimal OA { get; set; }
        public decimal AA { get; set; }
        public decimal Rate { get; set; }
        public string Currency_id { get; set; }
        public string ISOCode { get; set; }
        public int AssignId { get; set; }
    }
}
