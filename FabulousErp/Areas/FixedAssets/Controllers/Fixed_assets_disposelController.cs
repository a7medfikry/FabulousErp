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
    public class Fixed_assets_disposelController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext Mdb = new DBContext();

        // GET: Fixed_assets_disposel
        public ActionResult Index()
        {
            List<Fixed_assets_disposel> fixed_assets_disposel =
                db.Fixed_assets_disposel.Include(f => f.Asset).ToList();

            fixed_assets_disposel.ForEach(x => x.Transaction_date =
           Convert.ToDateTime(Mdb.C_GeneralJournalEntry_Tables.Where(z => z.C_JournalEntryNumber == x.Gl_transaction_id).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { C_TransactionDate = null }).FirstOrDefault().C_TransactionDate));

            return View(fixed_assets_disposel.ToList());
        }

        // GET: Fixed_assets_disposel/Details/5 
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixed_assets_disposel fixed_assets_disposel = db.Fixed_assets_disposel.Find(id);
            if (fixed_assets_disposel == null)
            {
                return HttpNotFound();
            }
            return View(fixed_assets_disposel);
        }

        // GET: Fixed_assets_disposel/Create
        public ActionResult Create()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            #region detectJEPer

            if (string.IsNullOrEmpty(companyID))
            {
                return Redirect("/");
            }
            using (FabulousDB.DB_Context.DBContext DB = new FabulousDB.DB_Context.DBContext())
            {
                var detectJEPer =FabulousErp.Business.GetPostingSetup();// Business.GetPostingSetup();
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


            ViewBag.Currency_id = new SelectList(Mdb.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList(), "CurrencyID", "ISOCode");

            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description");
            return View();
        }

        // POST: Fixed_assets_disposel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fixed_assets_disposel fixed_assets_disposel)
        {
            if (ModelState.IsValid)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                fixed_assets_disposel.Disposal_no = db.Fixed_assets_disposel.Max(x => x.Disposal_no) +1;
                fixed_assets_disposel.Company_id = companyID;
                db.Fixed_assets_disposel.Add(fixed_assets_disposel);
                db.SaveChanges();
                return Json(fixed_assets_disposel.Id);
            }

            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description", fixed_assets_disposel.Assets_id);
            return View(fixed_assets_disposel);
        }

        // GET: Fixed_assets_disposel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixed_assets_disposel fixed_assets_disposel = db.Fixed_assets_disposel.Find(id);
            if (fixed_assets_disposel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description", fixed_assets_disposel.Assets_id);
            return View(fixed_assets_disposel);
        }

        // POST: Fixed_assets_disposel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fixed_assets_disposel fixed_assets_disposel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fixed_assets_disposel).State = EntityState.Modified;
                db.Entry(fixed_assets_disposel).Property(x => x.Disposal_no).IsModified = false;
                db.Entry(fixed_assets_disposel).Property(x => x.Company_id).IsModified = false;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description", fixed_assets_disposel.Assets_id);
            return View(fixed_assets_disposel);
        }

        // GET: Fixed_assets_disposel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixed_assets_disposel fixed_assets_disposel = db.Fixed_assets_disposel.Find(id);
            if (fixed_assets_disposel == null)
            {
                return HttpNotFound();
            }
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.PostingToOrThrow = Business.Business.PostingToOrThrow();
            ViewBag.PostingNum = Mdb.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == fixed_assets_disposel.Gl_transaction_id).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { C_PostingNumber = 0 })
                .FirstOrDefault().C_PostingNumber;

            ViewBag.TransactionDate = Mdb.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == fixed_assets_disposel.Gl_transaction_id).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { C_TransactionDate = DateTime.Now.ToShortDateString().ToString() })
                .FirstOrDefault().C_TransactionDate;

            ViewBag.Currency = fixed_assets_disposel.Currency_id;
            return View(fixed_assets_disposel);
        }

        // POST: Fixed_assets_disposel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fixed_assets_disposel fixed_assets_disposel = db.Fixed_assets_disposel.Find(id);
            int PostingNum = Business.Business.GetPotinNumber(fixed_assets_disposel.Gl_transaction_id);
            db.Fixed_assets_disposel.Remove(fixed_assets_disposel);
            if (fixed_assets_disposel.Gl_transaction_id.HasValue)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();
            }
            db.Delete_fixed_assets_disposel.Add(Business.Business.GetDeleteDisposal(fixed_assets_disposel));
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
