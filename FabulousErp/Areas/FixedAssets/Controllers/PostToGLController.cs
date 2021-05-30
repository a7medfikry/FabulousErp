using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FixedAssets.Controllers
{
    public class PostToGLController : Controller
    {
        // GET: FixedAssets/PostToGL
        public ActionResult Index()
        {
            return View();
        }
        public string SetMainTransViewBag(string companyID)
        {
            #region detectJEPer
              

            using (FabulousDB.DB_Context.DBContext DB = new FabulousDB.DB_Context.DBContext())
            {
                var detectJEPer =FabulousErp.Business.GetPostingSetup();//  Business.GetPostingSetup();
                //check Journal entry Per Transaction or Batch
                if (detectJEPer != null)
                {
                    ViewBag.FJEPer = "B1";//detectJEPer.CreateJEPer;
                    ViewBag.EPD = detectJEPer.EditPostingDate;
                    if (detectJEPer.CreateJEPer == "B2")
                    {
                        //ViewBag.BatchAction = detectJEPer.ExistingBatch;
                        ViewBag.PostDateType = detectJEPer.PostingDataFrom;
                    }
                }
                else
                {
                    ViewBag.FJEPer = "NoPS";
                }
                ViewBag.PostingToOrThrow = Business.Business.PostingToOrThrow();
               
            }
            ViewBag.FromTCGE = false;
            return null;
            #endregion
        }
    }
}