using FabulousDB.DB_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Controllers
{
    public class Inv_po_adjustmentController : Controller
    {
        // GET: Inventory/Inv_po_adjustment
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            ViewBag.Po = new SelectList(db.Inv_receive_po, "Id", "Gr_num");
            return View();
        }
    }
}