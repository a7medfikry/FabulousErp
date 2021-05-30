using FabulousDB.DB_Context;
using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FixedAssets.Controllers
{
    public class DeletedAssetsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: FixedAssets/DeletedAssets
        public ActionResult DeleteAssets()
        {
            return View(db.Deleted_assets.ToList());
        }
        public ActionResult DeleteAssetsClass()
        {
            return View(db.Deleted_assets_class.ToList());

        }
        public ActionResult DeleteAssetsMain()
        {
            return View(db.Delete_assets_main.ToList());
        }

        public ActionResult DeleteRenwal()
        {
            return View(db.Deleted_fixed_assets_renewal.ToList());
        }
        public ActionResult Deletedisposel()
        {
            return View(db.Delete_fixed_assets_disposel.ToList());

        }
        public ActionResult DeleteRevaluate()
        {
            return View(db.Delete_fixed_assets_revaluate.ToList());
        }
    }
}