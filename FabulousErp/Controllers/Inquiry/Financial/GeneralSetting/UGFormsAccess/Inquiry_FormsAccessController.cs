using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;

namespace FabulousErp.Controllers.Inquiry.Financial.GeneralSetting.UGFormsAccess
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_FormsAccessController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public Inquiry_FormsAccessController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: Inquiry_FormsAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IUFA")]
        public ActionResult FormsAccess()
        {
            ViewBag.CodeList = repetitionBusiness.RetrieveUserIDList();

            return View();
        }

        public JsonResult GetUserName(string userID)
        {
            return Json(repetitionBusiness.GetUserName(userID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFormsAccess(string userID)
        {
            // 6
            DB.Configuration.ProxyCreationEnabled = false;
            var list = DB.UserFormsAccess_Tables.Where(x => x.UserID == userID).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
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
        //            item.IUFA = Value;
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
        //        if (item.IUFA.ToString().Equals("True"))
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