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
using static FixedAssets.Business.Business;

namespace FixedAssets.Controllers
{
    public class Deprecation_SettingController : Controller
    {
        private DBContext db = new DBContext();

        // GET: FixedAssets/Deprecation_Setting
        public ActionResult Index()
        {
            if (!db.Deprecation_Setting.Any())
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                db.Deprecation_Setting.Add(new Deprecation_Setting
                {
                   Auto_or_manual=(int)Auto_or_manual.Manual,
                   Deprecation_calcualtion=(int)Deprecation_calcualtion.Yearly,
                   Deprecation_jv=(int)Deprecation_jv.Yearly,
                   Can_add_assets_info=true,
                   Company_id= companyID
                });
                db.SaveChanges();
            }
            return View(db.Deprecation_Setting.ToList());
        }

        // GET: FixedAssets/Deprecation_Setting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deprecation_Setting deprecation_Setting = db.Deprecation_Setting.Find(id);
            if (deprecation_Setting == null)
            {
                return HttpNotFound();
            }
            return View(deprecation_Setting);
        }

        // GET: FixedAssets/Deprecation_Setting/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: FixedAssets/Deprecation_Setting/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Auto_or_manual,Deprecation_calcualtion,Deprecation_jv,Change_deprecation_method")] Deprecation_Setting deprecation_Setting)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Deprecation_Setting.Add(deprecation_Setting);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(deprecation_Setting);
        //}

        // GET: FixedAssets/Deprecation_Setting/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deprecation_Setting deprecation_Setting = db.Deprecation_Setting.Find(id);

            //Array Auto = Enum.GetValues(typeof(Auto_or_manual));
            //Array Deprecation_calcualtion_MyEnum = Enum.GetValues(typeof(Deprecation_calcualtion));
            //Array Deprecation_jv_MyEnum = Enum.GetValues(typeof(Deprecation_jv));

            deprecation_Setting.Auto_or_manual_enum = (Auto_or_manual)deprecation_Setting.Auto_or_manual;
            deprecation_Setting.Deprecation_calcualtion_enum = (Deprecation_calcualtion)deprecation_Setting.Deprecation_calcualtion;
            deprecation_Setting.Deprecation_jv_enum = (Deprecation_jv)deprecation_Setting.Deprecation_jv;



            return View(deprecation_Setting);
        }

        // POST: FixedAssets/Deprecation_Setting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Deprecation_Setting deprecation_Setting)
        {
            if (ModelState.IsValid)
            {
                deprecation_Setting.Auto_or_manual = (int)deprecation_Setting.Auto_or_manual_enum;
                deprecation_Setting.Deprecation_calcualtion = (int)deprecation_Setting.Deprecation_calcualtion_enum;
               // deprecation_Setting.Deprecation_jv = (int)deprecation_Setting.Deprecation_jv_enum;

                db.Entry(deprecation_Setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deprecation_Setting);
        }

        // GET: FixedAssets/Deprecation_Setting/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Deprecation_Setting deprecation_Setting = db.Deprecation_Setting.Find(id);
        //    if (deprecation_Setting == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(deprecation_Setting);
        //}

        // POST: FixedAssets/Deprecation_Setting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deprecation_Setting deprecation_Setting = db.Deprecation_Setting.Find(id);
            db.Deprecation_Setting.Remove(deprecation_Setting);
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
