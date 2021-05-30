using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;

namespace FabulousErp.Controllers.Inquiry.Financial.GeneralSetting.UserAccess
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_FactoryAccessController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public Inquiry_FactoryAccessController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();

        // GET: Inquiry_FactoryAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IFA")]
        public ActionResult FactoryAccess()
        {
            ViewBag.CodeList = repetitionBusiness.RetrieveFactoryIDList();
            return View();
        }

        public JsonResult GetData()
        {
            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UAFactoryPremission_Tables.Where(x => x.UserID == userID).FirstOrDefault();
            if (check != null)
            {
                var list = DB.UAFactoryPremission_Tables.ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFactoryAccessData(string factoryID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var check = DB.UAFactoryPremission_Tables.Where(x => x.FactoryID == factoryID && x.UserID == userID).FirstOrDefault();
            if (check != null)
            {
                var list = DB.UAFactoryPremission_Tables.Where(x => x.FactoryID == factoryID).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
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
        //            item.IFA = Value;
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
        //        if (item.IFA.ToString().Equals("True"))
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