using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Tabels;
using FabulousDB.DB_Context;
using FabulousDB.Models;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
namespace FixedAssets.Controllers
{
    public class Assets_accountsController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext FixedDb = new DBContext();
        // GET: Assets_accounts
        private IEnumerable<C_CreateAccount_Table> GetAccount()
        {
            return db.C_CreateAccount_Tables.Where(x => x.ReconcileAccount == true && x.ConsolidationAccount == false && (x.C_Prefix == Prefix.Asset.ToString() || x.C_Prefix == null) && string.IsNullOrEmpty(x.C_AnalyticAccountID) && string.IsNullOrEmpty(x.CostCenterType))
                .OrderBy(x => x.AccountID).ToList().Select(x =>
                {
                    x.AccountName = x.AccountID + " " + x.AccountName;
                    x.AccountID = "";

                    return x;
                });
        }
        private IEnumerable<C_CreateAccount_Table> GetAccount(Posting_type PostinType)
        {
            return db.C_CreateAccount_Tables
                .Where(x => x.PostingType == PostinType.ToString() && x.ReconcileAccount == true && x.ConsolidationAccount == false && (x.C_Prefix == Prefix.Asset.ToString() || x.C_Prefix == null) && string.IsNullOrEmpty(x.C_AnalyticAccountID) && string.IsNullOrEmpty(x.CostCenterType))
                .OrderBy(x => x.AccountID).ToList().Select(x =>
                {
                    x.AccountName = x.AccountID + " " + x.AccountName;
                    x.AccountID = "";

                    return x;

                });
        }
        private IEnumerable<C_CreateAccount_Table> GetAccount(Prefix Prefix)
        {
           return db.C_CreateAccount_Tables.Where(x => x.C_Prefix == Prefix.ToString()).OrderBy(x => x.AccountID).ToList().Select(x =>
           {
               x.AccountName = x.AccountID + " " + x.AccountName;
               x.AccountID = "";

               return x;
           });
        }
        private IEnumerable<C_CreateAccount_Table> GetAccount(Prefix Prefix, Posting_type PostinType)
        {
            return db.C_CreateAccount_Tables.Where(x => x.PostingType == PostinType.ToString() && x.C_Prefix == Prefix.ToString()).OrderBy(x => x.AccountID).ToList().Select(x =>
                {
                    x.AccountName = x.AccountID + " " + x.AccountName;
                    x.AccountID = "";

                    return x;

                });
        }
        private IEnumerable<C_CreateAccount_Table> GetAccountNoPrifix()
        {
            return db.C_CreateAccount_Tables.Where(x => x.C_Prefix == null && x.ConsolidationAccount == false && x.ReconcileAccount == false)
                .OrderBy(x => x.AccountID).ToList().Select(x =>
                {
                    x.AccountName = x.AccountID + " " + x.AccountName;
                    x.AccountID = "";
                    return x;
                });
        }
        public ActionResult Index()
        {
            var assets_accounts = GetAccount();
            return View(assets_accounts.ToList());
        }

        // GET: Assets_accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_CreateAccount_Table assets_accounts = db.C_CreateAccount_Tables.Find(id);
            if (assets_accounts == null)
            {
                return HttpNotFound();
            }
            return View(assets_accounts);
        }
        void SetAccountViewBage()
        {
            List<C_CreateAccount_Table> MyAccounts = GetAccount(Posting_type.BallanceSheet).ToList();
            List<C_CreateAccount_Table> GetAccountNoPrifixProp = GetAccountNoPrifix().Where(x => x.PostingType == Posting_type.PL.ToString()).ToList();

            ViewBag.Cost_account = new SelectList(MyAccounts, "C_AID", "AccountName");
            ViewBag.Deprecation_accumulated_account = new SelectList(MyAccounts, "C_AID", "AccountName");
            ViewBag.Lose_account = new SelectList(GetAccountNoPrifixProp, "C_AID", "AccountName");
            ViewBag.Payable_account = new SelectList(GetAccount(Prefix.Pay, Posting_type.BallanceSheet), "C_AID", "AccountName");
            ViewBag.Receivable_account = new SelectList(GetAccount(Prefix.REC, Posting_type.BallanceSheet), "C_AID", "AccountName");
            ViewBag.Profit_account = new SelectList(GetAccountNoPrifixProp, "C_AID", "AccountName");
            ViewBag.Retirment = new SelectList(MyAccounts, "C_AID", "AccountName");
            ViewBag.Revaluation_account = new SelectList(GetAccountNoPrifixProp, "C_AID", "AccountName");
            ViewBag.Deprcation = new SelectList(GetAccountNoPrifixProp, "C_AID", "AccountName");
            ViewBag.Accrued = new SelectList(GetAccountNoPrifix().ToList().Where(x => x.PostingType == Posting_type.BallanceSheet.ToString()), "C_AID", "AccountName");
            ViewBag.Class_Id = new SelectList(FixedDb.Assets_class, "Id", "Description");
        }
        void SetAccountViewBage(Assets_accounts assets_accounts)
        {
            List<C_CreateAccount_Table> MyAccounts = GetAccount(Posting_type.BallanceSheet).ToList();
            List<C_CreateAccount_Table> GetAccountNoPrifixProp = GetAccountNoPrifix().Where(x => x.PostingType == Posting_type.PL.ToString()).ToList();

            ViewBag.Cost_account = new SelectList(MyAccounts, "C_AID", "AccountName", assets_accounts.Cost_account);
            ViewBag.Deprecation_accumulated_account = new SelectList(MyAccounts, "C_AID", "AccountName", assets_accounts.Deprecation_accumulated_account);
            ViewBag.Lose_account = new SelectList(GetAccountNoPrifixProp, "C_AID", "AccountName", assets_accounts.Lose_account);
            ViewBag.Payable_account = new SelectList(GetAccount(Prefix.Pay, Posting_type.BallanceSheet), "C_AID", "AccountName", assets_accounts.Payable_account);
            ViewBag.Receivable_account = new SelectList(GetAccount(Prefix.REC, Posting_type.BallanceSheet), "C_AID", "AccountName", assets_accounts.Receivable_account);
            ViewBag.Profit_account = new SelectList(GetAccountNoPrifixProp, "C_AID", "AccountName", assets_accounts.Profit_account);
            ViewBag.Retirment = new SelectList(MyAccounts, "C_AID", "AccountName", assets_accounts.Retirment);
            ViewBag.Revaluation_account = new SelectList(GetAccountNoPrifixProp, "C_AID", "AccountName", assets_accounts.Revaluation_account);
            ViewBag.Deprcation = new SelectList(GetAccountNoPrifixProp, "C_AID", "AccountName", assets_accounts.Deprcation);
            ViewBag.Accrued = new SelectList(GetAccountNoPrifix().ToList().Where(x => x.PostingType == Posting_type.BallanceSheet.ToString()), "C_AID", "AccountName", assets_accounts.Accrued);
            ViewBag.Class_Id = new SelectList(FixedDb.Assets_class, "Id", "Description", assets_accounts.Class_id);
        }
        // GET: Assets_accounts/Create
        public ActionResult Create()
        {
            SetAccountViewBage();
            return View();
        }

        // POST: Assets_accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Accrued,Class_id,Cost_account,Deprecation_accumulated_account,Profit_account,Lose_account,Payable_account,Receivable_account,Revaluation_account,Deprcation,Retirment")] Assets_accounts assets_accounts)
        {
            if (ModelState.IsValid)
            {
                FixedDb.Assets_accounts.Add(assets_accounts);
                db.C_CreateAccount_Tables.Where(x => string.IsNullOrEmpty(x.C_Prefix)).Where(x => x.C_AID == assets_accounts.Retirment || x.C_AID == assets_accounts.Cost_account || x.C_AID == assets_accounts.Deprecation_accumulated_account)
                    .ToList().ForEach(x => x.C_Prefix = Prefix.Asset.ToString());
                FixedDb.SaveChanges();
                db.SaveChanges();
                return Json(1);
            }

            SetAccountViewBage();

            return View(assets_accounts);
        }

        // GET: Assets_accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assets_accounts assets_accounts = FixedDb.Assets_accounts.FirstOrDefault(x => x.Class_id == id);
            SetAccountViewBage(assets_accounts);

            if (assets_accounts == null)
            {
                return HttpNotFound();
            }

            return View(assets_accounts);
        }

        // POST: Assets_accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Accrued,Class_id,Cost_account,Deprecation_accumulated_account,Profit_account,Lose_account,Payable_account,Receivable_account,Revaluation_account,Deprcation,Retirment")] Assets_accounts assets_accounts)
        {
            if (ModelState.IsValid)
            {
                FixedDb.Entry(assets_accounts).State = EntityState.Modified;
                FixedDb.SaveChanges();
                return Json(0);
            }
            SetAccountViewBage(assets_accounts);
            return View(assets_accounts);
        }

        // GET: Assets_accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assets_accounts assets_accounts = FixedDb.Assets_accounts.Find(id);
            if (assets_accounts == null)
            {
                return HttpNotFound();
            }
            return View(assets_accounts);
        }

        // POST: Assets_accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assets_accounts assets_accounts = FixedDb.Assets_accounts.Find(id);
            FixedDb.Assets_accounts.Remove(assets_accounts);
            db.SaveChanges();
            return RedirectToAction("Index");
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
