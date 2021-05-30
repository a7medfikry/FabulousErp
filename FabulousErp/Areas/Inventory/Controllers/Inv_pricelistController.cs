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
    public class Inv_pricelistController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_pricelist
        public ActionResult Index(int? ItemId)
        {
            List<Inv_pricelist> P = new List<Inv_pricelist>();
            if (ItemId.HasValue)
            {
                P = db.Inv_pricelist.Where(x => x.Item_id == ItemId).Include(x=>x.UOM).Include(i => i.Currency).Include(i => i.Item).ToList();
            }
            else
            {
                return RedirectToAction("Create");
            }
            return View(P);
        }
        // GET: Inventory/Inv_pricelist/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_pricelist inv_pricelist = db.Inv_pricelist.Find(id);
            if (inv_pricelist == null)
            {
                return HttpNotFound();
            }
            return View(inv_pricelist);
        }

        // GET: Inventory/Inv_pricelist/Create
        public ActionResult Create()
        {
            string CompanyId = FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode");
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            ViewBag.Unit_of_measure_id = new SelectList(new List<string> { });
            return View();
        }

        // POST: Inventory/Inv_pricelist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Item_id,Desc,Price_lvl,Currency_id,Unit_of_measure_id,Start_quantity,End_quantity,Price")] Inv_pricelist inv_pricelist)
        {
            if (ModelState.IsValid)
            {
                db.Inv_pricelist.Add(inv_pricelist);
                db.SaveChanges();
                return Json(1);
            }
            string CompanyId = FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode");

            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_pricelist.Item_id);
            ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_pricelist.Unit_of_measure_id);
            return View(inv_pricelist);
        }
        public static decimal UnitPrice(int ItemId, float Qty,Price_lvl Pricelvl,int UOM)
        {
            DBContext db = new DBContext();
           return db.Inv_pricelist.Where(x => x.Item_id == ItemId && x.Start_quantity <= Qty
            && x.End_quantity >= Qty && x.Price_lvl== Pricelvl&&x.Unit_of_measure_id== UOM)
                .ToList().DefaultIfEmpty(new Inv_pricelist { Price = 0 }).FirstOrDefault().Price;
        }
        public JsonResult GetUnitPrice(int ItemId,float Qty, Price_lvl Pricelvl,int UOM)
        {
           if (!db.Inv_sales_GS.FirstOrDefault().Override_price_in_price_list)
            {
                return Json(UnitPrice(ItemId, Qty, Pricelvl, UOM).ToString(FabulousErp.Business.GetDecimalNumber()));
            }
            else
            {
                return Json(0);
            }
        }
        // GET: Inventory/Inv_pricelist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_pricelist inv_pricelist = db.Inv_pricelist.Find(id);
            if (inv_pricelist == null)
            {
                return HttpNotFound();
            }

            string CompanyId = FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode");
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_pricelist.Item_id);
            ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_pricelist.Unit_of_measure_id);
            return View(inv_pricelist);
        }

        // POST: Inventory/Inv_pricelist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Item_id,Desc,Price_lvl,Currency_id,Unit_of_measure_id,Start_quantity,End_quantity,Price")] Inv_pricelist inv_pricelist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_pricelist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            string CompanyId = FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode");
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_pricelist.Item_id);
            ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_pricelist.Unit_of_measure_id);
            return View(inv_pricelist);
        }

        // GET: Inventory/Inv_pricelist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_pricelist inv_pricelist = db.Inv_pricelist.Find(id);
            if (inv_pricelist == null)
            {
                return HttpNotFound();
            }
            return View(inv_pricelist);
        }

        // POST: Inventory/Inv_pricelist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_pricelist inv_pricelist = db.Inv_pricelist.Find(id);
            db.Inv_pricelist.Remove(inv_pricelist);
            db.SaveChanges();
            return RedirectToAction("Create");
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
