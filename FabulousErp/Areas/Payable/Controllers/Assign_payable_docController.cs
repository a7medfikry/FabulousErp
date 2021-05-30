using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousErp.Payable.Models; using FabulousDB.DB_Context;
using FabulousDB.Models;

namespace Payable.Controllers
{
    public class Assign_payable_docController : Controller
    {
        private DBContext db = new DBContext();
        private FabulousDB.DB_Context.DBContext dbM = new FabulousDB.DB_Context.DBContext();

        // GET: Payable/Assign_payable_doc
        public ActionResult Index()
        {

            if (Request["section"] == "Inquiry")
            {
                var assign_payable_docs = db.Assign_payable_docs.Where(x => x.Is_void == false)
                    .Include(a => a.Trans_doc_type).Include(a => a.Vendor)
                    .ToList().Where(x => x.Applay_date.ToShortDateString() == DateTime.Now.ToShortDateString());

                return View(assign_payable_docs.ToList());

            }
            else
            {
                var assign_payable_docs = db.Assign_payable_docs.Where(x=>x.Is_void==false).Include(a => a.Trans_doc_type).Include(a => a.Vendor);
                return View(assign_payable_docs.ToList());

            }
        }

        public ActionResult IndexPartial(string section, int SortBy, DateTime? From = null, DateTime? To = null)
        {
            List<Assign_payable_doc> Res = db.Assign_payable_docs
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
        // GET: Payable/Assign_payable_doc/Details/5
        public ActionResult Details(int? id)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.companyID = companyID;

            ViewBag.id = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assign_payable_doc assign_payable_doc = db.Assign_payable_docs.Find(id);
            if (assign_payable_doc == null)
            {
                return HttpNotFound();
            }
            return View(assign_payable_doc);
        }

        // GET: Payable/Assign_payable_doc/Create
        public ActionResult Create()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.companyID = companyID;
            //ViewBag.Currency_id = new SelectList(Mdb.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Trans_doc_type_id = new SelectList(Enumerable.Empty<Payable_transactions_types>().ToList());//new SelectList(db.Payable_transactions_types, "Id", "Trx_num");
            ViewBag.Vendor_id = Business.GetPayableVendoreSelect();

            if (companyID == null)
            {
                return Redirect("/");
            }
            ViewBag.CompCurr = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).FirstOrDefault().CurrencyID;

            ViewBag.Doc_type = new SelectList(
                db.General_settings.Where(x => x.Doc_type == Doc_type.Payment ||
                x.Doc_type == Doc_type.Credit_Memo || x.Doc_type == Doc_type.Return)
                , "Doc_type", "Doc_type");

            return View(new Assign_payable_doc { Applay_date = DateTime.Now });
        }
        public JsonResult GetTransaction(Doc_type doc_type)
        {
            try
            {
                return Json(db.General_settings.Where(x => x.Doc_type == doc_type)
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
           
            List<AssignSel> UnPaidInvoice = Payable.Controllers.Business.GetUnAssignPayment(VendorId, doc_type)
                .Select(x => new AssignSel
                {
                    Id = x.Trans_doc_type.Id,
                    Trx_num = x.Trans_doc_type.Trx_num,
                    Counter = x.Trans_doc_type.Counter,
                    UnAssigned = Math.Round(db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc { Unassign_amount = -1 }).OrderByDescending(z => z.Id).ToList().LastOrDefault().Unassign_amount, 2),
                    Assigned = db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc { Applay_assign = -1 }).OrderByDescending(z => z.Id).ToList().LastOrDefault().Applay_assign,
                    AssignId = db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc { Id = -1 }).FirstOrDefault().Id,
                    Doc_type_id=x.Trans_doc_type_id
                }).ToList();
            List<int> Remove = new List<int>();
            foreach (AssignSel i in UnPaidInvoice)
            {
                TransactionPay ThisTrx= GetTransactionFunc(i.Doc_type_id);
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
        public TransactionPay GetTransactionFunc(int TransTypeId)
        {
            List<TransactionPay> PT = Enumerable.Empty<TransactionPay>().ToList();
            string Currency_id = "";
            string Iso = "";
            string Doc_Num = "";
            decimal Rate = 1;
            decimal OA = 1;
            decimal AssignAmount = 0;
            Payable_payment PP = db.Payable_payments.Where(x => x.Trans_doc_type_id == TransTypeId).ToList().DefaultIfEmpty(new Payable_payment { Orginal_amount = 0, Taken_discount = 0, Transaction_rate = 1, Transaction_id = 0 }).FirstOrDefault();

            PT.AddRange(db.Payable_transactions.Where(x => x.Id == PP.Transaction_id)
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

            if (db.Payable_transactions.Any(x => x.Trans_doc_type_id == TransTypeId))
            {

                PT.AddRange(db.Assign_payable_docs.Where(x => x.Trans_doc_type_id == TransTypeId)
                    .Include(x => x.Trans_doc_type)
                    .Include(x => x.Currency)
                    .ToList().Select(x => new TransactionPay
                    {
                        Doc_Num = x.Trans_doc_type.Payable_transaction.FirstOrDefault().VDocument_number,
                        OA = 0,
                        ISOCode = x.Currency.ISOCode,
                        AA = x.Applay_assign + db.Payable_transactions.Where(z => z.Id == PP.Transaction_id).ToList().DefaultIfEmpty(new Payable_transaction { Taken_discount = 0, Tax = 0, Transaction_rate = 1, Purchase = 0 }).Sum(z => (z.Purchase - z.Taken_discount + z.Tax) * z.Transaction_rate),//db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc { Applay_assign = 0 }).Sum(z => z.Applay_assign),
                        Rate = x.Trans_doc_type.Payable_transaction.FirstOrDefault().Transaction_rate,
                        Currency_id = x.Currency_id,
                        AssignId = x.Id //db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc { Id = 0 }).FirstOrDefault().Id
                    }).ToList());
                Currency_id = db.Payable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Currency_id;
                Iso = db.Payable_transactions.Include(x => x.Currency).FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Currency.ISOCode;
                Rate = db.Payable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Transaction_rate;
                Doc_Num = db.Payable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).VDocument_number;

                Payable_transaction MPT = db.Payable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId);
                if (MPT.Doc_type == Doc_type.Return
                    || MPT.Doc_type == Doc_type.Credit_Memo)
                {
                    OA = -(MPT.Purchase - MPT.Taken_discount + MPT.Tax);
                }
                else
                {
                    OA = (MPT.Purchase - MPT.Taken_discount + MPT.Tax);

                }
                int Transaction_id = db.Payable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Id;
                AssignAmount = db.Payable_payments.Where(x => x.Transaction_id == Transaction_id).ToList().DefaultIfEmpty(new Payable_payment { Orginal_amount = 0 }).Sum(x => x.Orginal_amount);
            }
            else
            {
                PT.AddRange(db.Assign_payable_docs.Where(x => x.Trans_doc_type_id == TransTypeId)
                    .Include(x => x.Trans_doc_type)
                    .Include(x => x.Currency)
                   .ToList().Select(x => new TransactionPay
                   {
                       Doc_Num = "",
                       OA = (x.Trans_doc_type.Payable_payment.FirstOrDefault().Orginal_amount - x.Trans_doc_type.Payable_payment.FirstOrDefault().Taken_discount),
                       ISOCode = x.Currency.ISOCode,
                       AA = x.Applay_assign + db.Payable_transactions.Where(z => z.Id == PP.Transaction_id).ToList().DefaultIfEmpty(new Payable_transaction { Taken_discount = 0, Tax = 0, Transaction_rate = 1, Purchase = 0 }).Sum(z => (z.Purchase - z.Taken_discount + z.Tax) * z.Transaction_rate),//db.Assign_payable_docs.Where(z=>z.Trans_doc_type_id==x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc {Applay_assign=0 }).Sum(z=>z.Applay_assign),
                       Rate = x.Trans_doc_type.Payable_payment.FirstOrDefault().Transaction_rate,
                       Currency_id = x.Currency_id,
                       AssignId = x.Id//db.Assign_payable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc {Id=0 }).FirstOrDefault().Id
                   }).ToList());
                Currency_id = db.Payable_payments.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Currency_id;
                Iso = db.Payable_payments.Include(x => x.Currency).FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Currency.ISOCode;
                Rate = db.Payable_payments.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId).Transaction_rate;

                Payable_payment MPT = db.Payable_payments.FirstOrDefault(x => x.Trans_doc_type_id == TransTypeId);

                OA = (MPT.Orginal_amount - MPT.Taken_discount);

            }

            foreach (TransactionPay i in PT)
            {
                decimal TransactionRate, TransactionRateTo;
                string TrxCurr, TrxToCurr;
                Assign_payable_doc ThisI = db.Assign_payable_docs.Find(i.AssignId);
                if (ThisI != null)
                {
                    CalcRate(db.Assign_payable_docs.Find(i.AssignId), out TransactionRate, out TransactionRateTo, out TrxCurr, out TrxToCurr);
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
            //List<Payable_transaction> Res = db.Payable_transactions.Include(x => x.Trans_doc_type)
            //     .Include(x => x.Currency)
            //     .Where(x =>
            //   x.Is_void==false&&
            //   x.Vendor_id == VendoreId &&
            //   (x.Doc_type == Doc_type.Invoice || x.Doc_type == Doc_type.Debit_Memo)).ToList()
            //   .Where(x =>
            //   (x.Purchase - x.Taken_discount + x.Tax -
            //     db.Assign_payable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id)
            //   .ToList().DefaultIfEmpty(new Assign_payable_doc { Taken_discount = 0 })
            //   .Sum(y => y.Taken_discount)) >
            //   db.Assign_payable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id)
            //   .ToList().DefaultIfEmpty(new Assign_payable_doc { Applay_assign = 0 })
            //   .Sum(y => y.Applay_assign)).ToList()
            //   .Select(x => 
            //   {
            //       x.Purchase = x.Purchase - db.Assign_payable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id)
            //   .ToList().DefaultIfEmpty(new Assign_payable_doc { Taken_discount = 0 }).FirstOrDefault().Taken_discount;
            //       return x;
            //   }).ToList();


            List<Payable_transaction> Res = Business.GetUnpaidTransaction(VendoreId).Where(x => (x.Doc_type == Doc_type.Invoice || x.Doc_type == Doc_type.Debit_Memo))
                .ToList();

            Res.RemoveAll(x => x.Purchase - x.Taken_discount + x.Tax == 0);
            List<Payable_transaction> Remove = new List<Payable_transaction>();
            decimal Assigned = 0;
            foreach (Payable_transaction i in Res)
            {
                Assigned = db.Assign_payable_docs.Where(x => x.Trans_doc_type_id_to == i.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_payable_doc { Applay_assign = 0 }).Sum(x => x.Applay_assign);
                if (Assigned>= i.Purchase - i.Taken_discount + i.Tax)
                {
                    Remove.Add(i);
                }

            }
            Res.RemoveAll(x => Remove.Any(z => z.Id == x.Id));
            return PartialView(Res);
        }
        public PartialViewResult BelowTableRes(int VendoreId, int To,bool IsEdit=false)
        {
            List<Payable_transaction> Res = db.Payable_transactions.Include(x => x.Trans_doc_type)
                 .Include(x => x.Currency)
                 .Where(x =>
                  x.Is_void == false &&
               x.Vendor_id == VendoreId &&
               (x.Doc_type == Doc_type.Invoice || x.Doc_type == Doc_type.Debit_Memo)).ToList()
               .Where(x => db.Assign_payable_docs.Any(z => z.Trans_doc_type_id_to == To)).ToList()
               .Select(x =>
               {
                   x.Purchase = x.Purchase - db.Assign_payable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id)
               .ToList().DefaultIfEmpty(new Assign_payable_doc { Taken_discount = 0 }).FirstOrDefault().Taken_discount;
                   x.Applay_asign_taken_discount = db.Assign_payable_docs.Where(z => z.Trans_doc_type_id_to == To).Sum(z => z.Taken_discount);
                   return x;
               }).ToList();
            if (IsEdit)
            {
                Res.Where(x => x.Trans_doc_type_id == To).ToList().ForEach(x => x.Purchase = Res.Where(z => z.Trans_doc_type_id == To).Sum(z => z.Purchase));
                Res.RemoveAll(x => x.Trans_doc_type_id != To);
            }

            return PartialView("BelowTable", Res);
        }
        // POST: Payable/Assign_payable_doc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(List<Assign_payable_doc> assign_payable_doc)
        {
            try
            {
                assign_payable_doc.ForEach(x => x.Creation_date = DateTime.Now);
                int AssignNum = db.Assign_payable_docs.ToList().DefaultIfEmpty(new Assign_payable_doc { Assign_no = 0 }).Max(x => x.Assign_no) + 1;
                assign_payable_doc.ForEach(x => x.Assign_no = AssignNum);
                //foreach (Assign_payable_doc i in assign_payable_doc)
                //{
                //    //decimal TransactionRate, TransactionRateTo;
                //    //string TrxCurr, TrxToCurr;
                //    //CalcRate(i, out TransactionRate, out TransactionRateTo, out TrxCurr, out TrxToCurr);
                //    //if (TrxCurr != TrxToCurr)
                //    //{
                //    //    i.Applay_assign = (i.Applay_assign * TransactionRate / TransactionRateTo);
                //    //}
                //}

                db.Assign_payable_docs.AddRange(assign_payable_doc);
                db.SaveChanges();
                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }

        }

        private void CalcRate(Assign_payable_doc i, out decimal TransactionRate, out decimal TransactionRateTo, out string TrxCurr, out string TrxToCurr)
        {
            TransactionRate = 1;
            TransactionRateTo = 1;
            Payable_transactions_types TrxTo = db.Payable_transactions_types.Include(x => x.Payable_payment).Include(x => x.Payable_transaction)
.FirstOrDefault(x => x.Id == i.Trans_doc_type_id_to);
            Payable_transactions_types Trx = db.Payable_transactions_types.Include(x => x.Payable_transaction).Include(x => x.Payable_payment)
                .FirstOrDefault(x => x.Id == i.Trans_doc_type_id);
            TrxCurr = "";
            TrxToCurr = "";
            if (Trx != null)
            {
                if (Trx.Payable_payment.Any())
                {
                    TrxCurr = Trx.Payable_payment.FirstOrDefault().Currency_id;
                    TransactionRate = Trx.Payable_payment.FirstOrDefault().Transaction_rate;
                }
                else if (Trx.Payable_transaction.Any())
                {
                    TrxCurr = Trx.Payable_transaction.FirstOrDefault().Currency_id;
                    TransactionRate = Trx.Payable_transaction.FirstOrDefault().Transaction_rate;
                }
            }
            if (TrxTo != null)
            {
                if (TrxTo.Payable_payment.Any())
                {
                    TrxToCurr = TrxTo.Payable_payment.FirstOrDefault().Currency_id;
                    TransactionRateTo = TrxTo.Payable_payment.FirstOrDefault().Transaction_rate;
                }
                else if (TrxTo.Payable_transaction.Any())
                {
                    TrxToCurr = TrxTo.Payable_transaction.FirstOrDefault().Currency_id;
                    TransactionRateTo = TrxTo.Payable_transaction.FirstOrDefault().Transaction_rate;
                }
            }
        }

        // GET: Payable/Assign_payable_doc/Edit/5
        public ActionResult Edit(int? id, bool Partial = false)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.companyID = companyID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assign_payable_doc assign_payable_doc = db.Assign_payable_docs.Include(x=>x.Trans_doc_type).FirstOrDefault(x=>x.Id==id);
            if (assign_payable_doc == null)
            {
                return HttpNotFound();
            }
            ViewBag.Partial = Partial;
            ViewBag.DocType = (int)db.General_settings.FirstOrDefault(x=>x.Doc_type== assign_payable_doc.Trans_doc_type.Doc_type).Id;
            ViewBag.Doc_type = new SelectList(db.General_settings.Where(x => x.Doc_type == Doc_type.Payment || x.Doc_type == Doc_type.Credit_Memo).ToList(), "Id", "Doc_type", ViewBag.DocType);
            ViewBag.Trans_doc_type_id = new SelectList(db.Payable_transactions_types, "Id", "Trx_num", assign_payable_doc.Trans_doc_type_id);
            ViewBag.Vendor_id = new SelectList(db.Payable_creditor_setting.Where(x => x.Inactive == false), "Id", "Vendor_id", assign_payable_doc.Vendor_id);
            return View(assign_payable_doc);
        }

        // POST: Payable/Assign_payable_doc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Assign_payable_doc assign_payable_doc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assign_payable_doc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Trans_doc_type_id = new SelectList(db.Payable_transactions_types, "Id", "Trx_num", assign_payable_doc.Trans_doc_type_id);
            ViewBag.Vendor_id = new SelectList(db.Payable_creditor_setting.Where(x => x.Inactive == false), "Id", "Vendor_id", assign_payable_doc.Vendor_id);
            return View(assign_payable_doc);
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
                Payable_transactions_types PT = db.Payable_transactions_types.Include(x => x.Payable_transaction).
                    Include(x => x.Payable_payment).FirstOrDefault(x => x.Id == Id);
                if (PT.Payable_transaction != null)
                {
                    OrginalAmount = PT.Payable_transaction.FirstOrDefault().Purchase - PT.Payable_transaction.FirstOrDefault().Taken_discount + PT.Payable_transaction.FirstOrDefault().Tax;
                }
                else if (PT.Payable_payment != null)
                {
                    OrginalAmount = PT.Payable_payment.FirstOrDefault().Orginal_amount - PT.Payable_payment.FirstOrDefault().Taken_discount;
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
    public class DPayable_transaction: Payable_transaction
    {
        public decimal Asign_taken_discount { get; set; }
    }
}
