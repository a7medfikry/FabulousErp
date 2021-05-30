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
using FabulousErp;
using FabulousModels.ViewModels;

namespace Installment.Controllers
{
    public class Installment_contractController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext dbR = new DBContext();

        // GET: Installment/Installment_contract
        public ActionResult Index(string IsPayS="false")
        {
            bool IsPay = false;
            if (IsPayS == "True")
            {
                IsPay = true;

            }
            var installment_contracts = db.Installment_contracts.Where(x=> x.IsPay == IsPay).Include(i => i.Currency).Include(i => i.Installment_plan).Include(i => i.Vendore);
            installment_contracts.ToList().Where(x => x.Customer_id != null).ToList().ForEach(x => x.Customer_name = dbR.Receivable_vendore_settings.Find(x.Customer_id).Vendor_name);
            return View(installment_contracts.ToList());
        }

        // GET: Installment/Installment_contract/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Installment_contract installment_contract = db.Installment_contracts.Find(id);
            if (installment_contract == null)
            {
                return HttpNotFound();
            }
            return View(installment_contract);
        }

        // GET: Installment/Installment_contract/Create
        public ActionResult Create()
        {
            string CompId = FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x=>x.CompanyID== CompId), "CurrencyID", "ISOCode",CompId);
            ViewBag.Installment_plan_id = new SelectList(db.Installment_settings, "Id", "Plan_id");
            using (DBContext dbP = new DBContext())
            {
                ViewBag.Vendore_id = new SelectList(dbP.Payable_creditor_setting.ToList(), "Id", "Vendor_id");
                ViewBag.Vendore_invoices= new SelectList(new List<int>(){ });
            }
            using (DBContext dbR = new DBContext())
            {
                ViewBag.Customer_id = new SelectList(dbR.Receivable_vendore_settings.ToList(), "Id", "Vendor_id");
                ViewBag.Vendore_invoices= new SelectList(new List<int>(){ });
            }
            return View(new Installment_contract {
                Contract_no=(Business.GetDigits(db.Installment_contracts.Max(x=>x.Contract_no),true)).ToString(),
                Contract_date=DateTime.Now
            });
        }

        // POST: Installment/Installment_contract/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Installment_contract installment_contract
            ,int[] Vendore_invoices=null, string InstallmentStr = null)
        {
            List<Installments> Installment = null;

            Installment = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Installments>>(InstallmentStr);

            if (ModelState.IsValid)
            {
                installment_contract.Amount = Installment.Sum(x => x.Amount);
                db.Installment_contracts.Add(installment_contract);
               // db.SaveChanges();
               // Installment.ForEach(x => x.Contract_id = installment_contract.Id);
               
                db.Installments.AddRange(Installment);
                if (Vendore_invoices != null)
                {
                    if (installment_contract.IsPay == false)
                    {
                        db.Installment_contract_invoice.AddRange(Vendore_invoices
              .Select(x => new Installment_contract_invoice
              {
                  Invoice_id = x,
                  Contract_id = installment_contract.Id
              }));
                    }
                    else
                    {
                        db.Purchase_Installment_contract_invoice.AddRange(Vendore_invoices
         .Select(x => new Purchase_Installment_contract_invoice
         {
             Invoice_id = x,
             Contract_id = installment_contract.Id
         }));
                    }
                 
                }
             
                db.SaveChanges();
                return RedirectToAction("Index",new { IsPayS=installment_contract.IsPay });
            }

            string CompId = FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompId), "CurrencyID", "ISOCode");
            ViewBag.Installment_plan_id = new SelectList(db.Installment_settings, "Id", "Plan_id", installment_contract.Installment_plan_id);
            using (DBContext dbP = new DBContext())
            {
                ViewBag.Vendore_id = new SelectList(dbP.Payable_creditor_setting.ToList(), "Id", "Vendor_id");
            }

            using (DBContext dbR = new DBContext())
            {
                ViewBag.Customer_id = new SelectList(dbR.Receivable_vendore_settings.ToList(), "Id", "Vendor_id");
            }
            return View(installment_contract);
        }
       

        // GET: Installment/Installment_contract/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Installment_contract installment_contract = db.Installment_contracts.Find(id);
            if (installment_contract == null)
            {
                return HttpNotFound();
            }
            string CompId = FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompId), "CurrencyID", "ISOCode");
            ViewBag.Installment_plan_id = new SelectList(db.Installment_settings, "Id", "Plan_id", installment_contract.Installment_plan_id);
            using (DBContext dbP = new DBContext())
            {
                ViewBag.Vendore_id = new SelectList(dbP.Payable_creditor_setting.ToList(), "Id", "Vendor_id");
            }

            using (DBContext dbR = new DBContext())
            {
                ViewBag.Customer_id = new SelectList(dbR.Receivable_vendore_settings.ToList(), "Id", "Vendor_id");
            }
            return View(installment_contract);
        }

        // POST: Installment/Installment_contract/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Contract_no,Desc,Vendore_id,Customer_id,Amount,Currency_id,Installment_plan_id,Contract_date,Creation_date")] Installment_contract installment_contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(installment_contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            string CompId = FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompId), "CurrencyID", "ISOCode");
            ViewBag.Installment_plan_id = new SelectList(db.Installment_settings, "Id", "Plan_id", installment_contract.Installment_plan_id);
            using (DBContext dbP = new DBContext())
            {
                ViewBag.Vendore_id = new SelectList(dbP.Payable_creditor_setting.ToList(), "Id", "Vendor_id");
            }

            using (DBContext dbR = new DBContext())
            {
                ViewBag.Customer_id = new SelectList(dbR.Receivable_vendore_settings.ToList(), "Id", "Vendor_id");
            }
            return View(installment_contract);
        }

        // GET: Installment/Installment_contract/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Installment_contract installment_contract = db.Installment_contracts.Find(id);
            if (installment_contract == null)
            {
                return HttpNotFound();
            }
            return View(installment_contract);
        }

        // POST: Installment/Installment_contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Installment_contract installment_contract = db.Installment_contracts.Find(id);
            db.Installment_contracts.Remove(installment_contract);
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
