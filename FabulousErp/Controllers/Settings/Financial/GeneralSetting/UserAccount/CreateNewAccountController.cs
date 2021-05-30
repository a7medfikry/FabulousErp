using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.UserAccount;
using FabulousDB.DB_Tabels.Important;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FabulousErp.Repository.Interface;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.UserAccount
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class CreateNewAccountController : Controller
    {
        DBContext DB = new DBContext();

        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCNA")]
        // GET: CreateAccount
        public ActionResult NewAccountInfo()
        {
            return View();
        }

        public JsonResult AddAccount(string UserID, string UserName, string Password, bool ChangePassFirst, bool UpdateProfFirst, bool PasswordExpired, bool DisActive)
        {

            // Get image path  
            string imgPath = Server.MapPath("~/_Content/Pictures/profile.png");
            // Convert image to byte array  
            byte[] byteData = System.IO.File.ReadAllBytes(imgPath);

            string LastPasswordChangedDate = "";

            var check = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

            if (ChangePassFirst == false)
            {
                LastPasswordChangedDate = DateTime.Now.ToShortDateString();
            }
            else
            {
                LastPasswordChangedDate = null;
            }

            if (check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 4
                //AccountGroup_Table accountGroup_Table = new AccountGroup_Table()
                //{
                //    UserID = UserID,

                //    GroupID = "EmptyGroup",
                //};

                CreateAccount_Table createAccount_Table = new CreateAccount_Table()
                {
                    UserID = UserID,

                    UserName = UserName,

                    Password = Password,

                    ChangePassFirst = ChangePassFirst,

                    UpdateProfFirst = UpdateProfFirst,

                    Date = DateTime.Now.ToShortDateString(),

                    PasswordExpired = PasswordExpired,

                    DisActive = DisActive,

                    oldPassword = Password,

                    LastPasswordChangedDate = LastPasswordChangedDate,

                    ProfilePicByte = byteData,

                    Deleted = false,

                    DateOfAssignGroup = null
                };


                //FavouritesForms_Table favouritesForms_Table = new FavouritesForms_Table()
                //{
                //    UserID = UserID
                //};

                DB.CreateAccount_Tables.Add(createAccount_Table);
                // 5DB.AccountGroup_Tables.Add(accountGroup_Table);
                //DB.FavouritesForms_Tables.Add(favouritesForms_Table);
                DB.SaveChanges();

                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckUserID(string UserID)
        {
            var Check = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

            if (Check != null)
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAccountInfo(string UserID)
        {
            var GetInfo = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

            if (GetInfo != null)
            {

                R_User_Account_DTO r_User_Account_DTO = new R_User_Account_DTO()
                {
                    UserName = GetInfo.UserName,

                    Date = GetInfo.Date,

                    ChangePassFirst = GetInfo.ChangePassFirst,

                    Deleted = GetInfo.Deleted,

                    DisActive = GetInfo.DisActive,

                    Password = GetInfo.Password,

                    PasswordExpired = GetInfo.PasswordExpired,

                    UpdateProfFirst = GetInfo.UpdateProfFirst,
                };

                return Json(r_User_Account_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateAccount(string UserID, bool PasswordExpired, bool DisActive)
        {
            /*
            var GetAccointID = DB.createAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
            var AccID = GetAccointID.ID;
            */
            var UpdateAccount = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).First();

            UpdateAccount.PasswordExpired = PasswordExpired;
            UpdateAccount.DisActive = DisActive;

            DB.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAccount(string UserID, bool DeleteAccount)
        {
            /*
            var GetAccointID = DB.createAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
            var AccID = GetAccointID.ID;
            */
            var UpdateAccount = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).First();

            UpdateAccount.Deleted = DeleteAccount;

            DB.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
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
        //            item.SCNA = Value;
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
        //        if (item.SCNA.ToString().Equals("True"))
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