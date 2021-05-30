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

namespace FixedAssets.Controllers
{
    public class Fixed_assets_renewalController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext Mdb = new DBContext();

        // GET: Fixed_assets_renewal
        public ActionResult Index()
        {
            List<Fixed_assets_renewal> fixed_assets_renewal = db.Fixed_assets_renewal.Include(f => f.Asset).ToList();
            fixed_assets_renewal.ForEach(x => x.Transaction_date =
           Convert.ToDateTime(Mdb.C_GeneralJournalEntry_Tables.Where(z => z.C_JournalEntryNumber == x.Gl_transaction_id).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { C_TransactionDate = null }).FirstOrDefault().C_TransactionDate));

            return View(fixed_assets_renewal.ToList());
        }

        // GET: Fixed_assets_renewal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Id = id;
            Fixed_assets_renewal fixed_assets_renewal = db.Fixed_assets_renewal.Find(id);
            if (fixed_assets_renewal == null)
            {
                return HttpNotFound();
            }
            return View(fixed_assets_renewal);
        }

        // GET: Fixed_assets_renewal/Create
        public ActionResult Create(int? Id=null,bool Edit=false,bool Void=false,bool IsPartial = false)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.IsPartial = IsPartial;
            #region detectJEPer

            if (string.IsNullOrEmpty(companyID))
            {
                return Redirect("/");
            }
            using (FabulousDB.DB_Context.DBContext DB = new FabulousDB.DB_Context.DBContext())
            {
                var detectJEPer =FabulousErp.Business.GetPostingSetup();//  Business.GetPostingSetup();
                //check Journal entry Per Transaction or Batch
                if (detectJEPer != null)
                {
                    ViewBag.FJEPer = "B1";//detectJEPer.CreateJEPer;
                    ViewBag.EPD = detectJEPer.EditPostingDate;
                    if (detectJEPer.CreateJEPer == "B2")
                    {
                        //ViewBag.BatchAction = detectJEPer.ExistingBatch;
                        ViewBag.PostDateType = detectJEPer.PostingDataFrom;
                    }
                }
                else
                {
                    ViewBag.FJEPer = "NoPS";
                }
                ViewBag.PostingToOrThrow = Business.Business.PostingToOrThrow();
            }
            ViewBag.FromTCGE = false;
            #endregion

            if (Edit)
            {
                ViewBag.Action = "Edit";
            }
            else
            {
                ViewBag.Action = "Create";

            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Assets_number");
            if (Id == null)
            {
                ViewBag.Currency_id = new SelectList(Mdb.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList(), "CurrencyID", "ISOCode");

                return View(new Fixed_assets_renewal {});
            }
            else
            {
                Fixed_assets_renewal fixed_assets_renewal = db.Fixed_assets_renewal.Find(Id);
                ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Assets_number", fixed_assets_renewal.Assets_id);
                ViewBag.Currency_id = new SelectList(Mdb.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList(), "CurrencyID", "ISOCode", fixed_assets_renewal.Currency_id);
                return View(fixed_assets_renewal);
            }
        }

        // POST: Fixed_assets_renewal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fixed_assets_renewal fixed_assets_renewal)
        {
            if (ModelState.IsValid)
            {
                db.Fixed_assets_renewal.Add(fixed_assets_renewal);
                db.SaveChanges();
                return Json(fixed_assets_renewal.Id);
            }
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Currency_id = new SelectList(Mdb.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList(), "CurrencyID", "ISOCode");

            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Assets_number", fixed_assets_renewal.Assets_id);
            return Json(fixed_assets_renewal.Id);
        }
        // GET: Fixed_assets_renewal/Edit/5
        public ActionResult Edit(int? id)
        {
            return RedirectToAction("Create", new { Id = id });
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixed_assets_renewal fixed_assets_renewal = db.Fixed_assets_renewal.Find(id);
            if (fixed_assets_renewal == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Assets_number", fixed_assets_renewal.Assets_id);
            return View(fixed_assets_renewal);
        }

        // POST: Fixed_assets_renewal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fixed_assets_renewal fixed_assets_renewal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fixed_assets_renewal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Assets_number", fixed_assets_renewal.Assets_id);
            return View(fixed_assets_renewal);
        }

        // GET: Fixed_assets_renewal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixed_assets_renewal fixed_assets_renewal = db.Fixed_assets_renewal.Find(id);
            if (fixed_assets_renewal == null)
            {
                return HttpNotFound();
            }
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.PostingToOrThrow=Business.Business.PostingToOrThrow();
            ViewBag.PostingNum = Mdb.C_GeneralJournalEntry_Tables.Where(x=>x.C_JournalEntryNumber== fixed_assets_renewal.Gl_transaction_id).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table {C_PostingNumber=0 })
                .FirstOrDefault().C_PostingNumber;

            ViewBag.TransactionDate = Mdb.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == fixed_assets_renewal.Gl_transaction_id).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { C_PostingNumber = 0 })
              .FirstOrDefault().C_TransactionDate;

            ViewBag.Currency = fixed_assets_renewal.Currency_id;

            return View(fixed_assets_renewal);
        }

        // POST: Fixed_assets_renewal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fixed_assets_renewal fixed_assets_renewal = db.Fixed_assets_renewal.Find(id);
            int PostingNum = Mdb.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == fixed_assets_renewal.Gl_transaction_id).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { C_PostingNumber = 0 })
    .FirstOrDefault().C_PostingNumber;
            db.Fixed_assets_renewal.Remove(fixed_assets_renewal);
            //if (fixed_assets_renewal.Gl_transaction_id.HasValue)
            //{
            //    string companyID = (string)FabulousErp.Business.GetCompanyId();
            //    VoidThisTransaction(fixed_assets_renewal.Gl_transaction_id.Value, companyID);
            //}

            db.Deleted_fixed_assets_renewal.Add(Business.Business.GetDeleteRenwal(fixed_assets_renewal));;
            db.SaveChanges();
            return Json(PostingNum);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
