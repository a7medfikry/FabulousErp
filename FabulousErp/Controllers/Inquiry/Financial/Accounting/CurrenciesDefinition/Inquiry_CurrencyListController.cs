using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting.CurrenciesDefinition
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_CurrencyListController : Controller
    {
        DBContext DB = new DBContext();

        // GET: Inquiry_CurrencyList
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ILOC")]
        public ActionResult CurrencyList()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.CompanyID = companyID;
            string companyName = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == companyID).Select(x => x.CompanyName).First().ToString();
            ViewBag.CompanyName = companyName;
            return View();
        }

        public JsonResult GetCompanyCurrency(string companyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;
            var userID = FabulousErp.Business.GetUserId();
            var check = DB.UACompPremission_Tables.Where(x => x.CompanyID == companyID && x.UserID == userID).FirstOrDefault();
            if (check != null)
            {
                var list = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
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
        //            item.ILOC = Value;
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
        //        if (item.ILOC.ToString().Equals("True"))
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