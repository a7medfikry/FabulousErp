using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.Models;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CreateAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using FabulousModels.Inventory;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Company.C_Account
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_AddCostAccountToAccountController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public C_AddCostAccountToAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: C_AddCostAccountToAccount
        [HttpGet]
        // [Authorize]
        [CustomAuthorize(Roles = "SCATCA")]
        public ActionResult CostAccountToCompAccount()
        {
            return View();
        }
        [HttpGet]
        // [Authorize]
        [CustomAuthorize(Roles = "SCATCA")]
        public ActionResult ConnectCostCenterToAccount()
        {
            return View();
        }

        public ActionResult CostCenterAccounts(string CCId)
        {
            if (CCId != "-1")
            {
                List<CostCenterAccount> CostCenterAccounts =
             DB.C_CreateAccount_Tables
             .Include(x => x.Cost_center)
             .Select(x => new CostCenterAccount
             {
                 C_aid = x.C_AID,
                 Account_name = x.AccountName,
                 Cost_center = CCId,
                 Exist = x.Cost_center.Any(z => z.Cost_center_id == CCId && z.Account_id == x.C_AID)
             })
             .ToList();

               
                CostCenterAccounts.RemoveAll(x =>
                DB.Cost_center_accounts.Any(z => z.Account_id == x.C_aid && z.Cost_center_id!= CCId)
                ||
                DB.Groupcostcenter_accounts.Any(z => z.Account_id == x.C_aid)
                );
                return View(CostCenterAccounts);
            }
            else
            {
                return View(new List<CostCenterAccount> { });
            }
        } 
        
        public ActionResult CostCenterGroupAccounts(int GroupId)
        {
            if (GroupId != -1)
            {
                List<CostCenterAccount> CostCenterAccounts =
             DB.C_CreateAccount_Tables
             .Include(x => x.Groupcostcenter_accounts)
             .Select(x => new CostCenterAccount
             {
                 C_aid = x.C_AID,
                 Account_name = x.AccountName,
                 Cost_center = GroupId.ToString(),
                 Exist = x.Groupcostcenter_accounts.Any(z => z.Group_id == GroupId && z.Account_id == x.C_AID)
             })
             .ToList();

                CostCenterAccounts.RemoveAll(x => 
                 DB.Cost_center_accounts.Any(z => z.Account_id == x.C_aid)
                 ||
                  DB.Groupcostcenter_accounts.Any(z => z.Account_id == x.C_aid && z.Group_id != GroupId)
                );


                return View("CostCenterAccounts",CostCenterAccounts);
            }
            else
            {
                return View("CostCenterAccounts", new List<CostCenterAccount> { });
            }
        }
        public JsonResult GetCostCenterName(string Id)
        {
            try
            {
                return Json(DB.C_CostCenter_Tables.Find(Id).C_CostCenterName);
            }
            catch
            {
                return Json("");
            }
        }
        public JsonResult GetCostGroups()
        {
            try
            {
                return Json(DB.C_GroupCostCenter_Tables.Select(x => new { x.GroupID }));
            }
            catch
            {
                return Json("");
            }
        } 
        public JsonResult GetCostGroupName(int GroupId)
        {
            try
            {
                return Json(DB.C_GroupCostCenter_Tables.Find(GroupId).C_CostCenterGroupID);
            }
            catch
            {
                return Json("");
            }
        }
        public JsonResult AddAccountToCostCenter(string CCID, int CAID,bool Exist)
        {
            if (Exist)
            {
                DB.Cost_center_accounts.Add(new Cost_center_accounts
                {
                    Account_id = CAID,
                    Cost_center_id = CCID,
                });
            }
            else
            {
                DB.Cost_center_accounts.RemoveRange(DB.Cost_center_accounts.Where(x => 
                x.Account_id == CAID&&x.Cost_center_id == CCID).ToList());
            }
           
            DB.SaveChanges();
            return Json(1);
        } 
        public JsonResult AddAccountToCostCenterGroup(int CCID, int CAID,bool Exist)
        {
            if (Exist)
            {
                DB.Groupcostcenter_accounts.Add(new Groupcostcenter_accounts
                {
                    Account_id = CAID,
                    Group_id = CCID,
                });
            }
            else
            {
                DB.Groupcostcenter_accounts.RemoveRange(DB.Groupcostcenter_accounts.Where(x => 
                x.Account_id == CAID&&x.Group_id == CCID).ToList());
            }
           
            DB.SaveChanges();
            return Json(1);
        }
        public JsonResult GetAccountDist()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.C_Prefix == null).Select(x => new RetrieveDataToAccount_DTO
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
                    CostCenterType =getAccountData.CostCenterType,
                    R_CostCenterID = getAccountData.C_CostCenterID,
                    R_CostCenterGroupID = getAccountData.C_CostCenterGroupID,
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
            List<RetrieveDataToAccount_DTO> DataToAccount_DTO = DB.C_CostCenterAccounts_Tables.Where(x => x.C_CostCenterID == CostCenterID).Select(x => new RetrieveDataToAccount_DTO
            {
                CostAccountID = x.C_CostAccountID,

                C_CAID = x.C_CAID

            }).ToList();

            return Json(DataToAccount_DTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCostAccountName(int CostAccountID)
        {
            var getName = DB.C_CostCenterAccounts_Tables.Where(x => x.C_CAID == CostAccountID).FirstOrDefault();

            if (getName != null)
            {
                return Json(getName.C_CostAccountName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public JsonResult AddCostAccountTypeCC(int AccountID, string CostCenterID, int CostAccountID, string Percentage, string Type, string AccountChartID)
        {

            var checkDuplicate = DB.C_CreateAccountCCAccount_Tables.Where(x => x.C_AID == AccountID && x.CostCenterType == Type && x.C_CostCenterID == CostCenterID && x.C_CAID == CostAccountID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {

                C_CreateAccountCCAccount_Table c_CreateAccountCCAccount_Table = new C_CreateAccountCCAccount_Table()
                {
                    C_AID = AccountID,

                    CostCenterType = Type,

                    C_CostCenterID = CostCenterID,

                    C_CAID = CostAccountID,

                    Percentage = Percentage
                };
                DB.C_CreateAccountCCAccount_Tables.Add(c_CreateAccountCCAccount_Table);
                DB.SaveChanges();

                return null;
            }
        }


        public JsonResult GetCostAccounts(int AccountID, string CostCenterID, string Type, string AccountChartID)
        {
            List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.C_CreateAccountCCAccount_Tables.Where(x => x.C_AID == AccountID && x.C_CostCenterID == CostCenterID && x.CostCenterType == Type).Select(x => new RetrieveDataToAccount_DTO
            {

                C_CostAccountID = x.C_CostCenterAccounts_Table.C_CostAccountID,

                C_CostAccountName = x.C_CostCenterAccounts_Table.C_CostAccountName,

                Percentage = x.Percentage,

                ID = x.ID

            }).ToList();

            return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteCostAccountComp(int CostAccountID)
        {
            var deleteCostAccount = DB.C_CreateAccountCCAccount_Tables.Where(x => x.ID == CostAccountID).FirstOrDefault();

            DB.C_CreateAccountCCAccount_Tables.Remove(deleteCostAccount);
            DB.SaveChanges();

            return null;
        }


        public JsonResult GetMainCostCenterID(string CostCenterGroupID)
        {
            List<RetrieveDataToAccount_DTO> DataToAccount_DTO = DB.C_GroupCostCenter_Tables.Where(x => x.C_CostCenterGroupID == CostCenterGroupID).Select(x => new RetrieveDataToAccount_DTO
            {
                CostCenterID = x.C_CostCenterID,

                GroupID = x.GroupID

            }).ToList();

            return Json(DataToAccount_DTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCostCenterIDData(string CostCenterIDFromMain, string MainCostCenterID)
        {
            var getData = DB.C_GroupCostCenter_Tables.Where(x => x.C_CostCenterID == CostCenterIDFromMain && x.C_CostCenterGroupID == MainCostCenterID).FirstOrDefault();

            if (getData != null)
            {

                RetrieveDataToAccount_DTO DataToAccount_DTO = new RetrieveDataToAccount_DTO()
                {
                    CostCenterName = getData.C_CostCenter_Table.C_CostCenterName,

                    CostCenterPercentage = getData.C_Percentage

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
            var checkDuplicate = DB.C_CreateAccountCCAccount_Tables.Where(x => x.C_AID == AccountID && x.CostCenterType == Type && x.C_CostCenterGroupID == MainCostCenterID && x.C_CostCenterID == CostCenterIDFromMain && x.C_CAID == CostAccountIDFromMain).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                C_CreateAccountCCAccount_Table c_CreateAccountCCAccount_Table = new C_CreateAccountCCAccount_Table()
                {
                    C_AID = AccountID,

                    C_CostCenterGroupID = MainCostCenterID,

                    C_CostCenterID = CostCenterIDFromMain,

                    //C_CCIDFromGroupName = CostCenterNameFromMain,

                    //C_CCIDFromGroupPercentage = PercentageCostCenterID,

                    C_CAID = CostAccountIDFromMain,

                    Percentage = PercentageFromMain,

                    CostCenterType = Type,

                    GroupID = GroupID
                };

                DB.C_CreateAccountCCAccount_Tables.Add(c_CreateAccountCCAccount_Table);
                DB.SaveChanges();

                return null;
            }
        }

        //public ActionResult AssigAccountParent()
        //{

        //   return View(DB.C_CreateAccountCCAccount_Tables.Where(x=>x.CostCenterType== "MainCostCenter").FirstOrDefault(x => x.GroupID == null));
        //}
        //[HttpPost]
        //public ActionResult AssigAccountParent(C_CreateAccountCCAccount_Table ThisCostTable)
        //{
        //    DB.Entry(ThisCostTable).State = System.Data.Entity.EntityState.Modified;
        //    DB.SaveChanges();
        //    return RedirectToAction("AssigAccountParent");
        //}
        //public static IEnumerable<SelectListItem> GetCostCenterGroupId()
        //{
        //    DBContext DB = new DBContext();
        //    IEnumerable<SelectListItem> GroupID = DB.C_GroupCostCenter_Tables.Select(x => new SelectListItem
        //    {
        //        Text = x.C_CostCenterID,
        //        Value = x.GroupID.ToString()
        //    }).ToList();
        //    return GroupID;
        //}

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
        //            item.SCATCA = Value;
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
        //        if (item.SCATCA.ToString().Equals("True"))
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