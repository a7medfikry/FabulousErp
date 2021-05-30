using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CreateAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Factory.F_Account
{
    [AuthorizationFilter]
    public class F_AddDistToAccountController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public F_AddDistToAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();


        // GET: F_AddDistToAccount
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SDATFA")]
        public ActionResult DistToFactoryAccount()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string branchID =null;

            string factoryID = null;

            if (companyID != null)
            {
                ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCond(companyID);
            }
            else if (branchID != null)
            {
                ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCondByB(branchID);
            }
            else if (factoryID != null)
            {
                ViewBag.FactoryList = factoryID;

                ViewBag.FactoryName = repetitionBusiness.GetFactoryName(factoryID);
            }
            
            return View();
        }



        public JsonResult GetFactoryName(string FactoryID)
        {
            var userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UAFactoryPremission_Tables.Where(x => x.UserID == userID && x.FactoryID == FactoryID).FirstOrDefault();

            if (checkAccess != null)
            {

                var getName = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();


                return Json(getName.FactoryName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }

        }



        public JsonResult GetAccountDist(string FactoryID)
        {
            List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == FactoryID).Select(x => new RetrieveDataToAccount_DTO
            {

                F_AccountID = x.AccountID,

                F_AID = x.F_AID,

                AccountName = x.AccountName

            }).ToList();

            return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetAccountName(int AccountID)
        {
            var getAccountData = DB.F_CreateAccount_Tables.Where(x => x.F_AID == AccountID).FirstOrDefault();

            if (getAccountData != null)
            {
                RetrieveDataToAccount_DTO retrieveDataToAccount_DTO = new RetrieveDataToAccount_DTO()
                {
                    AccountName = getAccountData.AccountName,

                    R_AnalyticAccountID = getAccountData.F_AnalyticAccountID,

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
            List<RetrieveDataToAccount_DTO> DataToAccount_DTO = DB.F_AnalyticDistribution_Tables.Where(x => x.F_AnalyticAccountID == AnalyticAccountID).Select(x => new RetrieveDataToAccount_DTO
            {
                DistAccountID = x.F_AccountDistributionID,

                F_DistID = x.F_DistID

            }).ToList();

            return Json(DataToAccount_DTO, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetDistAccountName(int DistAccountID)
        {
            var getName = DB.F_AnalyticDistribution_Tables.Where(x => x.F_DistID == DistAccountID).FirstOrDefault();

            if (getName != null)
            {
                return Json(getName.F_AccountDistributionName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        public JsonResult AddDistributionAccount(int AccountID, string AnalyticAccountID, int DistAccountID, string Percentage, string AccountChartID)
        {

            var checkDuplicate = DB.F_CreatAccountDist_Tables.Where(x => x.F_DistID == DistAccountID && x.F_AID == AccountID && x.F_AnalyticAccountID == AnalyticAccountID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                F_CreatAccountDist_Table f_CreatAccountDist_Table = new F_CreatAccountDist_Table()
                {
                    F_AID = AccountID,

                    F_AnalyticAccountID = AnalyticAccountID,

                    F_DistID = DistAccountID,

                    Percentage = Percentage
                };

                DB.F_CreatAccountDist_Tables.Add(f_CreatAccountDist_Table);
                DB.SaveChanges();

                return null;
            }
        }



        public JsonResult GetDistributionAccounts(int AccountID, string AnalyticAccountID, string AccountChartID)
        {

            List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.F_CreatAccountDist_Tables.Where(x => x.F_AID == AccountID && x.F_AnalyticAccountID == AnalyticAccountID).Select(x => new RetrieveDataToAccount_DTO
            {

                C_DistAccountID = x.F_AnalyticDistribution_Table.F_AccountDistributionID,

                C_DistAccountName = x.F_AnalyticDistribution_Table.F_AccountDistributionName,

                Percentage = x.Percentage,

                ID = x.ID

            }).ToList();

            return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
        }



        public JsonResult DeleteAccountDistributionComp(int AccountDistributionID)
        {
            var deleteDistribution = DB.F_CreatAccountDist_Tables.Where(x => x.ID == AccountDistributionID).FirstOrDefault();

            DB.F_CreatAccountDist_Tables.Remove(deleteDistribution);
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
        //            item.SDATFA = Value;
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
        //        if (item.SDATFA.ToString().Equals("True"))
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