using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.UserAccount
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class ListOfUserController : Controller
    {
        DBContext DB = new DBContext();

        // GET: ListOfUser
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SLOU")]
        public ActionResult GetUsers()
        {
            var UserList = DB.CreateAccount_Tables.Where(x => x.Deleted == false || x.Deleted == null).ToList();

            List<List_Of_User> result = new List<List_Of_User>();

            foreach (var item in UserList)
            {
                string groupID = "Empty";
                if(item.UserGroup_Table != null)
                {
                    groupID = item.UserGroup_Table.GroupID;
                }

                List_Of_User Users = new List_Of_User()
                {
                    UserID = item.UserID,

                    UserName = item.UserName,

                    MembOfGroup = groupID,

                    Date = item.Date,

                    Deleted = item.Deleted.GetValueOrDefault(),

                    DisActive = item.DisActive.GetValueOrDefault()
                };

                result.Add(Users);

            }
            return View(result);
        }

        [HttpPost]
        public ActionResult GetUsers(List<List_Of_User> list_Of_User)
        {

            if (ModelState.IsValid)
            {
                foreach (var check in list_Of_User)
                {
                    var item = DB.CreateAccount_Tables.Where(x => x.UserID == check.UserID).FirstOrDefault();

                    if (item != null)
                    {
                        item.Deleted = check.Deleted;

                        item.DisActive = check.DisActive;
                    }
                }
                DB.SaveChanges();
                return RedirectToAction("GetUsers", "ListOfUser");
            }
            return View();
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
        //            item.SLOU = Value;
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
        //        if (item.SLOU.ToString().Equals("True"))
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