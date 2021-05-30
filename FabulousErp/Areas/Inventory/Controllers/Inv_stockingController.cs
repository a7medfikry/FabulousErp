using FabulousDB.DB_Context;
using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Controllers
{
    public class Inv_stockingController : Controller
    {
        DBContext db = new DBContext();
        // GET: Inventory/Inv_stocking
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(List<Inv_stocking> Stock)
        {
            db.Inv_stocking.AddRange(Stock);
            db.SaveChanges();
            return Json(1);
        }
    }
}