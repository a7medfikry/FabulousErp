using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup;
using FabulousErp.MyRoleProvider;
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
    public class CreateNewGroupController : Controller
    {
        DBContext DB = new DBContext();

        // GET: CreateNewGroup
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCNG")]
        public ActionResult NewGroupInfo()
        {
            return View();
        }

        public JsonResult AddGroup(string GroupID, string GroupName, string GroupDescription, int FromCBF)
        {
            var Check = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).FirstOrDefault();

            if(Check != null)
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CreateGroup_Table createGroup_Table = new CreateGroup_Table()
                {
                    GroupID = GroupID,

                    GroupName = GroupName,

                    GroupDescription = GroupDescription,

                    Date = DateTime.Now.ToShortDateString(),

                    FromCBF = FromCBF
                };

                DB.CreateGroup_Tables.Add(createGroup_Table);
                DB.SaveChanges();

                return Json(JsonRequestBehavior.AllowGet);
            }
        }
        
        //public JsonResult AddFavourites(bool Value)
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json("",JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item != null)
        //        {
        //            item.SCNG = Value;
        //            DB.SaveChanges();
        //        }
        //        return Json("",JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult CheckFavourites()
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json("",JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item.SCNG.ToString().Equals("True"))
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