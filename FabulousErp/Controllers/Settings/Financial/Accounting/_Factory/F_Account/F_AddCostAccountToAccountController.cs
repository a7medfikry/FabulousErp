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
    public class F_AddCostAccountToAccountController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public F_AddCostAccountToAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();


        // GET: F_AddCostAccountToAccount
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCATFA")]
        public ActionResult CostAccountToFactoryAccount()
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
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UAFactoryPremission_Tables.Where(x => x.UserID == userID && x.FactoryID == FactoryID).FirstOrDefault();

            if (checkAccess != null)
            {

                return Json(repetitionBusiness.GetFactoryName(FactoryID), JsonRequestBehavior.AllowGet);
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

                    CostCenterType = getAccountData.CostCenterType,

                    R_CostCenterID = getAccountData.F_CostCenterID,

                    R_CostCenterGroupID = getAccountData.F_CostCenterGroupID,

                    AccountChartID = getAccountData.AccountChartID
                };

                return Json(retrieveDataToAccount_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        public JsonResult GetCostCenterAccounts(string CostCenterID)
        {
            List<RetrieveDataToAccount_DTO> DataToAccount_DTO = DB.F_CostCenterAccounts_Tables.Where(x => x.F_CostCenterID == CostCenterID).Select(x => new RetrieveDataToAccount_DTO
            {
                CostAccountID = x.F_CostAccountID,

                C_CAID = x.F_CAID

            }).ToList();

            return Json(DataToAccount_DTO, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetCostAccountName(int CostAccountID)
        {
            var getName = DB.F_CostCenterAccounts_Tables.Where(x => x.F_CAID == CostAccountID).FirstOrDefault();

            if (getName != null)
            {
                return Json(getName.F_CostAccountName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        public JsonResult AddCostAccountTypeCC(int AccountID, string CostCenterID, int CostAccountID, string Percentage, string Type, string AccountChartID)
        {

            var checkDuplicate = DB.F_CreateAccountCCAccount_Tables.Where(x => x.F_AID == AccountID && x.CostCenterType == Type && x.F_CostCenterID == CostCenterID && x.F_CAID == CostAccountID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {

                F_CreateAccountCCAccount_Table f_CreateAccountCCAccount_Table = new F_CreateAccountCCAccount_Table()
                {
                    F_AID = AccountID,

                    CostCenterType = Type,

                    F_CostCenterID = CostCenterID,

                    F_CAID = CostAccountID,

                    Percentage = Percentage
                };
                DB.F_CreateAccountCCAccount_Tables.Add(f_CreateAccountCCAccount_Table);
                DB.SaveChanges();

                return null;
            }
        }



        public JsonResult GetCostAccounts(int AccountID, string CostCenterID, string Type, string AccountChartID)
        {
            List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.F_CreateAccountCCAccount_Tables.Where(x => x.F_AID == AccountID && x.F_CostCenterID == CostCenterID && x.CostCenterType == Type).Select(x => new RetrieveDataToAccount_DTO
            {

                C_CostAccountID = x.F_CostCenterAccounts_Table.F_CostAccountID,

                C_CostAccountName = x.F_CostCenterAccounts_Table.F_CostAccountName,

                Percentage = x.Percentage,

                ID = x.ID

            }).ToList();

            return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
        }



        public JsonResult DeleteCostAccountComp(int CostAccountID)
        {
            var deleteCostAccount = DB.F_CreateAccountCCAccount_Tables.Where(x => x.ID == CostAccountID).FirstOrDefault();

            DB.F_CreateAccountCCAccount_Tables.Remove(deleteCostAccount);
            DB.SaveChanges();

            return null;
        }



        public JsonResult GetMainCostCenterID(string CostCenterGroupID)
        {
            List<RetrieveDataToAccount_DTO> DataToAccount_DTO = DB.F_GroupCostCenter_Tables.Where(x => x.F_CostCenterGroupID == CostCenterGroupID).Select(x => new RetrieveDataToAccount_DTO
            {
                CostCenterID = x.F_CostCenterID,

                GroupID = x.GroupID

            }).ToList();

            return Json(DataToAccount_DTO, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetCostCenterIDData(string CostCenterIDFromMain, string MainCostCenterID)
        {
            var getData = DB.F_GroupCostCenter_Tables.Where(x => x.F_CostCenterID == CostCenterIDFromMain && x.F_CostCenterGroupID == MainCostCenterID).FirstOrDefault();

            if (getData != null)
            {

                RetrieveDataToAccount_DTO DataToAccount_DTO = new RetrieveDataToAccount_DTO()
                {
                    CostCenterName = getData.F_CostCenter_Table.F_CostCenterName,

                    CostCenterPercentage = getData.F_Percentage

                };

                return Json(DataToAccount_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        public JsonResult AddCostAccountTypeMCC(int AccountID, string MainCostCenterID, string CostCenterIDFromMain, string CostCenterNameFromMain, string PercentageCostCenterID, int CostAccountIDFromMain, string PercentageFromMain, string Type, string AccountChartID, int GroupID)
        {
            var checkDuplicate = DB.F_CreateAccountCCAccount_Tables.Where(x => x.F_AID == AccountID && x.CostCenterType == Type && x.F_CostCenterGroupID == MainCostCenterID && x.F_CostCenterID == CostCenterIDFromMain && x.F_CAID == CostAccountIDFromMain).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                F_CreateAccountCCAccount_Table f_CreateAccountCCAccount_Table = new F_CreateAccountCCAccount_Table()
                {
                    F_AID = AccountID,

                    F_CostCenterGroupID = MainCostCenterID,

                    F_CostCenterID = CostCenterIDFromMain,

                    //F_CCIDFromGroupName = CostCenterNameFromMain,

                    //F_CCIDFromGroupPercentage = PercentageCostCenterID,

                    F_CAID = CostAccountIDFromMain,

                    Percentage = PercentageFromMain,

                    CostCenterType = Type,

                    GroupID = GroupID
                };

                DB.F_CreateAccountCCAccount_Tables.Add(f_CreateAccountCCAccount_Table);
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
        //            item.SCATFA = Value;
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
        //        if (item.SCATFA.ToString().Equals("True"))
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