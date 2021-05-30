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
    public class BooksController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Books
        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }
        public ActionResult Bookreport()
        {
            ViewBag.Class_id = new SelectList(db.Assets_class, "Id", "Description");

            return View();
        }

        public ActionResult BookreportRpt(DateTime DateFrom , DateTime DateTo,int? Class_id)
        {
            List<Book_report> Rpt = new List<Book_report>();
            List<Asset> Assets = db.Assets.ToList();
            if (Class_id != null)
            {
                Assets = Assets.Where(x => x.Assets_class_id == Class_id).ToList();
            }
            Rpt.AddRange(Assets.Select(x => new Book_report
            {
                Assets_Id = x.Assets_number,
                Desciption=x.Description,
                Acquestion_cost = x.Acquisation_cost,
                Renwal  = x.Fixed_assets_renewal.ToList().DefaultIfEmpty(new Fixed_assets_renewal {Renewal_amount=0 }).Sum(z => z.Renewal_amount).Value,
                Disposal  = x.Fixed_assets_disposel.ToList().DefaultIfEmpty(new Fixed_assets_disposel {Disposal_amount=0 }).Sum(z => z.Disposal_amount).Value,
                Total_assets = x.Acquisation_cost + x.Fixed_assets_renewal.ToList().DefaultIfEmpty(new Fixed_assets_renewal { Renewal_amount=0}).Sum(z => z.Renewal_amount).Value - x.Fixed_assets_disposel.DefaultIfEmpty(new Fixed_assets_disposel {Disposal_amount=0}).Sum(z => z.Disposal_amount).Value,
                Beginning_accumlated = x.Deprecation_record.ToList().DefaultIfEmpty(new Deprecation_record { Depreication = 0 }).Where(z => z.Date >= DateFrom).Sum(z => z.Depreication).Value,
                Depreciation = x.Deprecation_record.ToList().DefaultIfEmpty(new Deprecation_record { Depreication=0}).Sum(z => z.Depreication).Value,
                Renwl_deprecation = x.Deprecation_record.ToList().DefaultIfEmpty(new Deprecation_record {Renewal_depreication=0 }).Sum(z => z.Renewal_depreication).Value
            }));
            Rpt.ForEach(x =>
            {
                x.Ending_accumlated = x.Beginning_accumlated + x.Depreciation + x.Renwl_deprecation;
            });

            Rpt.ForEach(x =>
            {
                x.Net_assets = x.Total_assets + x.Ending_accumlated;
            });
            return View(Rpt);
        }
        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Error = TempData["Error"];
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            try
            {
                db.Books.Remove(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                string AssetsIds = "";
                try
                {
                    AssetsIds = String.Join(",", db.Assets.Where(x => x.Book_id == id).Select(x => x.Assets_number));
                }
                catch
                {

                }
                TempData["Error"] = $"this book has assets {AssetsIds} related to it";
                return RedirectToAction("Delete", new { id = id });
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
    public class Book_report
    {
        public string Assets_Id { get; set; }
        public string Desciption { get; set; }

        public decimal Acquestion_cost { get; set; }
        public decimal Renwal { get; set; }
        public decimal Disposal { get; set; }
        public decimal Total_assets { get; set; }
        public decimal Beginning_accumlated { get; set; }
        public decimal Depreciation { get; set; }
        public decimal Renwl_deprecation { get; set; }
        public decimal Ending_accumlated { get; set; }
        public decimal Net_assets { get; set; }
    }
}
