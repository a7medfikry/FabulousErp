using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using FabulousErp.MyRoleProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Inquiry.Financial.GeneralSetting.UserAccount
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_UserDataController : Controller
    {
        DBContext DB = new DBContext();

        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ILOU")]
        public ActionResult ShowListofUsers()
        {
            var list = DB.CreateAccount_Tables.ToList();
            SelectList UserID = new SelectList(list, "UserID", "UserID");
            SelectList UserName = new SelectList(list, "UserName", "UserName");
            ViewBag.CodeList = UserID;
            ViewBag.CodeList2 = UserName;
            return View();
        }

        public JsonResult GetData()
        {
            List<CreateAccount_Table> createAccount_Tables = new List<CreateAccount_Table>();
            createAccount_Tables = DB.CreateAccount_Tables.ToList();
            return Json(createAccount_Tables, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserIDSearch(string InquiryuserID)
        {
            List<CreateAccount_Table> createAccount_Tables = new List<CreateAccount_Table>();
            createAccount_Tables = DB.CreateAccount_Tables.Where(x => x.UserID == InquiryuserID).ToList();
            return Json(createAccount_Tables, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserNameSearch(string InquiryuserName)
        {
            List<CreateAccount_Table> CreateAccount_Table = new List<CreateAccount_Table>();
            CreateAccount_Table = DB.CreateAccount_Tables.Where(x => x.UserName == InquiryuserName).ToList();
            return Json(CreateAccount_Table, JsonRequestBehavior.AllowGet);
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
        //            item.ILOU = Value;
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
        //        if (item.ILOU.ToString().Equals("True"))
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