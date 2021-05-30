using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Post;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.Post
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class AccessOfUserPostController : Controller
    {

        DBContext DB = new DBContext();

        // GET: AccessOfUserPost
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SUP")]
        public ActionResult UserPost()
        {

            var getUsers = DB.CreateAccount_Tables.ToList();

            SelectList items = new SelectList(getUsers, "UserID", "UserID");

            ViewBag.UserList = items;

            return View();
        }



        public JsonResult GetUserName(string userID)
        {
            var getName = DB.CreateAccount_Tables.Where(x => x.UserID == userID).FirstOrDefault();

            return Json(getName.UserName, JsonRequestBehavior.AllowGet);
        }



        public JsonResult SaveUserPostAccess(string userID, string formCode)
        {
            var checkDuplicate = DB.User_Post_Tables.Where(x => x.UserID == userID && x.FormCode == formCode).FirstOrDefault();

            if(checkDuplicate != null)
            {
                return Json("This User Already has This Access..!");
            }
            else
            {
                User_Post_Table user_Post_Table = new User_Post_Table()
                {
                    UserID = userID,

                    FormCode = formCode
                };

                DB.User_Post_Tables.Add(user_Post_Table);
                DB.SaveChanges();

                return null;
            }
        }



        public JsonResult GetAccessOfUser(string userID)
        {
            List<User_Post_DTO> UserPost = DB.User_Post_Tables.Where(x => x.UserID == userID).Select(x => new User_Post_DTO
            {
                ID = x.UPID,

                UserID = x.UserID,

                FormCode = x.FormCode

            }).ToList();

            return Json(UserPost, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RemoveAccessOfUser(int userID)
        {
            var getUser = DB.User_Post_Tables.Where(x => x.UPID == userID).FirstOrDefault();
            DB.User_Post_Tables.Remove(getUser);
            DB.SaveChanges();

            return null;
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
        //            item.SUP = Value;
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
        //        if (item.SUP.ToString().Equals("True"))
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