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
    public class Assets_mainController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Assets_main
        public ActionResult Index()
        {
            var assets_main = db.Assets_main.Include(a => a.Assets_class);
            return View(assets_main.ToList());
        }
        public JsonResult GetMainByClass(int ClassId)
        {
            try
            {
                List<Assets_main> AM = db.Assets_main.Where(x => x.Assets_class_id == ClassId).ToList();
                if (AM.Any())
                {
                    return Json(AM.Select(x=>new {x.Id,x.Description }));
                }
                else
                {
                    return Json(new List<Assets_main> { new Assets_main { Description = "No Data Found" } });
                }
            }
            catch
            {
                return Json(new List<Assets_main> { new Assets_main {Description="No Data Found" } });
            }
        }
        // GET: Assets_main/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assets_main assets_main = db.Assets_main.Find(id);
            ViewBag.Id = id;
            if (assets_main == null)
            {
                return HttpNotFound();
            }
            return View(assets_main);
        }
        public ActionResult GetNumberOfUnit(int? id)
        {
            try
            {
                return Json(db.Assets_main.Find(id).Number_of_parts);

            }
            catch
            {
                return Json(0);
            }
        }
        // GET: Assets_main/Create
        public ActionResult Create()
        {
            ViewBag.Assets_class_id = new SelectList(db.Assets_class, "Id", "Description");
            return View(new InsertAssets_main { });
        }

        // POST: Assets_main/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Assets_main assets_main)
        {
            if (ModelState.IsValid)
            {
                db.Assets_main.Add(assets_main);
                int? Assets_custom_id = db.Assets_main.Max(x => x.Assets_custom_id);
                if (Assets_custom_id.HasValue)
                {
                    assets_main.Assets_custom_id = Assets_custom_id.Value + 1;
                }
                else
                {
                    assets_main.Assets_custom_id = 1;
                }
                db.SaveChanges();

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Assets_class_id = new SelectList(db.Assets_class, "Id", "Description", assets_main.Assets_class_id);
            return View(Business.Business.GetInsertAssets(assets_main));

        }

        // GET: Assets_main/Edit/5
        public ActionResult Edit(int? id,bool IsPartial=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assets_main assets_main = db.Assets_main.Find(id);
            ViewBag.IsPartial = IsPartial;
            if (assets_main == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assets_class_id = new SelectList(db.Assets_class, "Id", "Description", assets_main.Assets_class_id);
            return View(assets_main);
        }
        // POST: Assets_main/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Assets_main assets_main)
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(assets_main).State = EntityState.Unchanged;
                
                db.Entry(assets_main).Property(x=>x.Description).IsModified = true;
                db.Entry(assets_main).Property(x=>x.Number_of_parts).IsModified = true;
                db.Entry(assets_main).Property(x=>x.Inactive).IsModified = true;
                
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.Assets_class_id = new SelectList(db.Assets_class, "Id", "Description", assets_main.Assets_class_id);
            return View(Business.Business.GetInsertAssets(assets_main));

        }

        // GET: Assets_main/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assets_main assets_main = db.Assets_main.Find(id);
            if (assets_main == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = id;
            ViewBag.Error = TempData["Error"];

            return View(assets_main);
        }

        // POST: Assets_main/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Assets_main assets_main = db.Assets_main.Find(id);
                if (assets_main.Assets.Any())
                {
                    TempData["Error"] = "you can't delete this assets main because it has assets in it ";
                    return RedirectToAction("Delete", new { id = id });
                }
                db.Delete_assets_main.Add(Business.Business.GetDeleteMainClass(assets_main));
                db.Assets_main.Remove(assets_main);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");

            }

        }
        public ActionResult DeleteWithAll(int id)
        {
            try
            {
                db.Delete_assets_main.Add(Business.Business.GetDeleteMainClass(db.Assets_main.Find(id)));
                Assets_main assets_main = db.Assets_main.Find(id);

                if (assets_main.Assets.Any())
                {
                    db.Assets.RemoveRange(db.Assets.Where(x => x.Assets_class_id == id));
                }
               
                db.Assets_main.Remove(assets_main);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                try
                {
                    string St = ex.InnerException.InnerException.ToString();

                    int pFrom = St.IndexOf("table") + "table".Length;
                    int pTo = St.LastIndexOf("column");

                    String result = St.Substring(pFrom, pTo - pFrom).Replace("\"dbo.", "").Replace("\"", "");
                    TempData["Error"] = $"This Assets Has {result} can't be deleted";
                    return RedirectToAction("Delete");

                }
                catch
                {

                }

            }
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
