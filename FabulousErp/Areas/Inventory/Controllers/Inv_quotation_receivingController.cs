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

namespace Inventory.Controllers
{
    public class Inv_quotation_receivingController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_quotation_receiving
        public ActionResult Index()
        {
            var inv_quotation_receiving = db.Inv_quotation_receiving.Include(i => i.Currency).Include(i => i.Pr_no).Include(i => i.Qutation);
            return View(inv_quotation_receiving.ToList());
        }

        // GET: Inventory/Inv_quotation_receiving/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_quotation_receiving inv_quotation_receiving = db.Inv_quotation_receiving.Find(id);
            if (inv_quotation_receiving == null)
            {
                return HttpNotFound();
            }
            return View(inv_quotation_receiving);
        }

       // GET: Inventory/Inv_quotation_receiving/Create
        public ActionResult Create()
        {
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables, "CurrencyID", "CurrencyName");
            ViewBag.Pr_no_id = new SelectList(db.Inv_purchase_request, "Id", "Po_number");
            ViewBag.Qutation_num_id = new SelectList(db.Inv_quotation_request, "Id", "Request_for_qut_no");
            return View();
        }

        // POST: Inventory/Inv_quotation_receiving/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Pr_no_id,Currency_id,Qutation_num_id,Vendore_id,Payment_term")] Inv_quotation_receiving inv_quotation_receiving)
        {
            if (ModelState.IsValid)
            {
                db.Inv_quotation_receiving.Add(inv_quotation_receiving);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables, "CurrencyID", "CurrencyName", inv_quotation_receiving.Currency_id);
            ViewBag.Pr_no_id = new SelectList(db.Inv_purchase_request, "Id", "Po_number", inv_quotation_receiving.Pr_no_id);
            ViewBag.Qutation_num_id = new SelectList(db.Inv_quotation_request, "Id", "Request_for_qut_no", inv_quotation_receiving.Qutation_num_id);
            return View(inv_quotation_receiving);
        }

        // GET: Inventory/Inv_quotation_receiving/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_quotation_receiving inv_quotation_receiving = db.Inv_quotation_receiving.Find(id);
            if (inv_quotation_receiving == null)
            {
                return HttpNotFound();
            }
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables, "CurrencyID", "CurrencyName", inv_quotation_receiving.Currency_id);
            ViewBag.Pr_no_id = new SelectList(db.Inv_purchase_request, "Id", "Po_number", inv_quotation_receiving.Pr_no_id);
            ViewBag.Qutation_num_id = new SelectList(db.Inv_quotation_request, "Id", "Request_for_qut_no", inv_quotation_receiving.Qutation_num_id);
            return View(inv_quotation_receiving);
        }

        // POST: Inventory/Inv_quotation_receiving/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Pr_no_id,Currency_id,Qutation_num_id,Vendore_id,Payment_term")] Inv_quotation_receiving inv_quotation_receiving)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_quotation_receiving).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables, "CurrencyID", "CurrencyName", inv_quotation_receiving.Currency_id);
            ViewBag.Pr_no_id = new SelectList(db.Inv_purchase_request, "Id", "Po_number", inv_quotation_receiving.Pr_no_id);
            ViewBag.Qutation_num_id = new SelectList(db.Inv_quotation_request, "Id", "Request_for_qut_no", inv_quotation_receiving.Qutation_num_id);
            return View(inv_quotation_receiving);
        }

        // GET: Inventory/Inv_quotation_receiving/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_quotation_receiving inv_quotation_receiving = db.Inv_quotation_receiving.Find(id);
            if (inv_quotation_receiving == null)
            {
                return HttpNotFound();
            }
            return View(inv_quotation_receiving);
        }

        // POST: Inventory/Inv_quotation_receiving/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_quotation_receiving inv_quotation_receiving = db.Inv_quotation_receiving.Find(id);
            db.Inv_quotation_receiving.Remove(inv_quotation_receiving);
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
