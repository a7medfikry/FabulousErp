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
    public class Inv_item_optionController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_item_option
        public ActionResult Index()
        {
            var inv_item_option = db.Inv_item_option.Include(i => i.Item);
            return View(inv_item_option.ToList());
        }

        // GET: Inventory/Inv_item_option/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item_option inv_item_option = db.Inv_item_option.Find(id);
            if (inv_item_option == null)
            {
                return HttpNotFound();
            }
            return View(inv_item_option);
        }

        // GET: Inventory/Inv_item_option/Create
        public ActionResult Create()
        {
           // ViewBag.Inv_item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            return View();
        }

        // POST: Inventory/Inv_item_option/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Inv_item_id,Height,Width,Depth,Size_type,Wight,Wight_type,Image")] Inv_item_option inv_item_option)
        {
            if (ModelState.IsValid)
            {
                db.Inv_item_option.Add(inv_item_option);
                db.SaveChanges();
                return Json(1);
            }

            ViewBag.Inv_item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_item_option.Inv_item_id);
            return View(inv_item_option);
        }

        // GET: Inventory/Inv_item_option/Edit/5
        public ActionResult Edit(int? ItemId)
        {
            if (ItemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item_option inv_item_option = db.Inv_item_option.FirstOrDefault(x=>x.Inv_item_id==ItemId);
            if (inv_item_option == null)
            {
                inv_item_option = new Inv_item_option
                {
                    Inv_item_id=ItemId.Value
                };
            }
            ViewBag.Inv_item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_item_option.Inv_item_id);
            return View(inv_item_option);
        }

        // POST: Inventory/Inv_item_option/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Inv_item_id,Height,Width,Depth,Size_type,Wight,Wight_type,Image")] Inv_item_option inv_item_option)
        {
            if (ModelState.IsValid)
            {
                if (inv_item_option.Id == 0)
                {
                    db.Inv_item_option.Add(inv_item_option);
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(inv_item_option).State = EntityState.Modified;
                    db.SaveChanges();
                }
                    return Json(1);

            }
            ViewBag.Inv_item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_item_option.Inv_item_id);
            return View(inv_item_option);
        }

        // GET: Inventory/Inv_item_option/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item_option inv_item_option = db.Inv_item_option.Find(id);
            if (inv_item_option == null)
            {
                return HttpNotFound();
            }
            return View(inv_item_option);
        }

        // POST: Inventory/Inv_item_option/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_item_option inv_item_option = db.Inv_item_option.Find(id);
            db.Inv_item_option.Remove(inv_item_option);
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
