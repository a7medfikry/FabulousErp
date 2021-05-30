using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Branch.Accounting;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Transaction.Financial.Branch.Accounting
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class B_CreateBatchController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public B_CreateBatchController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: B_CreateBatch
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "TBCB")]
        public ActionResult BranchCreateBatch()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                ViewBag.BranchList = repetitionBusiness.RetrieveBranchIDListCond(companyID);

                var checkPS = Business.GetPostingSetup();
                if (checkPS == null)
                {
                    ViewBag.PSExist = false;
                }
            }
            return View();
        }


        public JsonResult GetBranchName(string branchID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UABranchPremission_Tables.Where(x => x.UserID == userID && x.BranchID == branchID).FirstOrDefault();

            if (checkAccess != null)
            {
                return Json(repetitionBusiness.GetBranchName(branchID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SaveBranchBatch(string branchID, string module, string batchID, string batchDescription)
        {
            var checkDuplicate = DB.B_CreateBatch_Tables.Where(x => x.BranchID == branchID && x.B_Module == module && x.B_BatchID == batchID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                B_CreateBatch_Table b_CreateBatch_Table = new B_CreateBatch_Table()
                {
                    BranchID = branchID,

                    B_Module = module,

                    B_BatchID = batchID,

                    B_BatchDescription = batchDescription,

                    NotApproval = true
                };
                DB.B_CreateBatch_Tables.Add(b_CreateBatch_Table);
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
        //            item.TBCB = Value;
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
        //        if (item.TBCB.ToString().Equals("True"))
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