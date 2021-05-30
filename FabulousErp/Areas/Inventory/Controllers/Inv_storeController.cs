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
    public class Inv_storeController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_store
        public ActionResult Index()
        {
            return View(db.Inv_store.ToList());
        }

        // GET: Inventory/Inv_store/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_store inv_store = db.Inv_store.Find(id);
            if (inv_store == null)
            {
                return HttpNotFound();
            }
            return View(inv_store);
        }

        // GET: Inventory/Inv_store/Create
        public ActionResult Create()
        {
            ViewBag.Store_gl_account_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName");
            return View(new Inv_store {Next_gr_no=1,Next_goods_no=1 });
        }

        // POST: Inventory/Inv_store/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inv_store inv_store)
        {
            if (ModelState.IsValid)
            {
                db.Inv_store.Add(inv_store);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Store_gl_account_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName", inv_store.Store_gl_account_id);
            return View(inv_store);
        }

        // GET: Inventory/Inv_store/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_store inv_store = db.Inv_store.Find(id);
            if (inv_store == null)
            {
                return HttpNotFound();
            }
            ViewBag.Store_gl_account_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName", inv_store.Store_gl_account_id);
            return View(inv_store);
        }

        // POST: Inventory/Inv_store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inv_store inv_store)
        {
            ModelState["Store_id"].Errors.Clear();
            if (ModelState.IsValid)
            {
                db.Entry(inv_store).State = EntityState.Modified;
                db.Entry(inv_store).Property(x => x.Store_id).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Store_gl_account_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName", inv_store.Store_gl_account_id);
            return View(inv_store);
        }

        // GET: Inventory/Inv_store/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_store inv_store = db.Inv_store.Find(id);
            if (inv_store == null)
            {
                return HttpNotFound();
            }
            return View(inv_store);
        }

        // POST: Inventory/Inv_store/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Inv_store inv_store = db.Inv_store.Include(x=>x.Sites).FirstOrDefault(x=>x.Id==id);
                if (inv_store.Sites.Count()>0)
                {
                    return Json(0);
                }
                db.Inv_store.Remove(inv_store);
                db.SaveChanges();
                return Json(1);
            }
            catch
            {
                return Json(0);

            }

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
