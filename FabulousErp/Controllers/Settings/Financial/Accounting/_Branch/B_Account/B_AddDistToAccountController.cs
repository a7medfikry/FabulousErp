using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CreateAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Branch.B_Account
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class B_AddDistToAccountController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public B_AddDistToAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();


        // GET: B_AddDistToAccount
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SDATBA")]
        public ActionResult DistToBranchAccount()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string branchID = null;

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
            var userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UABranchPremission_Tables.Where(x => x.UserID == userID && x.BranchID == BranchID).FirstOrDefault();

            if (checkAccess != null)
            {

                var getName = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();


                return Json(getName.BranchName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }

        }



        public JsonResult GetAccountDist(string BranchID)
        {
            List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.B_CreateAccount_Tables.Where(x => x.BranchID == BranchID).Select(x => new RetrieveDataToAccount_DTO
            {

                B_AccountID = x.AccountID,

                B_AID = x.B_AID,

                AccountName = x.AccountName

            }).ToList();

            return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetAccountName(int AccountID)
        {
            var getAccountData = DB.B_CreateAccount_Tables.Where(x => x.B_AID == AccountID).FirstOrDefault();

            if (getAccountData != null)
            {
                RetrieveDataToAccount_DTO retrieveDataToAccount_DTO = new RetrieveDataToAccount_DTO()
                {
                    AccountName = getAccountData.AccountName,

                    R_AnalyticAccountID = getAccountData.B_AnalyticAccountID,

                    AccountChartID = getAccountData.AccountChartID
                };

                return Json(retrieveDataToAccount_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        public JsonResult GetDistAccountID(string AnalyticAccountID)
        {
            List<RetrieveDataToAccount_DTO> DataToAccount_DTO = DB.B_AnalyticDistribution_Tables.Where(x => x.B_AnalyticAccountID == AnalyticAccountID).Select(x => new RetrieveDataToAccount_DTO
            {
                DistAccountID = x.B_AccountDistributionID,

                B_DistID = x.B_DistID

            }).ToList();

            return Json(DataToAccount_DTO, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetDistAccountName(int DistAccountID)
        {
            var getName = DB.B_AnalyticDistribution_Tables.Where(x => x.B_DistID == DistAccountID).FirstOrDefault();

            if (getName != null)
            {
                return Json(getName.B_AccountDistributionName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        public JsonResult AddDistributionAccount(int AccountID, string AnalyticAccountID, int DistAccountID, string Percentage, string AccountChartID)
        {

            var checkDuplicate = DB.B_CreatAccountDist_Tables.Where(x => x.B_DistID == DistAccountID && x.B_AID == AccountID && x.B_AnalyticAccountID == AnalyticAccountID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                B_CreatAccountDist_Table b_CreatAccountDist_Table = new B_CreatAccountDist_Table()
                {
                    B_AID = AccountID,

                    B_AnalyticAccountID = AnalyticAccountID,

                    B_DistID = DistAccountID,

                    Percentage = Percentage
                };

                DB.B_CreatAccountDist_Tables.Add(b_CreatAccountDist_Table);
                DB.SaveChanges();

                return null;
            }
        }



        public JsonResult GetDistributionAccounts(int AccountID, string AnalyticAccountID, string AccountChartID)
        {

            List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.B_CreatAccountDist_Tables.Where(x => x.B_AID == AccountID && x.B_AnalyticAccountID == AnalyticAccountID).Select(x => new RetrieveDataToAccount_DTO
            {

                C_DistAccountID = x.B_AnalyticDistribution_Table.B_AccountDistributionID,

                C_DistAccountName = x.B_AnalyticDistribution_Table.B_AccountDistributionName,

                Percentage = x.Percentage,

                ID = x.ID

            }).ToList();

            return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
        }



        public JsonResult DeleteAccountDistributionComp(int AccountDistributionID)
        {
            var deleteDistribution = DB.B_CreatAccountDist_Tables.Where(x => x.ID == AccountDistributionID).FirstOrDefault();

            DB.B_CreatAccountDist_Tables.Remove(deleteDistribution);
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
        //            item.SDATBA = Value;
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
        //        if (item.SDATBA.ToString().Equals("True"))
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