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
    public class Deprecation_recordController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Deprecation_record
        public ActionResult Index(int? DeprecationId = null,bool IsMain=false)
        {
            ViewBag.CompanyID = (string)FabulousErp.Business.GetCompanyId();

            if (Request["section"]== "Inquiry"|| Request["section"] == "Reports")
            {
                if (Request["section"] == "Reports")
                {
                    var deprecation_record = db.Deprecation_record.Include(d => d.Asset);
                    return View(deprecation_record.ToList());
                }
                else
                {
                    var deprecation_record = db.Deprecation_record.Where(x => x.Deprecation_id == DeprecationId).Include(d => d.Asset);
                    ViewBag.JlNumber = deprecation_record.FirstOrDefault().Deprecation.Jornal_number;
                    return View(deprecation_record.ToList());
                }
             
            }
            else
            {
                return RedirectToAction("Index_temp",new { DeprecationId = DeprecationId });
            }

        }
        public ActionResult Index_temp(int? DeprecationId = null)
        {
            ViewBag.DeprecationId = DeprecationId;
            List<Deprecation_temp_record> deprecation_record = db.Deprecation_temp_record.Include(d => d.Asset).Where(x => x.Deprecation_id == DeprecationId).ToList();
            if (!deprecation_record.Any())
            {
                return View(new List<Deprecation_temp_record> { new Deprecation_temp_record { Deprecation_id=null } });
            }
            // ViewBag.JlNumber = deprecation_record.FirstOrDefault(x => x.Deprecation_id == DeprecationId).Deprecation_temp.Jornal_number;
            ViewBag.PostingDate = db.Deprecation_temp.Find(DeprecationId).Transaction_date.Value.ToString("yyyy-MM-dd");
            ViewBag.CompId = FabulousErp.Business.GetCompanyId();
            return View(deprecation_record.ToList());
        }
        // GET: Deprecation_record/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deprecation_record deprecation_record = db.Deprecation_record.Find(id);
            if (deprecation_record == null)
            {
                return HttpNotFound();
            }
            return View(deprecation_record);
        }

        // GET: Deprecation_record/Create
        public ActionResult Create()
        {
            ViewBag.Asset_id = new SelectList(db.Assets, "Id", "Description");
            return View();
        }

        // POST: Deprecation_record/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Asset_id,Date,Assets_acquisition_cost,Renewal_amount,Disposal_amount,Total,Beginning_deprecation_accumulated,Depreication,Renewal_depreication,Disposal_depreication,Ending_deprecication_accumulated,Net_assets_cost")] Deprecation_record deprecation_record)
        {
            if (ModelState.IsValid)
            {
                db.Deprecation_record.Add(deprecation_record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Asset_id = new SelectList(db.Assets, "Id", "Description", deprecation_record.Asset_id);
            return View(deprecation_record);
        }

        // GET: Deprecation_record/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deprecation_temp_record deprecation_record = db.Deprecation_temp_record.Find(id);
            if (deprecation_record == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepId = deprecation_record.Deprecation_id;
            ViewBag.Asset_id = new SelectList(db.Assets, "Id", "Description", deprecation_record.Asset_id);
            return View(deprecation_record);
        }
        public ActionResult MainEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deprecation_record deprecation_record = db.Deprecation_record.Find(id);
            if (deprecation_record == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepId = deprecation_record.Deprecation_id;
            ViewBag.Asset_id = new SelectList(db.Assets, "Id", "Description", deprecation_record.Asset_id);
            return View(deprecation_record);
        }

        // POST: Deprecation_record/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Deprecation_temp_record deprecation_record)
        {
            int? DepId = deprecation_record.Deprecation_id;

            if (ModelState.IsValid)
            {

                Deprecation_temp_record DA = db.Deprecation_temp_record.Include(x => x.Deprecation_temp).Include(x => x.Asset)
                    
                    .Where(x => x.Id == deprecation_record.Id).FirstOrDefault();
                DA.Depreication = deprecation_record.Depreication;
                DA.Renewal_depreication = deprecation_record.Renewal_depreication;
                
                DA.Ending_deprecication_accumulated = DA.Beginning_deprecation_accumulated +
                    DA.Depreication + DA.Renewal_depreication;
                DA.Net_assets_cost = DA.Total - DA.Ending_deprecication_accumulated;

                DA.Total = DA.Ending_deprecication_accumulated + DA.Net_assets_cost;
                db.SaveChanges();
                return RedirectToAction("Index", new { DeprecationId = DA.Deprecation_id });
            }
            ViewBag.Asset_id = new SelectList(db.Assets, "Id", "Description", deprecation_record.Asset_id);
            return RedirectToAction("Index", new { DeprecationId = deprecation_record.Deprecation_id });
        }

        // GET: Deprecation_record/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deprecation_record deprecation_record = db.Deprecation_record.Find(id);
            if (deprecation_record == null)
            {
                return HttpNotFound();
            }
            return View(deprecation_record);
        }
        public JsonResult SubmitDepreciation(int Id,string JN)
        {
            Deprecation_temp DT= db.Deprecation_temp.Include(x=>x.Deprecation_temp_record).FirstOrDefault(x=>x.Id==Id);
            List<Deprecation_temp_record> DR = DT.Deprecation_temp_record.ToList();
            Deprecation D = new Deprecation
            {
                Acquisition_cost= DT.Acquisition_cost,
                Adjustment_cost= DT.Adjustment_cost,
                Company_id= DT.Company_id,
                Createion_date= DT.Createion_date,
                Deprecation_date= DT.Deprecation_date,
                Deprecation_no= DT.Deprecation_no,
                Deprecation_periods= DT.Deprecation_periods,
                Deprecation_rate= DT.Deprecation_rate,
                Depreciation_accumulated=DT.Depreciation_accumulated,
                Depreication_assets_id_connection=null,
                Depreication_cost=DT.Depreication_cost,
                Is_assets_class=DT.Is_assets_class,
                Jornal_number= JN,
                Month=DT.Month,
                Period=DT.Period,
                Period_id=DT.Period_id,
                Special_depreication=DT.Special_depreication,
                Transaction_date=DT.Transaction_date,
                Year=DT.Year
            };
            D.Deprecation_record = DR.Select(x => new Deprecation_record
            {
                Asset=x.Asset,
                Net_assets_cost=x.Net_assets_cost,
                Renewal_amount=x.Renewal_amount,
                Total=x.Total,
                Renewal_depreication=x.Renewal_depreication,
                Assets_acquisition_cost=x.Assets_acquisition_cost,
                Asset_id=x.Asset_id,
                Beginning_deprecation_accumulated=x.Beginning_deprecation_accumulated,
                Company_id=x.Company_id,
                Date=x.Date,
                Depreication=x.Depreication,
                Ending_deprecication_accumulated=x.Ending_deprecication_accumulated,
                Deprecation_id=x.Deprecation_id,
                Disposal_amount=x.Disposal_amount,
                Disposal_depreication=x.Disposal_depreication
            }).ToList();
            db.Deprecations.Add(D);
            db.SaveChanges();
            db.Deprecation_temp_record.RemoveRange(db.Deprecation_temp_record.ToList().Where(x=>x.Date==D.Deprecation_record.FirstOrDefault().Date).ToList());
            db.Deprecation_temp.RemoveRange(db.Deprecation_temp.ToList().Where(x=>x.Transaction_date==D.Transaction_date).ToList());
            db.SaveChanges();
            return Json(D.Id);
        }
        // POST: Deprecation_record/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deprecation_record deprecation_record = db.Deprecation_record.Find(id);
            db.Deprecation_record.Remove(deprecation_record);
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
