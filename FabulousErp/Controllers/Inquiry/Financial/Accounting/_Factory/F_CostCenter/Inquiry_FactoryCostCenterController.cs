using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting._Factory.F_CostCenter
{
    [AuthorizationFilter]
    public class Inquiry_FactoryCostCenterController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public Inquiry_FactoryCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: InquiryFactoryCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IFCC")]
        public ActionResult FactoryCostCenter()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                ViewBag.CodeList = repetitionBusiness.RetrieveFactoryIDListCond(companyID);
            }
          
            return View();
        }

        public JsonResult GetFactoryName(string factoryID)
        {
            var list = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == factoryID).FirstOrDefault();
            if (list != null)
            {
                string FactoryName = list.FactoryName;
                return Json(FactoryName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetData(string factoryID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UAFactoryPremission_Tables.Where(x => x.UserID == userID && x.FactoryID == factoryID).FirstOrDefault();
            if (check != null)
            {
                var list = DB.F_CostCenter_Tables.Where(x => x.FactoryID == factoryID).ToList();
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
        //            item.IFCC = Value;
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
        //        if (item.IFCC.ToString().Equals("True"))
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