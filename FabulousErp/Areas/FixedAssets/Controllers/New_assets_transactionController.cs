using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.Models;
using FabulousDB.DB_Tabels;
using FabulousDB.DB_Context;
using System.Data.Entity.Migrations;

namespace FixedAssets.Controllers
{
    public class New_assets_transactionController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext Mdb = new DBContext();
        private DBContext dbP = new DBContext();
        // GET: FixedAssets/New_assets_transaction
        public ActionResult Index()
        {
            return View(db.New_assets_transaction.ToList());
        }

        // GET: FixedAssets/New_assets_transaction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            New_assets_transaction new_assets_transaction = db.New_assets_transaction.Find(id);
            if (new_assets_transaction == null)
            {
                return HttpNotFound();
            }
            return View(new_assets_transaction);
        }
        public JsonResult DetailsJson(int? id,Purchase_type P)
        {
            try
            {
                if (P == Purchase_type.Direct)
                {
                    New_assets_transaction new_assets_transaction = db.New_assets_transaction.Find(id);
                    return Json(new_assets_transaction.Acquesation_cost);
                }
                else if (P==Purchase_type.Payable)
                {
                    using (DBContext dbP = new DBContext())
                    {
                        FabulousErp.Payable.Models.Payable_transaction PT = dbP.Payable_transactions.Find(id);
                        return Json(PT.Purchase-PT.Taken_discount);
                    }
                }
                return Json("");
            }
            catch
            {
                return Json("");

            }
        }
        // GET: FixedAssets/New_assets_transaction/Create
        public ActionResult Create(int? id)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();


            New_assets_transaction new_assets_transaction = db.New_assets_transaction.Find(id);
            if (new_assets_transaction == null)
            {
                new_assets_transaction = new New_assets_transaction
                {

                };
            }
            ViewBag.Currency_id = new SelectList(Mdb.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList(), "CurrencyID", "ISOCode", new_assets_transaction.Currency_id);

            ViewBag.Assets_class_id = new SelectList(db.Assets_class.Where(x => x.Active == false), "Id", "Description", new_assets_transaction.Assets_class_id);

            if (new_assets_transaction == null)
            {
                return HttpNotFound();
            }
            return View(new_assets_transaction);
        }

        // POST: FixedAssets/New_assets_transaction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(New_assets_transaction new_assets_transaction)
        {
            if (ModelState.IsValid)
            {
                db.New_assets_transaction.AddOrUpdate(new_assets_transaction);
                db.SaveChanges();
                return Json(new_assets_transaction.Id);
            }

            return View(new_assets_transaction);
        }

        // GET: FixedAssets/New_assets_transaction/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            New_assets_transaction new_assets_transaction = db.New_assets_transaction.Find(id);
            ViewBag.Currency_id = new SelectList(Mdb.CurrenciesDefinition_Tables.ToList(), "CompanyID", "CurrencyName", new_assets_transaction.Currency_id);
            ViewBag.Assets_class_id = new SelectList(db.Assets_class.Where(x => x.Active == false), "Id", "Description");

            if (new_assets_transaction == null)
            {
                return HttpNotFound();
            }
            return View(new_assets_transaction);
        }

        // POST: FixedAssets/New_assets_transaction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Acquesation_cost,Date_of_orgin,Currency_id,Assets_class,Reference,Vendor_id,Type")] New_assets_transaction new_assets_transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(new_assets_transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(new_assets_transaction);
        }

        // GET: FixedAssets/New_assets_transaction/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            New_assets_transaction new_assets_transaction = db.New_assets_transaction.Find(id);
            if (new_assets_transaction == null)
            {
                return HttpNotFound();
            }
            return View(new_assets_transaction);
        }

        // POST: FixedAssets/New_assets_transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            New_assets_transaction new_assets_transaction = db.New_assets_transaction.Find(id);
            db.New_assets_transaction.Remove(new_assets_transaction);
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
