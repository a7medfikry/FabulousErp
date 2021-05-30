using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Important;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CostCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Company.C_CostCenter
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_UpdateMainCostCenterController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public C_UpdateMainCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();

        // GET: C_UpdateMainCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SUMCC")]
        public ActionResult CompUpdateGroupCC()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            //ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDListCond(companyID);
            ViewBag.CompanyID = companyID;
            ViewBag.CompanyName = repetitionBusiness.GetCompanyName(companyID);

            return View();
        }

        //public JsonResult GetCompanyName(string CompanyID)
        //{
        //    string userID = FabulousErp.Business.GetUserId();

        //    var checkAccess = DB.UACompPremission_Tables.Where(x => x.CompanyID == CompanyID && x.UserID == userID).FirstOrDefault();

        //    if (checkAccess != null)
        //    {
        //        return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("False", JsonRequestBehavior.AllowGet);
        //    }

        //}


        public JsonResult FilterCostCenterGroupIDFrorComp(string CompanyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            List<Get_Small_Data_DTO> get_Small_Data_DTO = DB.C_MainCostCenter_Tables.Where(x => x.CompanyID == CompanyID).Select(x => new Get_Small_Data_DTO
            {

                CostCenterGroupID = x.C_CostCenterGroupID

            }).ToList();

            return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCCGroupIDForComp(string CCGroupID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var getCCGrouprName = DB.C_MainCostCenter_Tables.Where(x => x.C_CostCenterGroupID == CCGroupID).FirstOrDefault();

            if (getCCGrouprName != null)
            {

                return Json(getCCGrouprName.C_CostCenterGroupName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public JsonResult FilterCostCenterIDForComp(string CompanyID=null)
        {
            CompanyID = FabulousErp.Business.GetCompanyId();
            return Json(repetitionBusiness.FilterCostCenterIDForComp(CompanyID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCompCostCenter(string CostCenterID)
        {
            return Json(repetitionBusiness.GetCompCostCenterName(CostCenterID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetGroupCCDataComp(string CompCostCenterGroupID)
        {
            List<Main_Cost_Center_DTO> main_Cost_Center_DTOs = DB.C_GroupCostCenter_Tables.Where(x => x.C_CostCenterGroupID == CompCostCenterGroupID).Select(x => new Main_Cost_Center_DTO
            {

                GroupID = x.GroupID,

                CostCenterID = x.C_CostCenterID,

                CostCenterName = x.C_CostCenter_Table.C_CostCenterName,

                Percentage = x.C_Percentage

            }).ToList();

            return Json(main_Cost_Center_DTOs, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateCompCostGroup(string CompCostCenterGroupID, List<C_GroupCostCenter_Table> order, List<Main_Cost_Center_DTO> deleted)
        {

            //foreach (var asd in order)
            //{
            //    foreach (var ff in asd.C_CreateAccountCCAccount_Tables)
            //    {
            //        ff.GroupID = null;
            //    }
            //}
            //DB.C_GroupCostCenter_Tables.AddRange(order);
            //DB.SaveChanges();

            //List<C_CreateAccountCCAccount_Table> MyAccounts=
            //    DB.C_CreateAccountCCAccount_Tables.Where(x => GroupsMapdToAccount.Any(z=>z.Old_group_id== x.GroupID)).ToList();

            //foreach (GroupsMapdToAccount ThisGroup in GroupsMapdToAccount)
            //{
            //   List<C_CreateAccountCCAccount_Table> ThisMyAccount = MyAccounts.Where(x => x.GroupID == ThisGroup.Old_group_id).ToList();
            //    //DB.C_GroupCostCenter_Tables.Where(x => x.);
            //   ThisMyAccount.ForEach(x => x.GroupID = ThisGroup.Old_group_id);

            //}

            if(deleted != null)
            {
                foreach(var deletedItems in deleted)
                {
                    var deleteChildData = DB.C_CreateAccountCCAccount_Tables.Where(x => x.C_CostCenterGroupID == CompCostCenterGroupID && x.C_CostCenterID == deletedItems.CostCenterID).ToList();
                    if (deleteChildData != null)
                    {
                        DB.C_CreateAccountCCAccount_Tables.RemoveRange(deleteChildData);
                        DB.SaveChanges();
                    }

                    var deleteParentData = DB.C_GroupCostCenter_Tables.Where(x => x.C_CostCenterGroupID == CompCostCenterGroupID && x.C_CostCenterID == deletedItems.CostCenterID).FirstOrDefault();
                    if (deleteParentData != null)
                    {
                        DB.C_GroupCostCenter_Tables.Remove(deleteParentData);
                        DB.SaveChanges();
                    }
                }
            }

            //var deleteOldData = DB.C_GroupCostCenter_Tables.Where(x => x.C_CostCenterGroupID == CompCostCenterGroupID).ToList();
            //DB.C_GroupCostCenter_Tables.RemoveRange(deleteOldData);
            //DB.SaveChanges();

            //var deleteOldDataFromAcc = DB.C_CreateAccountCCAccount_Tables.Where(x => x.C_CostCenterGroupID == CompCostCenterGroupID).ToList();
            //DB.C_CreateAccountCCAccount_Tables.RemoveRange(deleteOldDataFromAcc);
            //DB.SaveChanges();

            string userID = FabulousErp.Business.GetUserId();

            foreach (var item in order)
            {
                var updateData = DB.C_GroupCostCenter_Tables.Where(x => x.C_CostCenterGroupID == CompCostCenterGroupID && x.C_CostCenterID == item.C_CostCenterID).FirstOrDefault();
                if (updateData != null)
                {
                    updateData.C_Percentage = item.C_Percentage;
                    updateData.MoveUserID = userID;
                    DB.SaveChanges();
                }
                else
                {
                    C_GroupCostCenter_Table c_GroupCostCenter_Table = new C_GroupCostCenter_Table()
                    {
                        C_CostCenterGroupID = CompCostCenterGroupID,

                        C_CostCenterID = item.C_CostCenterID,

                        C_Percentage = item.C_Percentage,

                        MoveUserID = userID
                    };
                    DB.C_GroupCostCenter_Tables.Add(c_GroupCostCenter_Table);
                    DB.SaveChanges();
                }
            }

            return Json("True", JsonRequestBehavior.AllowGet);
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
        //            item.SUMCC = Value;
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
        //        if (item.SUMCC.ToString().Equals("True"))
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
    //public class GroupsMapdToAccount
    //{
    //    public int? Old_group_id { get; set; }
    //    public string New_group_name { get; set; }
    //    public List<int> Accounts { get; set; }
    //}
}