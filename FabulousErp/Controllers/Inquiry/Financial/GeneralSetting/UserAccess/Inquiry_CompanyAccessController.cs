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
    public class Inquiry_CompanyAccessController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public Inquiry_CompanyAccessController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();

        // GET: Inquiry_CompanyAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ICA")]
        public ActionResult CompanyAccess()
        {
            ViewBag.CodeList = repetitionBusiness.RetrieveCompIDList();
            return View();
        }

        public JsonResult GetData()
        {
            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UACompPremission_Tables.Where(x => x.UserID == userID).FirstOrDefault();
            if (check != null)
            {
                var list = DB.UACompPremission_Tables.ToList();
                return Json(list , JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCompanyAccessData(string companyID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var check = DB.UACompPremission_Tables.Where(x => x.CompanyID == companyID && x.UserID == userID).FirstOrDefault();
            if (check != null)
            {
                var list = DB.UACompPremission_Tables.Where(x=> x.CompanyID == companyID).ToList();
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
        //            item.ICA = Value;
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
        //        if (item.ICA.ToString().Equals("True"))
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