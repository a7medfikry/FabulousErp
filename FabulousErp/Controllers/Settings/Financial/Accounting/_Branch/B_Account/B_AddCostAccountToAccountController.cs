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
    public class B_AddCostAccountToAccountController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public B_AddCostAccountToAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();


        // GET: B_AddCostAccountToAccount
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCATBA")]
        public ActionResult CostAccountToBranchAccount()
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
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UABranchPremission_Tables.Where(x => x.UserID == userID && x.BranchID == BranchID).FirstOrDefault();

            if (checkAccess != null)
            {

                return Json(repetitionBusiness.GetBranchName(BranchID), JsonRequestBehavior.AllowGet);
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

                    CostCenterType = getAccountData.CostCenterType,

                    R_CostCenterID = getAccountData.B_CostCenterID,

                    R_CostCenterGroupID = getAccountData.B_CostCenterGroupID,

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
            List<RetrieveDataToAccount_DTO> DataToAccount_DTO = DB.B_CostCenterAccounts_Tables.Where(x => x.B_CostCenterID == CostCenterID).Select(x => new RetrieveDataToAccount_DTO
            {
                CostAccountID = x.B_CostAccountID,

                C_CAID = x.B_CAID

            }).ToList();

            return Json(DataToAccount_DTO, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetCostAccountName(int CostAccountID)
        {
            var getName = DB.B_CostCenterAccounts_Tables.Where(x => x.B_CAID == CostAccountID).FirstOrDefault();

            if (getName != null)
            {
                return Json(getName.B_CostAccountName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        public JsonResult AddCostAccountTypeCC(int AccountID, string CostCenterID, int CostAccountID, string Percentage, string Type, string AccountChartID)
        {

            var checkDuplicate = DB.B_CreateAccountCCAccount_Tables.Where(x => x.B_AID == AccountID && x.CostCenterType == Type && x.B_CostCenterID == CostCenterID && x.B_CAID == CostAccountID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {

                B_CreateAccountCCAccount_Table b_CreateAccountCCAccount_Table = new B_CreateAccountCCAccount_Table()
                {
                    B_AID = AccountID,

                    CostCenterType = Type,

                    B_CostCenterID = CostCenterID,

                    B_CAID = CostAccountID,

                    Percentage = Percentage
                };
                DB.B_CreateAccountCCAccount_Tables.Add(b_CreateAccountCCAccount_Table);
                DB.SaveChanges();

                return null;
            }
        }



        public JsonResult GetCostAccounts(int AccountID, string CostCenterID, string Type, string AccountChartID)
        {
            List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.B_CreateAccountCCAccount_Tables.Where(x => x.B_AID == AccountID && x.B_CostCenterID == CostCenterID && x.CostCenterType == Type).Select(x => new RetrieveDataToAccount_DTO
            {

                C_CostAccountID = x.B_CostCenterAccounts_Table.B_CostAccountID,

                C_CostAccountName = x.B_CostCenterAccounts_Table.B_CostAccountName,

                Percentage = x.Percentage,

                ID = x.ID

            }).ToList();

            return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
        }



        public JsonResult DeleteCostAccountComp(int CostAccountID)
        {
            var deleteCostAccount = DB.B_CreateAccountCCAccount_Tables.Where(x => x.ID == CostAccountID).FirstOrDefault();

            DB.B_CreateAccountCCAccount_Tables.Remove(deleteCostAccount);
            DB.SaveChanges();

            return null;
        }



        public JsonResult GetMainCostCenterID(string CostCenterGroupID)
        {
            List<RetrieveDataToAccount_DTO> DataToAccount_DTO = DB.B_GroupCostCenter_Tables.Where(x => x.B_CostCenterGroupID == CostCenterGroupID).Select(x => new RetrieveDataToAccount_DTO
            {
                CostCenterID = x.B_CostCenterID,

                GroupID = x.GroupID

            }).ToList();

            return Json(DataToAccount_DTO, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetCostCenterIDData(string CostCenterIDFromMain, string MainCostCenterID)
        {
            var getData = DB.B_GroupCostCenter_Tables.Where(x => x.B_CostCenterID == CostCenterIDFromMain && x.B_CostCenterGroupID == MainCostCenterID).FirstOrDefault();

            if (getData != null)
            {

                RetrieveDataToAccount_DTO DataToAccount_DTO = new RetrieveDataToAccount_DTO()
                {
                    CostCenterName = getData.B_CostCenter_Table.B_CostCenterName,

                    CostCenterPercentage = getData.B_Percentage

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
            var checkDuplicate = DB.B_CreateAccountCCAccount_Tables.Where(x => x.B_AID == AccountID && x.CostCenterType == Type && x.B_CostCenterGroupID == MainCostCenterID && x.B_CostCenterID == CostCenterIDFromMain && x.B_CAID == CostAccountIDFromMain).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                B_CreateAccountCCAccount_Table b_CreateAccountCCAccount_Table = new B_CreateAccountCCAccount_Table()
                {
                    B_AID = AccountID,

                    B_CostCenterGroupID = MainCostCenterID,

                    B_CostCenterID = CostCenterIDFromMain,

                    //B_CCIDFromGroupName = CostCenterNameFromMain,

                    //B_CCIDFromGroupPercentage = PercentageCostCenterID,

                    B_CAID = CostAccountIDFromMain,

                    Percentage = PercentageFromMain,

                    CostCenterType = Type,

                    GroupID = GroupID
                };

                DB.B_CreateAccountCCAccount_Tables.Add(b_CreateAccountCCAccount_Table);
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
        //            item.SCATBA = Value;
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
        //        if (item.SCATBA.ToString().Equals("True"))
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