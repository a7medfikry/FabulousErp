using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.UserGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.UserGroup
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class ListOfGroupController : Controller
    {
        DBContext DB = new DBContext();

        // GET: ListOfGroup
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SLOG")]
        public ActionResult GetGroups()
        {
            var UserList = DB.CreateGroup_Tables.Where(x => x.Deleted == false || x.Deleted == null).ToList();
            List<List_Of_Group> result = new List<List_Of_Group>();
            foreach (var item in UserList)
            {
                List_Of_Group Users = new List_Of_Group()
                {
                    GroupID = item.GroupID,

                    GroupName = item.GroupName,

                    GroupDescription = item.GroupDescription,

                    Date = item.Date,

                    Deleted = item.Deleted.GetValueOrDefault(),

                    DisActive = item.DisActive.GetValueOrDefault()
                };

                result.Add(Users);

            }

            return View(result);
        }

        [HttpPost]
        public ActionResult GetGroups(List<List_Of_Group> list_Of_Group)
        {
            if (ModelState.IsValid)
            {
                foreach (var check in list_Of_Group)
                {
                    var item = DB.CreateGroup_Tables.Where(x => x.GroupID == check.GroupID).FirstOrDefault();

                    if (item != null)
                    {
                        item.Deleted = check.Deleted;

                        item.DisActive = check.DisActive;
                    }
                }
                DB.SaveChanges();
                return RedirectToAction("GetGroups", "ListOfGroup");
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
        //            item.SLOG = Value;
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
        //        if (item.SLOG.ToString().Equals("True"))
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