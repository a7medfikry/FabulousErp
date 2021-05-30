using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccess;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.UserAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.UserAccess
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class CompanyPremissionAccessController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public CompanyPremissionAccessController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();
        
        // GET: CompanyPremissionAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SUACP")]
        public ActionResult UACompPremission()
        {
            ViewBag.UserList = repetitionBusiness.RetrieveUserIDList();
            
            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDList();

            
            return View();
        }

        public JsonResult GetUserName(string UserID) {

            return Json(repetitionBusiness.GetUserName(UserID),JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetCompanyName(string CompanyID)
        {
            
            return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);

        }

        public JsonResult AddUserToComp(string UserID, string UserName, string CompanyID, string CompanyName)
        {

            var Check = DB.UACompPremission_Tables.Where(x => x.UserID == UserID && x.CompanyID == CompanyID).FirstOrDefault();

            if (Check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                UACompPremission_Table uACompPremission_Table = new UACompPremission_Table()
                {
                    UserID = UserID,

                    UserName = UserName,

                    CompanyID = CompanyID,

                    CompanyName = CompanyName
                };

                DB.UACompPremission_Tables.Add(uACompPremission_Table);
                DB.SaveChanges();

                return Json(JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetUserCompAccess(string UserID)
        {
            List<UA_Comp_Premission_DTO> uACompPremission = DB.UACompPremission_Tables.Where(x => x.UserID == UserID).Select(x => new UA_Comp_Premission_DTO
            {
                UserID = x.UserID,

                CompanyID = x.CompanyID,

                UserName = x.CreateAccount_Table.UserName,

                CompanyName = x.CompanyMainInfo_Table.CompanyName
            }).ToList();

            return Json(uACompPremission, JsonRequestBehavior.AllowGet);
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
        //            item.SUACP = Value;
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
        //        if (item.SUACP.ToString().Equals("True"))
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