using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Factory.Accounting;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Transaction.Financial.Factory.Accounting
{
    [AuthorizationFilter]
    public class F_CreateBatchController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public F_CreateBatchController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();


        // GET: F_CreateBatch
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "TFCB")]
        public ActionResult FactoryCreateBatch()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCond(companyID);

                var checkPS = Business.GetPostingSetup();
                if (checkPS == null)
                {
                    ViewBag.PSExist = false;
                }
            }
            return View();
        }


        public JsonResult GetFactoryName(string factoryID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UAFactoryPremission_Tables.Where(x => x.UserID == userID && x.FactoryID == factoryID).FirstOrDefault();

            if (checkAccess != null)
            {
                return Json(repetitionBusiness.GetFactoryName(factoryID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SaveFactoryBatch(string factoryID, string module, string batchID, string batchDescription)
        {
            var checkDuplicate = DB.F_CreateBatch_Tables.Where(x => x.FactoryID == factoryID && x.F_Module == module && x.F_BatchID == batchID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                F_CreateBatch_Table f_CreateBatch_Table = new F_CreateBatch_Table()
                {
                    FactoryID = factoryID,

                    F_Module = module,

                    F_BatchID = batchID,

                    F_BatchDescription = batchDescription,

                    NotApproval = true
                };
                DB.F_CreateBatch_Tables.Add(f_CreateBatch_Table);
                DB.SaveChanges();
                return null;
            }
        }



        //public JsonResult AddFavourites(bool Value)
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item != null)
        //        {
        //            item.TFCB = Value;
        //            DB.SaveChanges();
        //        }
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult CheckFavourites()
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item.TFCB.ToString().Equals("True"))
        //        {
        //            return Json("True", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("False", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}

    }
}