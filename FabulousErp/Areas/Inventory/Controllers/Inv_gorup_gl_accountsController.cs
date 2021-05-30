using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.Models;
using FabulousErp;
using FabulousErp.Bussiness;

namespace Inventory.Controllers
{
    public class Inv_gorup_gl_accountsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_gorup_gl_accounts
        public ActionResult Index()
        {
            var asd = Business.GetAreasNames();
            var inv_gorup_gl_accounts = db.Inv_gorup_gl_accounts.Include(i => i.Cost_of_GS).Include(i => i.Damage).Include(i => i.Inventory).Include(i => i.item_group).Include(i => i.Purchase_variance).Include(i => i.Inventory_returne).Include(i => i.Sales).Include(i => i.Sales_return).Include(i => i.Variancies);
            return View(inv_gorup_gl_accounts.ToList());
        }

        // GET: Inventory/Inv_gorup_gl_accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_gorup_gl_accounts inv_gorup_gl_accounts = db.Inv_gorup_gl_accounts.Find(id);
            if (inv_gorup_gl_accounts == null)
            {
                return HttpNotFound();
            }
            return View(inv_gorup_gl_accounts);
        }

        // GET: Inventory/Inv_gorup_gl_accounts/Create
        public ActionResult Create(int GroupId)
        {
            Inv_gorup_gl_accounts S = db.Inv_gorup_gl_accounts.Where(x => x.Item_group_id == GroupId)
              .ToList().DefaultIfEmpty(new Inv_gorup_gl_accounts { }).FirstOrDefault();

            ViewBag.Inventory_id = new SelectList(db.C_CreateAccount_Tables.Where(x =>x.InventoryArea==true && x.ReconcileAccount == true
            && (x.C_Prefix == null || x.C_Prefix == "INV") && x.PostingType == PostingType.BallanceSheet.ToString()), 
            "C_AID", "AccountName",S.Inventory_id);


            ViewBag.Cost_of_GS_id = new SelectList(db.C_CreateAccount_Tables.Where(x=>x.InventoryArea==true), "C_AID", "AccountName",S.Cost_of_GS_id);
            ViewBag.Damage_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName", S.Damage_id);
            ViewBag.Item_group_id = new SelectList(db.Inv_item_group, "Id", "Item_group_id", S.Item_group_id);
            ViewBag.Purchase_variance_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName", S.Purchase_variance_id);
            ViewBag.Inventory_Returne_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName", S.Inventory_returne_id);
            ViewBag.Sales_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName", S.Sales_id);
            ViewBag.Sales_return_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName", S.Sales_return_id);
            ViewBag.Variancies_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName", S.Variancies_id);
            ViewBag.Fright_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName", S.Fright_id);
            ViewBag.Accrual_fright_id = new SelectList(db.C_CreateAccount_Tables.Where(x => x.InventoryArea == true), "C_AID", "AccountName",S.Accrual_fright_id);

            return View(S);
        }
        public JsonResult GetGroupAccounts(int GorupId)
        {
            try
            {
                return Json(db.Inv_gorup_gl_accounts.Where(x => x.Item_group_id == GorupId)
              .FirstOrDefault());
            }
            catch
            {
                return Json(new Inv_gorup_gl_accounts
                {

                });
            }

        }
        // POST: Inventory/Inv_gorup_gl_accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inv_gorup_gl_accounts inv_gorup_gl_accounts)
        {
            if (ModelState.IsValid)
            {
                if (inv_gorup_gl_accounts.Id == 0)
                {
                    db.Inv_gorup_gl_accounts.Add(inv_gorup_gl_accounts);
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(inv_gorup_gl_accounts).State = EntityState.Modified;
                    //Inv_gorup_gl_accounts I = db.Inv_gorup_gl_accounts.Find(inv_gorup_gl_accounts.Id);
                    //I.Cost_of_GS_id = inv_gorup_gl_accounts.Cost_of_GS_id;
                    //I.Damage_id = inv_gorup_gl_accounts.Damage_id;
                    //I.Inventory_id = inv_gorup_gl_accounts.Inventory_id;
                    //I.Purchase_variance_id = inv_gorup_gl_accounts.Purchase_variance_id;
                    //I.Inventory_returne_id = inv_gorup_gl_accounts.Inventory_returne_id;
                    //I.Sales_id = inv_gorup_gl_accounts.Sales_id;
                    //I.Sales_return_id = inv_gorup_gl_accounts.Sales_return_id;
                    //I.Variancies_id = inv_gorup_gl_accounts.Variancies_id;
                    //I.Fright_id = inv_gorup_gl_accounts.Fright_id;
                    //I.Accrual_fright_id = inv_gorup_gl_accounts.Accrual_fright_id;

                    db.SaveChanges();
                }
                FabulousErp.Business.SetPrefix(inv_gorup_gl_accounts.Inventory_id, "INV");
               
                return Json(1);
            }
            return Json(0);
        }

        // GET: Inventory/Inv_gorup_gl_accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_gorup_gl_accounts inv_gorup_gl_accounts = db.Inv_gorup_gl_accounts.Find(id);
            if (inv_gorup_gl_accounts == null)
            {
                return HttpNotFound();
            }
            return View(inv_gorup_gl_accounts);
        }

        // POST: Inventory/Inv_gorup_gl_accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Desc,Item_group_id,Inventory_id,Returne_id,Damage_id,Variancies_id,Sales_id,Sales_return_id,Cost_of_GS_id,Purchase_variance_id")] Inv_gorup_gl_accounts inv_gorup_gl_accounts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_gorup_gl_accounts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inv_gorup_gl_accounts);
        }

        // GET: Inventory/Inv_gorup_gl_accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_gorup_gl_accounts inv_gorup_gl_accounts = db.Inv_gorup_gl_accounts.Find(id);
            if (inv_gorup_gl_accounts == null)
            {
                return HttpNotFound();
            }
            return View(inv_gorup_gl_accounts);
        }

        // POST: Inventory/Inv_gorup_gl_accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_gorup_gl_accounts inv_gorup_gl_accounts = db.Inv_gorup_gl_accounts.Find(id);
            db.Inv_gorup_gl_accounts.Remove(inv_gorup_gl_accounts);
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
