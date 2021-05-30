using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CreateAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Company.C_Account
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_AddDistToAccountController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public C_AddDistToAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: AddDistToCAccount
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SDATCA")]
        public ActionResult DistToCompAccount()
        {
            return View();
        }


        public JsonResult GetAccountDist()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).Select(x => new RetrieveDataToAccount_DTO
            {
                C_AccountID = x.AccountID,

                C_AID = x.C_AID,

                AccountName = x.AccountName

            }).ToList();

            return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAccountName(int AccountID)
        {
            var getAccountData = DB.C_CreateAccount_Tables.Where(x => x.C_AID == AccountID).FirstOrDefault();

            if (getAccountData != null)
            {
                RetrieveDataToAccount_DTO retrieveDataToAccount_DTO = new RetrieveDataToAccount_DTO()
                {
                    AccountName = getAccountData.AccountName,

                    R_AnalyticAccountID = getAccountData.C_AnalyticAccountID,

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
            List<RetrieveDataToAccount_DTO> DataToAccount_DTO = DB.C_AnalyticDistribution_Tables.Where(x => x.C_AnalyticAccountID == AnalyticAccountID).Select(x => new RetrieveDataToAccount_DTO
            {
                DistAccountID = x.C_AccountDistributionID,

                C_DistID = x.C_DistID

            }).ToList();

            return Json(DataToAccount_DTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDistAccountName(int DistAccountID)
        {
            var getName = DB.C_AnalyticDistribution_Tables.Where(x => x.C_DistID == DistAccountID).FirstOrDefault();

            if(getName != null)
            {
                return Json(getName.C_AccountDistributionName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public JsonResult AddDistributionAccount(int AccountID, string AnalyticAccountID, int DistAccountID, string Percentage, string AccountChartID)
        {

            var checkDuplicate = DB.C_CreatAccountDist_Tables.Where(x => x.C_DistID == DistAccountID && x.C_AID == AccountID && x.C_AnalyticAccountID == AnalyticAccountID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                C_CreatAccountDist_Table c_CreatAccountDist_Table = new C_CreatAccountDist_Table()
                {
                    C_AID = AccountID,

                    C_AnalyticAccountID = AnalyticAccountID,

                    C_DistID = DistAccountID,

                    Percentage = Percentage
                };

                DB.C_CreatAccountDist_Tables.Add(c_CreatAccountDist_Table);
                DB.SaveChanges();

                return null;
            }
        }


        public JsonResult GetDistributionAccounts(int AccountID, string AnalyticAccountID, string AccountChartID)
        {

            List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.C_CreatAccountDist_Tables.Where(x => x.C_AID == AccountID && x.C_AnalyticAccountID == AnalyticAccountID).Select(x => new RetrieveDataToAccount_DTO
            {

                C_DistAccountID = x.C_AnalyticDistribution_Table.C_AccountDistributionID,

                C_DistAccountName = x.C_AnalyticDistribution_Table.C_AccountDistributionName,

                Percentage = x.Percentage,

                ID = x.ID

            }).ToList();

            return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteAccountDistributionComp(int AccountDistributionID)
        {
            var deleteDistribution = DB.C_CreatAccountDist_Tables.Where(x => x.ID == AccountDistributionID).FirstOrDefault();

            DB.C_CreatAccountDist_Tables.Remove(deleteDistribution);
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
        //            item.SDATCA = Value;
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
        //        if (item.SDATCA.ToString().Equals("True"))
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