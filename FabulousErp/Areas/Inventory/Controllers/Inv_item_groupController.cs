using FabulousDB.DB_Context;
using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Controllers
{
    public class Inv_item_groupController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_item_group
        public ActionResult Index()
        {
            var inv_item_group = db.Inv_item_group.Include(i => i.Unit_of_measure);
            return View(inv_item_group.ToList());
        }

        // GET: Inventory/Inv_item_group/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item_group inv_item_group = db.Inv_item_group.Find(id);
            if (inv_item_group == null)
            {
                return HttpNotFound();
            }
            return View(inv_item_group);
        }

        // GET: Inventory/Inv_item_group/Create
        public ActionResult Create(int? Id)
        {
            Inv_item_group I = db.Inv_item_group
                .Where(x => x.Id == Id).Include(x => x.Deduct_tax).ToList()
                .DefaultIfEmpty(new Inv_item_group { }).FirstOrDefault();
            ViewBag.Validation_method = new SelectList(Enum.GetValues(typeof(Validation_method))
                .Cast<Validation_method>().Select(x => new { Text = x.ToString(), Value = (int)x })
                , "Value", "Text", (int)I.Validation_method);
            ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id");

            ViewBag.Vat_Item_type = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = FabulousErp.BusController.Translate("3- Material") , Value = "3" },
                new SelectListItem{Text =FabulousErp.BusController.Translate( "4- Service") , Value = "4" },
                new SelectListItem{Text = FabulousErp.BusController.Translate("5- Equipment") , Value = "5" },
                new SelectListItem{Text = FabulousErp.BusController.Translate("6- Parts of Equipment") , Value = "6" },
                new SelectListItem{Text = FabulousErp.BusController.Translate("7- Exemptions") , Value = "7" }
            }, "Value", "Text", I.Vat_Item_type);

            ViewBag.Tax_type_id = new SelectList(new List<SelectListItem>
                                     {
                                         new SelectListItem{Text = FabulousErp.BusController.Translate("1- General") , Value = "1" },
                                         new SelectListItem{Text = FabulousErp.BusController.Translate("2- Tax Table") , Value = "2" }
                                     }, "Value", "Text", I.Tax_type_id);
            ViewBag.Tax_table_type_id = new SelectList(new List<SelectListItem>
                                     {
                                         new SelectListItem{Text =FabulousErp.BusController.Translate("0- None") , Value = "0" },
                                         new SelectListItem{Text = FabulousErp.BusController.Translate("1- Table First") , Value = "1" },
                                         new SelectListItem{Text = FabulousErp.BusController.Translate("2- Table Second") , Value = "2" }
                                     }, "Value", "Text", I.Tax_table_type_id);

            ViewBag.Cost_center_id = new SelectList(db.C_CostCenter_Tables.ToList(), "C_CostCenterID", "C_CostCenterName", I.Cost_center_id);

            return View(I);
        }

        // POST: Inventory/Inv_item_group/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inv_item_group inv_item_group, List<int> DeductTaxIds)
        {
            if (DeductTaxIds == null)
            {
                DeductTaxIds = new List<int>();
            }
            //if (ModelState.IsValid)
            if (inv_item_group.Id == 0)
            {
                db.Inv_item_group.Add(inv_item_group);
                db.SaveChanges();
                foreach (int i in DeductTaxIds.Where(x => x != 0).ToList())
                {
                    db.Inv_item_group_deduct_tax.Add(new Inv_item_group_deduct_tax
                    {
                        Deduct_id = i,
                        item_group_id = inv_item_group.Id
                    });
                }
                db.SaveChanges();
            }
            else
            {
                db.Entry(inv_item_group).State = EntityState.Modified;

                db.SaveChanges();
                db.Inv_item_group_deduct_tax.RemoveRange(db.Inv_item_group_deduct_tax.Where(x => x.item_group_id == inv_item_group.Id));
                foreach (int i in DeductTaxIds.Where(x => x != 0).ToList())
                {
                    db.Inv_item_group_deduct_tax.Add(new Inv_item_group_deduct_tax
                    {
                        Deduct_id = i,
                        item_group_id = inv_item_group.Id
                    });
                }
                db.SaveChanges();
            }
            return Json(inv_item_group.Id);

            //ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_item_group.Unit_of_measure_id);
            //return View(inv_item_group);
        }
        public JsonResult GetTaxGroup(int GorupId)
        {
            return Json(db.Inv_item_group.Include(x => x.Deduct_tax).Where(x => x.Id == GorupId).Select(x => new
            {
                Deduct_tax = x.Deduct_tax.Select(z => new { z.Deduct_id }).ToList(),
                Tax_table_type_id = x.Tax_table_type_id,
                Tax_type_id = x.Tax_type_id,
                Vat_id = x.Vat_id,
                Tbl_vat_Id = x.Tbl_vat_Id,
                Type = x.Type,

                CostCenter = x.Cost_center_id,
                CostCenterAccount = x.Cost_center_account_id
            }).FirstOrDefault());
        }
        public JsonResult GetProp(int GorupId)
        {
            return Json(db.Inv_item_group.Where(x => x.Id == GorupId).Select(x => new
            {
                Valution = x.Validation_method,
                MOS = x.Martial_or_service,
                Has_warranty = x.Has_warranty,
                Has_expiery = x.Has_expiry_date,
                Has_serial = x.Has_serial,
                UOM = x.Unit_of_measure_id
            }).FirstOrDefault());
        }
        // GET: Inventory/Inv_item_group/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item_group inv_item_group = db.Inv_item_group.Find(id);
            if (inv_item_group == null)
            {
                return HttpNotFound();
            }
            ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_item_group.Unit_of_measure_id);
            return View(inv_item_group);
        }

        // POST: Inventory/Inv_item_group/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Item_group_id,Desc,Type,Unit_of_measure_id,Sales_tax_group_id,Purchase_tax_group_id")] Inv_item_group inv_item_group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_item_group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_item_group.Unit_of_measure_id);
            return View(inv_item_group);
        }

        // GET: Inventory/Inv_item_group/Delete/5
        public ActionResult Delete(int? id, string Msg = "")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item_group inv_item_group = db.Inv_item_group.Find(id);
            if (inv_item_group == null)
            {
                return HttpNotFound();
            }
            ViewBag.Msg = Msg;
            return View(inv_item_group);
        }

        // POST: Inventory/Inv_item_group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Inv_item_group inv_item_group = db.Inv_item_group.Include(x => x.Inv_group_gl).FirstOrDefault(x => x.Id == id);
                db.Inv_gorup_gl_accounts.RemoveRange(inv_item_group.Inv_group_gl);
                db.Inv_item_group.Remove(inv_item_group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete", new { Msg = "This Group Has Items You Can't Delete It" });

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
