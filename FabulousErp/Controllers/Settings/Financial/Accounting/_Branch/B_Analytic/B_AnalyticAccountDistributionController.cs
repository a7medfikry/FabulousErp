using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Analytic;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Important;
using FabulousModels.DTOModels.Settings.Financial.Accounting.Analytic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Branch.B_Analytic
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class B_AnalyticAccountDistributionController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public B_AnalyticAccountDistributionController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: B_AnalyticAccountDistribution
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SBAAD")]
        public ActionResult BranchAnalyticDistribution()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string branchID =null;

            if (companyID != null)
            {
                ViewBag.BranchList = repetitionBusiness.RetrieveBranchIDListCond(companyID);
            }
            else if (branchID != null)
            {
                ViewBag.BranchList = branchID;

                ViewBag.BranchName = repetitionBusiness.GetBranchName(branchID);
            }

            return View();
        }


        public JsonResult GetBranchName(string BranchID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UABranchPremission_Tables.Where(x => x.BranchID == BranchID && x.UserID == userID).FirstOrDefault();

            if (checkAccess != null)
            {
                return Json(repetitionBusiness.GetBranchName(BranchID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FilterAnalyticIDForBranch(string BranchID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            List<Get_Small_Data_DTO> get_Small_Data_DTO = DB.B_AnalyticAccount_Tables.Where(x => x.BranchID == BranchID).Select(x => new Get_Small_Data_DTO
            {

                AnalyticID = x.B_AnalyticAccountID

            }).ToList();

            return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchAnalyticName(string AnalyticID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var getAnalyticName = DB.B_AnalyticAccount_Tables.Where(x => x.B_AnalyticAccountID == AnalyticID).FirstOrDefault();

            if (getAnalyticName != null)
            {
                Get_Small_Data_DTO get_Small_Data_DTO = new Get_Small_Data_DTO()
                {
                    Name = getAnalyticName.B_AnalyticAccountName
                };

                return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public JsonResult SaveDistributionRecordBranch(string BranchAnalyticID, string DistributionID, string DistributionName)
        {

            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicateBranch = DB.B_AnalyticDistribution_Tables.Where(x => x.B_AccountDistributionID == DistributionID && x.B_AnalyticAccountID == BranchAnalyticID).FirstOrDefault();

            if (checkDuplicateBranch != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                B_AnalyticDistribution_Table b_AnalyticDistribution_Table = new B_AnalyticDistribution_Table()
                {
                    B_AnalyticAccountID = BranchAnalyticID,

                    B_AccountDistributionID = DistributionID,

                    B_AccountDistributionName = DistributionName,

                    MoveUserID = userID
                };
                DB.B_AnalyticDistribution_Tables.Add(b_AnalyticDistribution_Table);
                DB.SaveChanges();
                return null;
            }

        }


        public JsonResult GetBranchAnalyticDistributionData(string BranchAnalyticID)
        {
            List<Analytic_Account_Distribution_DTO> analytic_Account_Distribution_DTO = DB.B_AnalyticDistribution_Tables.Where(x => x.B_AnalyticAccountID == BranchAnalyticID).Select(x => new Analytic_Account_Distribution_DTO
            {

                AccountDistributionID = x.B_AccountDistributionID,

                AccountDistributionName = x.B_AccountDistributionName

            }).ToList();

            return Json(analytic_Account_Distribution_DTO, JsonRequestBehavior.AllowGet);
        }



        public JsonResult DeleteAccountDistributionBranch(string AccountDistributionID)
        {
            var deleteDistribution = DB.B_AnalyticDistribution_Tables.Where(x => x.B_AccountDistributionID == AccountDistributionID).FirstOrDefault();

            DB.B_AnalyticDistribution_Tables.Remove(deleteDistribution);
            DB.SaveChanges();

            return null;
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
        //            item.SBAAD = Value;
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
        //        if (item.SBAAD.ToString().Equals("True"))
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