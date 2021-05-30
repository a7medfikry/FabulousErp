using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccess;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
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
    public class FactoryPremissionAccessController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public FactoryPremissionAccessController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: FactoryPremissionAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SUAFP")]
        public ActionResult UAFactoryPremission()
        {

            ViewBag.UserList = repetitionBusiness.RetrieveUserIDList();

            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDList();

            return View();
        }

        public JsonResult GetUserName(string UserID)
        {
            return Json(repetitionBusiness.GetUserName(UserID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyName(string CompanyID)
        {
            return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterBranchID(string CompanyID)
        {
            return Json(repetitionBusiness.FilterBranchID(CompanyID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchName(string BranchID)
        {
            return Json(repetitionBusiness.GetBranchName(BranchID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult FilterFactoryID(string BranchID, string CompanyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var BranchList = DB.CompanyFactoryInfo_Tables.Where(x => x.CompanyID == CompanyID && x.BranchID == BranchID).ToList();

            if (BranchList != null)
            {
                return Json(BranchList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FilterFactoryID2(string CompanyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var BranchList = DB.CompanyFactoryInfo_Tables.Where(x => x.CompanyID == CompanyID && x.BranchID == null).ToList();

            if (BranchList != null)
            {
                return Json(BranchList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFactoryName(string FactoryID)
        {
            return Json(repetitionBusiness.GetFactoryName(FactoryID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserFactoryAccess(string UserID)
        {

            List<UAFactoryPremission_Table> uAFactoryPremission_Tables = new List<UAFactoryPremission_Table>();

            uAFactoryPremission_Tables = DB.UAFactoryPremission_Tables.Where(x => x.UserID == UserID).ToList();

            if (uAFactoryPremission_Tables != null)
            {
                return Json(uAFactoryPremission_Tables, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult AddUserToFactory(string UserID, string UserName, string CompanyID, string CompanyName, string BranchID, string BarnchName, string FactoryID, string FactoryName)
        {

            if (BranchID == "-1")
            {

                var Check = DB.UAFactoryPremission_Tables.Where(x => x.UserID == UserID && x.CompanyID == CompanyID && x.FactoryID == FactoryID).FirstOrDefault();

                if (Check != null)
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
                else
                {

                    UAFactoryPremission_Table uAFactory = new UAFactoryPremission_Table()
                    {
                        UserID = UserID,

                        UserName = UserName,

                        CompanyID = CompanyID,

                        CompanyName = CompanyName,

                        BranchID = null,

                        BranchName = BarnchName,

                        FactoryID = FactoryID,

                        FactoryName = FactoryName
                    };

                    DB.UAFactoryPremission_Tables.Add(uAFactory);
                    DB.SaveChanges();

                    return Json(JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var Check = DB.UAFactoryPremission_Tables.Where(x => x.UserID == UserID && x.CompanyID == CompanyID && x.FactoryID == FactoryID && x.BranchID == BranchID).FirstOrDefault();

                if (Check != null)
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    UAFactoryPremission_Table uAFactory = new UAFactoryPremission_Table()
                    {
                        UserID = UserID,

                        UserName = UserName,

                        CompanyID = CompanyID,

                        CompanyName = CompanyName,

                        BranchID = BranchID,

                        BranchName = BarnchName,

                        FactoryID = FactoryID,

                        FactoryName = FactoryName
                    };

                    DB.UAFactoryPremission_Tables.Add(uAFactory);
                    DB.SaveChanges();

                    return Json(JsonRequestBehavior.AllowGet);
                }
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
        //            item.SUAFP = Value;
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
        //        if (item.SUAFP.ToString().Equals("True"))
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